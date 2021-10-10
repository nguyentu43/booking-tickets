using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingTickets.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<List<dynamic>> Reports { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void OnGet(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate == DateTime.MinValue ? DateTime.Now : startDate;
            EndDate = endDate == DateTime.MinValue ? DateTime.Now : endDate;

            var groups = unitOfWork.ReservationRepository.DbSet
                .Include(r => r.Seats)
                .Where(r => r.ReservationDate >= StartDate && r.ReservationDate <= EndDate)
                .AsEnumerable()
                .GroupBy(r => new { Month = r.ReservationDate.Month, Year = r.ReservationDate.Year })
                .ToList();

            Reports = new List<List<dynamic>>();
            var header = new List<dynamic>();
            header.AddRange(new string[] { "Month/Year", "Revenue" });
            Reports.Add(header);
            foreach (var group in groups)
            {
                var row = new List<dynamic>();
                row.Add(group.Key.Month + "/" + group.Key.Year);
                row.Add(group.Sum(r => r.Price));
                Reports.Add(row);
            }
        }
    }
}
