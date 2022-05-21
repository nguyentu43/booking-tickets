using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Helper;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Movie
{
    public class EditModel : PageModel
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public IConfiguration configuration;
        public EditModel(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            configuration = config;
        }
        [BindProperty]
        public MovieVM Movie { get; set; }
        public List<Models.Genre> Genres { get; set; }

        public long Timestamp { 
            get{
                return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            } 
        }

        public string Signature { 
            get {
                string data =
                    string.Format("cloud_name={0}&timestamp={1}&username={2}{3}",
                    configuration["Cloudinary:CloudName"],
                    Timestamp,
                    configuration["Cloudinary:Username"],
                    configuration["Cloudinary:ApiSecret"]
                );

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            } 
        }
        public async Task OnGetAsync(int? id = null)
        {
            Movie = _mapper.Map<Models.Movie, MovieVM>(await _unitOfWork.MovieRepository.DbSet.Include(m => m.Genres).SingleOrDefaultAsync(m => m.Id == id));
            Genres = await _unitOfWork.GenreRepository.GetAll().ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            Genres = await _unitOfWork.GenreRepository.GetAll().ToListAsync();
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    var movie = await _unitOfWork.MovieRepository.Add(Movie);
                    await _unitOfWork.SaveChangeAsync();

                    foreach (var g in Movie.Genres)
                    {
                        _unitOfWork.MovieGenreRepository.Add(new Models.MovieGenre { GenreId = g, MovieId = movie.Id });
                    }
                    await _unitOfWork.SaveChangeAsync();

                    TempData["message"] = "Create successful";
                    return RedirectToPage(new { id = movie.Id });
                }
                else
                {
                    var movie = await _unitOfWork.MovieRepository.Update((int)id, Movie);
                    await _unitOfWork.SaveChangeAsync();

                    var movieGenres = await _unitOfWork.MovieGenreRepository.DbSet.Where(mg => mg.MovieId == movie.Id).ToListAsync();
                    _unitOfWork.MovieGenreRepository.DbSet.RemoveRange(movieGenres);
                    await _unitOfWork.SaveChangeAsync();

                    foreach (var g in Movie.Genres)
                    {
                        _unitOfWork.MovieGenreRepository.Add(new Models.MovieGenre { GenreId = g, MovieId = movie.Id });
                    }
                    await _unitOfWork.SaveChangeAsync();

                    TempData["message"] = "Update successful";
                    return RedirectToPage(new { id = movie.Id });
                }
            }
            return Page();
        }
    }
}
