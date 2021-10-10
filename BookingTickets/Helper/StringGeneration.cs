using System;

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
