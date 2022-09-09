using System;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        int n = 4;
        int gap = n.ToString().Length + (n * n).ToString().Length + 1;
        StringBuilder table = new StringBuilder();
        for (int current_n = 1; current_n <= n; current_n++)
        {
            var str_n = current_n.ToString();
            table.Append(str_n).Append((current_n * current_n).ToString().PadLeft(gap - str_n.Length)).Append('\n');
        }
        Console.WriteLine(table.ToString());
    }
}