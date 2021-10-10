using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTickets.Data.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models = BookingTickets.Models;

namespace BookingTickets.Pages.Admin.Cinema.Room
{
    public class IndexModel : PageModel
    {
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork { get; }
        public List<Models.Room> Rooms { get; set; }
        public async Task OnGetAsync(int cinemaId)
        {
            Rooms = await _unitOfWork.RoomRepository.DbSet.Where(r => r.CinemaId == cinemaId).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var deleteRoom = await _unitOfWork.RoomRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id);
            if(deleteRoom != null)
            {
                _unitOfWork.RoomRepository.Remove(deleteRoom);
                await _unitOfWork.SaveChangeAsync();
                TempData["message"] = "Delete successful";
            }
            else
            {
                TempData["error_message"] = "Room not found";
            }
            return RedirectToPage();
        }
    }
}
