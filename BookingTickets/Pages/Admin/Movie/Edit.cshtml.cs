using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Helper;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Movie
{
    public class EditModel : PageModel
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IWebHostEnvironment _enviroment;
        public EditModel(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _enviroment = environment;
        }
        [BindProperty]
        public MovieVM Movie { get; set; }
        public List<Models.Genre> Genres { get; set; }
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
