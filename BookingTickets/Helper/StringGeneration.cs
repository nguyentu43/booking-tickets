using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTickets.Helper
{
    public static class StringGeneration
    {
        public static string Unique()
        {
            Guid g = Guid.NewGuid();
            return g.ToString().Replace("-", "");
        }
    }
}
