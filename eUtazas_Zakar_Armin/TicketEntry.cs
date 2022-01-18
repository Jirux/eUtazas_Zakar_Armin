using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eUtazas_Zakar_Armin
{
    public class TicketEntry
    {
        public TicketEntry(int station, DateTime time, int id, TicketType type, DateTime expiry, int uses)
        {
            Station = station;
            Time = time;
            this.id = id;
            Type = type;
            Expiry = expiry;
            Uses = uses;
        }

        public int Station { get; }
        public DateTime Time { get; }
        public int id { get; }
        public TicketType Type { get; }
        public DateTime Expiry { get; }
        public int Uses { get; }
    }
}
