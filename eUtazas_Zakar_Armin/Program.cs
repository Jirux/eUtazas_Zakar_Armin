using System.Globalization;

namespace eUtazas_Zakar_Armin
{
    internal class Program
    {
        private List<TicketEntry> entries = new List<TicketEntry>();
        public Program()
        {
            Task1();
            Task2();
            Task3();
            Task4();
            Task5();
            Task7();
        }

        private void Task1()
        {
            foreach (var line in File.ReadAllLines("utasadat.txt"))
            {
                string[] data = line.Split(" ");
                int station = int.Parse(data[0]);
                DateTime time = DateTime.ParseExact(data[1], "yyyyMMdd-hhmm", CultureInfo.InvariantCulture);
                int id = int.Parse(data[2]);
                TicketType type = (TicketType)Enum.Parse(typeof(TicketType), data[3]);
                string expiryCandidate = data[4];
                DateTime expiry;
                int uses = 0;
                if (expiryCandidate.Length <= 2)
                {
                    expiry = time;
                    uses = int.Parse(expiryCandidate);
                }
                else
                {
                    expiry = DateTime.ParseExact(expiryCandidate, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                entries.Add(new TicketEntry(station, time, id, type, expiry, uses));
            }
        }

        private void Task2()
        {
            Console.WriteLine("2. feladat");
            Console.WriteLine("A buszra {0} utas akart felszállni.", entries.Count);
            Console.WriteLine();
        }

        private void Task3()
        {
            int expiredTickets = entries.FindAll(e => !validTicket(e)).Count;
            Console.WriteLine("3. feladat");
            Console.WriteLine("A buszra {0} utas nem szállhatott fel.", expiredTickets);
            Console.WriteLine();
        }

        private void Task4()
        {
            var stationDict = new Dictionary<int, int>();
            entries.ForEach(e =>
            {
                if (stationDict.ContainsKey(e.Station))
                    stationDict[e.Station] += 1;
                else
                    stationDict[e.Station] = 1;
            });
            int mostPeople = stationDict.Values.Max();
            int mostPeopleStation = stationDict.Keys.Where(i => stationDict[i] == mostPeople).Min();
            Console.WriteLine("4. feladat");
            Console.WriteLine("A legtöbb utas ({0} fő) a {1}. megállóban próbált felszállni.", stationDict[mostPeopleStation], mostPeopleStation);
            Console.WriteLine();
        }

        private void Task5()
        {
            int freeUses = entries.FindAll(e => freeType(e.Type) && validTicket(e)).Count;
            int discountedUses = entries.FindAll(e => discountedType(e.Type) && validTicket(e)).Count;
            Console.WriteLine("5. feladat");
            Console.WriteLine("Ingyenesen utazók száma: {0} fő", freeUses);
            Console.WriteLine("A kedvezményesen utazók száma: {0} fő", discountedUses);
            Console.WriteLine();
        }

        private int napokszama(int e1, int h1, int n1, int e2, int h2, int n2)
        {
            h1 = (h1 + 9) % 12;
            e1 -= h1 / 10;
            int d1 = 365 * e1 + e1 / 4 - e1 / 100 + e1 / 400 +
                (h1 * 306 + 5) / 10 + n1 - 1;
            h2 = (h2 + 9) % 12;
            e2 -= h2 / 10;
            int d2 = 365 * e2 + e2 / 4 - e2 / 100 + e2 / 400 +
                (h2 * 306 + 5) / 10 + n2 - 1;
            return d2 - d1;
        }

        private void Task7()
        {
            List<string> output = new List<string>();
            entries.DistinctBy(e => e.id).ToList().ForEach(entry =>
            {
                if ((entry.Expiry - entry.Time).TotalDays < 3)
                    output.Add(String.Format("{0} {1}-{2}-{3}", entry.id, entry.Expiry.Year, entry.Expiry.Month, entry.Expiry.Day));
            });
            File.WriteAllLines("figyelmeztetes.txt", output);
        }

        private bool discountedType(TicketType type)
        {
            return type == TicketType.TAB || type == TicketType.NYB;
        }

        private bool freeType(TicketType type)
        {
            return type == TicketType.NYP || type == TicketType.RVS || type == TicketType.GYK;
        }

        private bool validTicket(TicketEntry entry)
        {
            if (entry.Type == TicketType.JGY)
                return entry.Uses > 0;
            else
                return entry.Time <= entry.Expiry || (entry.Time.Year == entry.Expiry.Year &&
                                                      entry.Time.Month == entry.Expiry.Month &&
                                                      entry.Time.Day == entry.Expiry.Day);
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}