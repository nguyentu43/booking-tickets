using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BookingTickets.Constants;
using BookingTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingTickets.Data
{
    public class DbInit : IDbInit
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
       
        public DbInit(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void GenerateInitData()
        {
            if(_dbContext.Database.GetPendingMigrations().Count() > 0)
            {
                _dbContext.Database.Migrate();
            }

            if (_dbContext.Users.Count() > 0) return;

            _roleManager.CreateAsync(new IdentityRole
            {
                Name = Role.Admin.ToString()
            }).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole
            {
                Name = Role.Customer.ToString()
            }).GetAwaiter().GetResult();

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                Name = "Admin"
            };
            var user1 = new ApplicationUser
            {
                UserName = "user1",
                Email = "user1@example.com",
                Name = "User1"
            };

            _userManager.CreateAsync(admin, "Abcd@123").GetAwaiter().GetResult();
            _userManager.CreateAsync(user1, "Abcd@123").GetAwaiter().GetResult();

            _userManager.AddToRoleAsync(admin, Role.Admin.ToString()).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user1, Role.Customer.ToString()).GetAwaiter().GetResult();

            var genre1 = new Genre
            {
                Name = "Action"
            };
            var genre2 = new Genre
            {
                Name = "Cartoon"
            };
            var genre3 = new Genre
            {
                Name = "Adventure"
            };
            _dbContext.Genres.AddRange(genre1, genre2, genre3);
            _dbContext.SaveChanges();

            var movie1 = new Movie
            {
                Title = "Luca",
                Description = "Luca and his best friend Alberto experience an unforgettable summer on the Italian Riviera. But all the fun is threatened by a deeply-held secret: they are sea monsters from another world just below the water’s surface.",
                Director = "Enrico Casarosa",
                Cast = "Jesse Andrews",
                ReleaseDate = DateTime.Parse("6/17/2021"),
                Language = "English",
                Duration = 95,
                Rated = "PG",
                Cover = "c0509309e7754fbd9311adf668a742f4.jpg"
            };
            var movie2 = new Movie
            {
                Title = "PAW Patrol: The Movie",
                Description = "Ryder and the pups are called to Adventure City to stop Mayor Humdinger from turning the bustling metropolis into a state of chaos.",
                Director = "Cal Brunker",
                Cast = "Kim Kardashian, Iain Armitage, Lilly Bartlam, Marsai Martin",
                ReleaseDate = DateTime.Parse("10/8/2021"),
                Language = "English",
                Duration = 86,
                Rated = "PG",
                Cover = "6713bcbcc5bf4e2ba07cc921042d19d8.jpg"
            };
            _dbContext.Movies.AddRange(movie1, movie2);
            _dbContext.SaveChanges();

            _dbContext.MovieGenre.Add(new MovieGenre { GenreId = genre1.Id, MovieId = movie1.Id });
            _dbContext.MovieGenre.Add(new MovieGenre { GenreId = genre2.Id, MovieId = movie1.Id });
            _dbContext.MovieGenre.Add(new MovieGenre { GenreId = genre1.Id, MovieId = movie2.Id });
            _dbContext.MovieGenre.Add(new MovieGenre { GenreId = genre2.Id, MovieId = movie2.Id });
            _dbContext.SaveChanges();
        }
    }
}
