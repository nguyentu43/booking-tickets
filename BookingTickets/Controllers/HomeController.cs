﻿using BookingTickets.Data.Base;
using BookingTickets.Models;
using BookingTickets.Models.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SearchFormVM form)
        {
            var whereBuilder = PredicateBuilder.New<Movie>(true);
            if (form.Keyword != null && form.Keyword != "")
            {
                whereBuilder = whereBuilder.And(m => m.Title.ToLower().Contains(form.Keyword.ToLower()));
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

            var model = new IndexHomeVM
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
            return View(new MovieDetailVM { Movie = movie, Rooms = rooms });
        }

        public async Task<IActionResult> MyReservations(string phone)
        {
            List<int> reservations = null;
            if (!User.Identity.IsAuthenticated)
            {
                reservations = await _unitOfWork.ReservationRepository.DbSet.Where(r => r.Phone == phone).Select(r => r.Id).ToListAsync();
            }
            else
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                reservations = await _unitOfWork.ReservationRepository.DbSet.Where(r => r.CustomerId == user.Id).Select(r => r.Id).ToListAsync();
            }
            return View(reservations);
        }

        [HttpGet("/ajax/getScreenings")]
        public async Task<IActionResult> GetScreenings(int roomId, int movieId, string screeningDate)
        {
            var screeningDateObj = DateTime.Parse(screeningDate);
            var screenings = await _unitOfWork.ScreeningRepository.DbSet
                .Where(sc => sc.RoomId == roomId && sc.MovieId == movieId
                             && sc.ScreeningStart >= DateTime.Now
                             && sc.ScreeningStart.Date == screeningDateObj.Date
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
                _unitOfWork.ReservationRepository.Add(reservation);
                await _unitOfWork.SaveChangeAsync();

                foreach (var rs in reservationSeats)
                {
                    rs.ReservationId = reservation.Id;
                    _unitOfWork.ReservationSeatRepository.Add(rs);
                }

                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception)
            {
                return Ok(new { error = "Booking ticket has errors" });
            }

            return Ok(new { message = "Booking ticket successful" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}