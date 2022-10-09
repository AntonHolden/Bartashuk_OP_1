public class Program
{

    public static void Main(string[] args)
    {
        var a = new List<int> { 1, 2, 5, 7 };
        int ind = a.BinarySearch(8);
        Console.WriteLine(ind);
        Console.WriteLine(~ind);
    }
}