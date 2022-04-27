using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using BotDetect.Web;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public HomeController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var mostViewedMovies = (await _unitOfWork.ReservationRepository.DbSet.Include(r => r.Screening).ThenInclude(s => s.Movie).Select(r => r.Screening.Movie).ToListAsync()).GroupBy(m => m.Id).OrderByDescending(g => g.Count()).Select(g => g.AsEnumerable().FirstOrDefault()).ToList();
            var carouselMovies = await _unitOfWork.MovieRepository.DbSet.Take(10).ToListAsync();
            var newestMovies = await _unitOfWork.MovieRepository.DbSet.OrderByDescending(m => m.ReleaseDate).Take(10).ToListAsync();
            var genres = await _unitOfWork.GenreRepository.DbSet.Include(g => g.Movies).OrderBy(g => g.Name).ToListAsync();

            return View(new IndexVM
            {
                CarouselMovies = carouselMovies,
                Genres = genres,
                MostViewedMovies = mostViewedMovies,
                NewestMovies = newestMovies
            });
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchFormVM form)
        {
            var whereBuilder = PredicateBuilder.New<Movie>(true);
            if (form.Keyword != null && form.Keyword != "")
            {
                var keyword = form.Keyword.ToLower();
                whereBuilder = whereBuilder
                                .And(m => m.Title.ToLower().Contains(keyword) || m.Director.ToLower().Contains(keyword)
                                || m.Cast.ToLower().Contains(keyword));
            }
            if (form.SelectedGenre > 0)
            {
                whereBuilder = whereBuilder.And(m => m.Genres.Any(g => g.Id == form.SelectedGenre));
            }

            var movies = await _unitOfWork.MovieRepository.DbSet
                .Include(m => m.Genres)
                .Where(whereBuilder)
                .OrderBy(m => m.ReleaseDate)
                .Skip((form.Page - 1) * form.PageSize)
                .Take(form.PageSize)
                .ToListAsync();
            var moviesTotal = await _unitOfWork.MovieRepository.DbSet.Where(whereBuilder).CountAsync();
            form.PageTotal = (int)moviesTotal / form.PageSize;
            if (moviesTotal % form.PageSize != 0)
            {
                form.PageTotal++;
            }

            var model = new SearchVM
            {
                Movies = movies,
                Genres = await _unitOfWork.GenreRepository.GetAll().ToListAsync(),
                Form = form
            };
            return View(model);
        }

        [HttpGet("/movie/{id}")]
        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            var rooms = await _unitOfWork.RoomRepository.GetAllWithCinema().ToListAsync();
            var rates = await _unitOfWork.RateRepository.DbSet.Include(r => r.Reservation).ThenInclude(r => r.Customer).Where(r => r.Reservation.Screening.MovieId == id).ToListAsync();
            return View(new MovieDetailVM { Movie = movie, Rooms = rooms, Rates = rates });
        }

        public async Task<IActionResult> MyReservations(string phone)
        {
            Func<Reservation, bool> query = null;
            if (!User.Identity.IsAuthenticated)
            {
                query = (r => r.Phone == phone);
            }
            else
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                query = r => r.CustomerId == user.Id;
            }
            List<int> reservations = _unitOfWork.ReservationRepository.DbSet.Where(query).Select(r => r.Id).ToList();
            return View(reservations);
        }

        [HttpGet("/ajax/getScreenings")]
        public async Task<IActionResult> GetScreenings(int roomId, int movieId, DateTime screeningDate)
        {
            var screenings = await _unitOfWork.ScreeningRepository.DbSet
                .Where(sc => sc.RoomId == roomId && sc.MovieId == movieId
                             && sc.ScreeningStart >= DateTime.Now
                             && sc.ScreeningStart.Date == screeningDate.Date
                )
                .OrderBy(sc => sc.ScreeningStart)
                .ToListAsync();
            return Ok(screenings);
        }

        [HttpGet("/ajax/getAvailableSeats")]
        public async Task<IActionResult> GetAvailableSeats(int screeningId)
        {
            var reserverdSeats = await _unitOfWork.ReservationRepository.DbSet
                                .Where(r => r.ScreeningId == screeningId)
                                .Include(r => r.Seats)
                                .SelectMany(r => r.Seats)
                                .Select(s => s.Id).ToListAsync();
            var screening = await _unitOfWork.ScreeningRepository.DbSet
                        .Include(sc => sc.Room).ThenInclude(r => r.Seats)
                        .Where(sc => sc.Id == screeningId)
                        .SingleOrDefaultAsync();
            var seats = screening.Room.Seats
                        .Select(s => new
                        {
                            id = s.Id,
                            row = s.Row,
                            col = s.Column,
                            type = s.SeatType.ToString(),
                            available = !reserverdSeats.Contains(s.Id)
                        })
                        .OrderBy(s => s.row)
                        .OrderBy(s => s.col)
                        .GroupBy(s => s.row)
                        .ToList();
            return Ok(seats);
        }

        [HttpPost("/ajax/booking")]
        public async Task<IActionResult> BookingTicket([FromBody] ReservationFormVM form)
        {
            SimpleCaptcha captcha = new SimpleCaptcha();
            bool isHuman = captcha.Validate(form.Captcha, form.CaptchaId);

            if (isHuman == false)
            {
                return Ok(new { error = "Captcha invalid" });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new { error = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(err => err.ErrorMessage).ToList()) });
            }

            var screening = await _unitOfWork.ScreeningRepository.DbSet.Where(sc => sc.Id == form.ScreeningId).SingleOrDefaultAsync();
            var seats = await _unitOfWork.SeatRepository.DbSet.Where(s => form.Seats.Contains(s.Id)).ToListAsync();

            if (screening.ScreeningStart < DateTime.Now)
            {
                return Ok(new { error = "Screening has been end" });
            }

            if (seats.Count != form.Seats.Count)
            {
                return Ok(new { error = "Seats not found" });
            }

            var reservation = new Reservation
            {
                Name = form.Name,
                Email = form.Email,
                Phone = form.Phone,
                ScreeningId = form.ScreeningId,
                ReservationDate = DateTime.Now,
            };

            if (User.Identity?.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                reservation.CustomerId = user.Id;
            }

            var total = 0d;
            var reservationSeats = new List<ReservationSeat>();
            foreach (var seat in seats)
            {
                var rs = new ReservationSeat();
                rs.SeatId = seat.Id;
                rs.SeatName = seat.SeatType.ToString() + "-" + Convert.ToChar(seat.Row + 65).ToString() + seat.Column;
                if (seat.SeatType == Constants.SeatType.VIP)
                {
                    total += screening.Price * 1.1d;
                    rs.Price = screening.Price * 1.1d;
                }
                else if (seat.SeatType == Constants.SeatType.NORMAL)
                {
                    total += screening.Price;
                    rs.Price = screening.Price;
                }
                reservationSeats.Add(rs);
            }
            reservation.Price = total;

            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeApi:Sk"].ToString();

                var tokenService = new TokenService();
                var token = await tokenService.CreateAsync(new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Cvc = form.Cvc,
                        Number = form.CardNumber,
                        ExpMonth = form.CardDate.Month,
                        ExpYear = form.CardDate.Year
                    }
                });

                var charge = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(total * 100),
                    Currency = "usd",
                    Source = token.Id
                };

                var chargeService = new ChargeService();
                await chargeService.CreateAsync(charge);

                _unitOfWork.ReservationRepository.Add(reservation);
                await _unitOfWork.SaveChangeAsync();

                foreach (var rs in reservationSeats)
                {
                    rs.ReservationId = reservation.Id;
                    _unitOfWork.ReservationSeatRepository.Add(rs);
                }

                await _unitOfWork.SaveChangeAsync();
            }
            catch (StripeException e)
            {
                return Ok(new { error = e.StripeError.Message });
            }
            catch (Exception)
            {
                return Ok(new { error = "Booking ticket has errors" });
            }

            return Ok(new { message = "Booking ticket successful" });
        }

        [HttpPost("/ajax/rate")]
        public async Task<IActionResult> SaveRate([FromBody] RateFormVM rate)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { error = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(err => err.ErrorMessage).ToList()) });
            }

            try
            {
                _unitOfWork.RateRepository.Add(_mapper.Map(rate, new Rate { RatedDate = DateTime.Now }));
                await _unitOfWork.SaveChangeAsync();
                return Ok(new { message = "Rate saved" });
            }catch(Exception)
            {
                return Ok(new { error = "Error add rate" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
