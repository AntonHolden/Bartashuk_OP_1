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

            public static IPv4Addr Min(IPv4Addr query1, IPv4Addr query2)
            {
                return (query2 < query1) ? query2 : query1;
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

        internal static Tuple<IPv4Addr, IPv4Addr>[]? LoadRanges(List<String> filenames)
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

            if (res.Count==0) return null;

            var tree = new Tuple<IPv4Addr, IPv4Addr>[4 * res.Count];
            build(res, 1, 0, res.Count - 1, ref tree);

            return tree;
        }

        public static void build(IPRangesDatabase a, int v, int l, int r, ref Tuple<IPv4Addr, IPv4Addr>[] tree)
        {
            if (l == r) tree[v] = a[l];
            else
            {
                int m = (r + l) / 2;
                build(a, v * 2, l, m, ref tree);
                build(a, v * 2 + 1, m + 1, r, ref tree);

                tree[v] = new Tuple<IPv4Addr, IPv4Addr>(IPv4Addr.Min(tree[v * 2].Item1, tree[v * 2 + 1].Item1), IPv4Addr.Max(tree[v * 2].Item2, tree[v * 2 + 1].Item2));
            }
        }

        public static Tuple<IPv4Addr, IPv4Addr>? FindInTree(int v, int l, int r, IPv4Addr x, Tuple<IPv4Addr, IPv4Addr>[] tree)
        {
            if ((l == r) && ((x >= tree[v].Item1) && (x <= tree[v].Item2))) return tree[v];
            if (!((x >= tree[v].Item1) && (x <= tree[v].Item2))) return null;

            int m = (r + l) / 2;

            Tuple<IPv4Addr, IPv4Addr>? findLeft = FindInTree(v * 2, l, m, x, tree);

            if (findLeft == null)
            {
                Tuple<IPv4Addr, IPv4Addr>? findRight = FindInTree(v * 2 + 1, m + 1, r, x, tree);

                if (findRight == null) return null;
                return findRight;
            }
            return findLeft;
        }

        internal static IPRange? FindRange(Tuple<IPv4Addr, IPv4Addr>[] tree, IPv4Addr query)
        {
            if (tree == null) return null;

            var range = FindInTree(1, 0, (tree.Length/4) - 1, query, tree);
            if (range == null) return null;

            return new IPRange(range.Item1, range.Item2);
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