using AutoMapper;
using BookingTickets.Data.Base;
using BookingTickets.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Pages.Admin.Cinema.Room
{
    public class EditModel : PageModel
    {
        public EditModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private IUnitOfWork _unitOfWork { get; }
        private IMapper _mapper { get; }
        [BindProperty]
        public RoomVM Room { get; set; }
        [BindProperty]
        public List<SeatVM> Seats { get; set; }
        public List<IGrouping<int, SeatVM>> SeatGroups { get; set; }
        public Models.Cinema Cinema { get; set; }
        public async Task OnGetAsync(int cinemaId, int? id = null)
        {
            Cinema = await _unitOfWork.CinemaRepository.DbSet.SingleOrDefaultAsync(c => c.Id == cinemaId);
            if (Cinema == null) throw new Exception("Cinema not found");
            Room = _mapper.Map<RoomVM>(await _unitOfWork.RoomRepository.DbSet.SingleOrDefaultAsync(g => g.Id == id));
            var seats = _mapper.Map<List<SeatVM>>(await _unitOfWork.SeatRepository.DbSet.Where(s => s.RoomId == id).ToListAsync());
            SeatGroups = seats.GroupBy(s => s.Row).OrderBy(g => g.Key).ToList();
        }
        public async Task<IActionResult> OnPostAsync(int cinemaId, int? id = null)
        {
            if (ModelState.IsValid)
            {
                var editRoom = await _unitOfWork.RoomRepository.DbSet.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
                var room = _mapper.Map<RoomVM, Models.Room>(Room);
                room.CinemaId = cinemaId;
                if (editRoom == null)
                {
                    _unitOfWork.RoomRepository.Add(room);
                    await _unitOfWork.SaveChangeAsync();
                    for (int row = 0; row < 7; row++)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            if (row > 3)
                            {
                                _unitOfWork.SeatRepository.Add(new Models.Seat { Row = row, Column = col, RoomId = room.Id, SeatType = Constants.SeatType.VIP });
                            }
                            else
                            {
                                _unitOfWork.SeatRepository.Add(new Models.Seat { Row = row, Column = col, RoomId = room.Id, SeatType = Constants.SeatType.NORMAL });
                            }
                        }
                    }
                    await _unitOfWork.SaveChangeAsync();
                    TempData["message"] = "Create successful";
                }
                else
                {
                    room.Id = (int)id;
                    _unitOfWork.RoomRepository.Update(room);
                    await _unitOfWork.SaveChangeAsync();
                    _unitOfWork.SeatRepository.DbSet.UpdateRange(_mapper.Map<List<Models.Seat>>(Seats));
                    await _unitOfWork.SaveChangeAsync();
                    TempData["message"] = "Update successful";
                }
                return RedirectToPage(new { id = room.Id });
            }
            return Page();
        }
    }
}
