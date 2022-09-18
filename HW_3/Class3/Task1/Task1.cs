// Колода
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;

// Масть
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

// Карта
record Card
{
    public Player player;
    private Suit suit;
    public Rank rank { get; private set; }
    public Card(Player player) => this.player = player;
    public Card(Rank rank, Suit suit)
    {
        this.suit = suit;
        this.rank = rank;
    }

};

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    P1 = 1,
    P2
}

namespace Task1
{
    public class Task1
    {
        public static T pop<T>(ref List<T> list, int ind = 0)
        {
            T element = list[ind];
            list.RemoveAt(ind);
            return element;
        }
        /*
        * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
        * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
        * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
        * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
        * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
        *
        * Реализация должна сопровождаться тестами.
        */

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            return (card1.rank == card2.rank) ? null : (card1.rank > card2.rank) ? card1.player : card2.player;
        }

        // Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            Deck deck = new Deck();
            int i = 0;
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    deck.Add(new Card(rank, suit));
                    i++;
                }
            }
            return deck;
        }

        // Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            var hands = new Dictionary<Player, Hand>();
            var randomizer = new Random();
            int deckCurrentLen = 36;
            var currentPlayer = Player.P1;
            for (int i = 0; i < 36; i++)
            {
                int ind = randomizer.Next(deckCurrentLen--);
                deck[ind].player = currentPlayer;
                hands[currentPlayer = 3 - currentPlayer].Insert(0,pop(ref deck,ind));
            }
            return hands;
        }

        // Один раунд игры (в том числе спор при равных картах).
        // Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player?, Table> Round(ref Dictionary<Player, Hand> hands)
        {
            
            Player? winner = null;
            Table table = new Table();
            while (winner==null)
            {
                if (!hands[Player.P1].Any() && !hands[Player.P1].Any()) return new Tuple<Player?, Table>(null, table);
                else if (!hands[Player.P1].Any()) return new Tuple<Player?, Table>(Player.P2, table);
                else if (!hands[Player.P2].Any()) return new Tuple<Player?, Table>(Player.P1, table);
                table.Add(hands[Player.P1][0]);
                table.Add(hands[Player.P2][0]);
                winner = RoundWinner(hands[Player.P1][0], hands[Player.P2][0]);
                hands[Player.P1].RemoveAt(0);
                hands[Player.P2].RemoveAt(0);
            }
            return new Tuple<Player?, Table>(winner, table);
        }

        // Полный цикл игры (возвращается победивший игрок)
        // в процессе игры печатаются ходы
        internal static Player? Game(Dictionary<Player, Hand> hands)
        {
            var winnerTable = Round(ref hands);
            while (hands[Player.P1].Any() && hands[Player.P1].Any())
            {
                if (winnerTable.Item1 == null) return null;
                hands[(Player)winnerTable.Item1].AddRange(winnerTable.Item2);
                winnerTable = Round(ref hands);
            }
            return winnerTable.Item1;
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            if (winner == null) Console.WriteLine("Ничья!");
            else Console.WriteLine($"Победитель: {winner}");
        }
    }
}