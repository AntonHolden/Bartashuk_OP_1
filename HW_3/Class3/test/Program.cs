internal enum Suit
{
    Diamonds,
    Hearts,
    Clubs,
    Spades
}

// Значение
internal enum Rank
{
    _6 = 2,
    _7,
    _8,
    _9,
    _10,
    J,
    Q,
    K,
    A
}
internal class Program
{
    private static void Main(string[] args)
    {
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            Console.WriteLine(rank);
        }
    }
}