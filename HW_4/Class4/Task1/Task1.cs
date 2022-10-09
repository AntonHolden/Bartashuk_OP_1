using static Task1.Task1;

namespace Task1
{
    using System.Text;
    // Необходимо заменить на более подходящий тип (коллекцию), позволяющий
    // эффективно искать диапазон по заданному IP-адресу
    using IPRangesDatabase = List<Tuple<IPv4Addr, IPv4Addr>>;

    public class Task1
    {
        /*
        * Объекты этого класса создаются из строки, но хранят внутри помимо строки
        * ещё и целочисленное значение соответствующего адреса. Например, для адреса
         * 127.0.0.1 должно храниться число 1 + 0 * 2^8 + 0 * 2^16 + 127 * 2^24 = 2130706433.
        */
        internal record IPv4Addr : IComparable<IPv4Addr>
        {
            internal ulong IntValue;
            private string StrValue { get; init; }
            public IPv4Addr(string StrValue)
            {
                this.StrValue = StrValue;
                IntValue = Ipstr2Int(StrValue);
            }

            private static ulong Ipstr2Int(string StrValue)
            {
                ulong res = 0;

                var strNums = StrValue.Split('.');
                Array.Reverse(strNums);
                ulong mult = 1;

                foreach (string strNum in strNums)
                {
                    res += ulong.Parse(strNum) * mult;
                    mult *= (ulong)Math.Pow(2, 8);
                }

                return res;
            }

            // Благодаря этому методу мы можем сравнивать два значения IPv4Addr
            public int CompareTo(IPv4Addr other)
            {
                return IntValue.CompareTo(other.IntValue);
            }

            public override string ToString()
            {
                return StrValue;
            }
        }

        internal record class IPRange(IPv4Addr IpFrom, IPv4Addr IpTo)
        {
            public override string ToString()
            {
                return $"{IpFrom},{IpTo}";
            }
        }

        internal record class IPLookupArgs(string IpsFile, List<string> IprsFiles);

        internal static IPLookupArgs? ParseArgs(string[] args)
        {
            try
            {
                if (args.Length < 2) return null;

                string ipsFile = args[0];
                var iprsFiles = new List<string>();
                for (uint i = 1; i < args.Length; i++)
                {
                    iprsFiles.Add(args[i]);
                }
                return new IPLookupArgs(ipsFile, iprsFiles);
            }
            catch { return null; }
        }

        internal static List<string> LoadQuery(string filename)
        {
            string dirPath = Directory.GetCurrentDirectory();
            dirPath = dirPath.Substring(0, dirPath.Length - 16);

            return File.ReadAllLines(dirPath + filename).ToList();
        }

        internal static IPRangesDatabase LoadRanges(List<String> filenames)
        {
            string dirPath = Directory.GetCurrentDirectory();
            dirPath = dirPath.Substring(0, dirPath.Length - 16);

            var ranges = new List<string>();

            foreach (var filename in filenames)
            {
                ranges.AddRange(File.ReadAllLines(dirPath + filename).ToList());
            }

            var res = new IPRangesDatabase();

            foreach (var range in ranges)
            {
                var splitter = range.Split(',');

                res.Add(new Tuple<IPv4Addr, IPv4Addr> (new IPv4Addr(splitter[0]), new IPv4Addr(splitter[1])));
            }

            return res;
        }

        internal static IPRange? FindRange(IPRangesDatabase ranges, IPv4Addr query)
        {
            throw new NotImplementedException();
        }

        public static void Main(string[] args)
        {
            var ipLookupArgs = ParseArgs(args);
            if (ipLookupArgs == null)
            {
                return;
            }

            var queries = LoadQuery(ipLookupArgs.IpsFile);
            var ranges = LoadRanges(ipLookupArgs.IprsFiles);
            foreach (var ip in queries)
            {
                var findRange = FindRange(ranges, new IPv4Addr(ip));
                var result = TODO<string>();
                Console.WriteLine($"{ip}: {result}");
            }
        }

        private static T TODO<T>()
        {
            throw new NotImplementedException();
        }
    }
}