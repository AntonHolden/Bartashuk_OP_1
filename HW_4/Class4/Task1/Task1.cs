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

        public record IPv4Addr : IComparable<IPv4Addr>
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
            public static bool operator <(IPv4Addr query1, IPv4Addr query2)
            {
                return (query1.IntValue < query2.IntValue);
            }
            public static bool operator >(IPv4Addr query1, IPv4Addr query2)
            {
                return (query1.IntValue > query2.IntValue);
            }

            public static bool operator <=(IPv4Addr query1, IPv4Addr query2)
            {
                return (query1.IntValue <= query2.IntValue);
            }
            public static bool operator >=(IPv4Addr query1, IPv4Addr query2)
            {
                return (query1.IntValue >= query2.IntValue);
            }

            public static IPv4Addr Max(IPv4Addr query1, IPv4Addr query2)
            {
                return (query2 > query1) ? query2 : query1;
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

                res.Add(new Tuple<IPv4Addr, IPv4Addr>(new IPv4Addr(splitter[0]), new IPv4Addr(splitter[1])));
            }
            return res;
        }

        public static IPRangesDatabase Merge(IPRangesDatabase intervals)
        {
            intervals.Sort(new IntervalComparer());
            var stack = new Stack<Tuple<IPv4Addr, IPv4Addr>>();

            var curr = intervals[0];

            for (int i = 1; i < intervals.Count; i++)
            {
                var next = intervals[i];
                if (curr.Item2 >= next.Item1)
                    curr = new Tuple<IPv4Addr, IPv4Addr>(curr.Item1, IPv4Addr.Max(curr.Item2, next.Item2));
                else
                {
                    stack.Push(curr);
                    curr = next;
                }
            }
            stack.Push(curr);

            var res = new IPRangesDatabase();
            for (int i = 0; i < stack.Count; i++) res.Insert(0, stack.Pop());

            return res;
        }

        public class IntervalComparer : IComparer<Tuple<IPv4Addr, IPv4Addr>>
        {
            public int Compare(Tuple<IPv4Addr, IPv4Addr> x, Tuple<IPv4Addr, IPv4Addr> y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }

        public static void build(IPRangesDatabase a, int v, int l, int r, ref IPRangesDatabase[] tree)
        {
            if (l == r) tree[v] = new IPRangesDatabase { a[l] };
            else
            {
                int m = (r + l) / 2;
                build(a, v * 2, l, m, ref tree);
                build(a, v * 2 + 1, m + 1, r, ref tree);

                var lowerNodes = tree[v * 2];
                lowerNodes.AddRange(tree[v * 2 + 1]);

                tree[v] = Merge(lowerNodes);
            }
        }

        public static int BinarySearchRight(IPRangesDatabase ranges, IPv4Addr query)
        {
            int l = -1;
            int r = ranges.Count;
            while ((r - l) > 1)
            {
                int m = (r + l) / 2;
                if (ranges[m].Item1 > query) r = m;
                else l = m;
            }
            return r;
        }

        public static void find(int v, int l, int r, IPv4Addr x, ref IPRangesDatabase[] tree)
        {
            int ind = BinarySearchRight(tree[v], x) - 1;
            if (ind < 0) return null;
        }

        internal static IPRange? FindRange(IPRangesDatabase ranges, IPv4Addr query)
        {

        }

        public static void Main(string[] args)
        {
            var ipLookupArgs = ParseArgs(new[] { "data/query.ips", "data/1.iprs", "data/2.iprs" });
            //var ipLookupArgs = ParseArgs(args);
            //if (ipLookupArgs == null)
            //{
            //    return;
            //}

            var queries = LoadQuery(ipLookupArgs.IpsFile);
            var ranges = LoadRanges(ipLookupArgs.IprsFiles);

            var tree = new IPRangesDatabase[4 * ranges.Count];
            build(ranges, 1, 0, ranges.Count - 1, ref tree);

            foreach (var ip in queries)
            {
                var findRange = FindRange(ranges, new IPv4Addr(ip));
                if (findRange == null) Console.WriteLine("NO");
                else
                {
                    var result = findRange.ToString();
                    Console.WriteLine($"{ip}: {result}");
                }
            }
        }

    }
}