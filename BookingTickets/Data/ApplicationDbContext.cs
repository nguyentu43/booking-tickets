using BookingTickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace BookingTickets.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<MovieGenre>(
                    mg => mg.HasOne(mg => mg.Genre).WithMany().HasForeignKey(mg => mg.GenreId),
                    mg => mg.HasOne(mg => mg.Movie).WithMany().HasForeignKey(mg => mg.MovieId)
                );

            builder.Entity<Reservation>()
                .HasMany(r => r.Seats)
                .WithMany(s => s.Reservations)
                .UsingEntity<ReservationSeat>(
                    rs => rs.HasOne(rs => rs.Seat).WithMany().HasForeignKey(rs => rs.SeatId).OnDelete(DeleteBehavior.NoAction),
                    rs => rs.HasOne(rs => rs.Reservation).WithMany().HasForeignKey(rs => rs.ReservationId)
                );
        }
    }
}
