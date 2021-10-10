using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Helper
{
    public static class IFormFileExtension
    {
        public static async Task CopyToPath(this IFormFile file, string path)
        {
            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
