using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        string s = "abcdab", s1 = "ab", s2 = "xxx";
        string[] splitter = s.Split(s1);
        try
        {
            string rez = String.Concat(splitter[0], s2, splitter[1]);
        }
        catch
        {
            Console.WriteLine(splitter[0]);
        }
    }
}