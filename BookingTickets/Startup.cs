using AutoMapper;
using BookingTickets.Constants;
using BookingTickets.Data;
using BookingTickets.Data.Base;
using BookingTickets.Mapper;
using BookingTickets.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BookingTickets
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine).EnableSensitiveDataLogging();
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.AllowedForNewUsers = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IDbInit, DbInit>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Manage", policy => policy.RequireRole(Role.Admin.ToString()));
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin", "Manage");
                options.Conventions.AddPageRoute("/Admin/Cinema/Room/Index", "/Admin/Cinema/{CinemaId}/Room");
                options.Conventions.AddPageRoute("/Admin/Cinema/Room/Edit", "/Admin/Cinema/{CinemaId}/Room/Edit/{id?}");
                options.Conventions.AddPageRoute("/Admin/Movie/Screening/Index", "/Admin/Movie/{MovieId}/Screening");
                options.Conventions.AddPageRoute("/Admin/Movie/Screening/Edit", "/Admin/Movie/{MovieId}/Screening/Edit/{id?}");
            }).AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
            });

            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IDbInit>().GenerateInitData();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
