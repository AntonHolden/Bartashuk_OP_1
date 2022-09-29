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
    Six = 6,
    Seven,
    Eight,
    Nine,
    Ten,
    J,
    Q,
    K,
    A
}

// Карта
record Card
{
    public Player player { get; private set; }
    public void SetPlayer(Player player) => this.player = player;
    public Suit suit { get; init; }
    public Rank rank { get; private set; }
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
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    deck.Add(new Card(rank, suit));
                }
            }
            return deck;
        }

        // Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            var hands = new Dictionary<Player, Hand>();

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                hands[player] = new Hand();
            }

            var randomizer = new Random();
            int deckCurrentLen = 36;
            var currentPlayer = Player.P1;
            for (int i = 0; i < 36; i++)
            {
                int ind = randomizer.Next(deckCurrentLen--);
                deck[ind].SetPlayer(currentPlayer);
                hands[currentPlayer].Insert(0, pop(ref deck, ind));
                currentPlayer = 3 - currentPlayer;
            }
            return hands;
        }

        // Один раунд игры (в том числе спор при равных картах).
        // Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player?, Table> Round(ref Dictionary<Player, Hand> hands)
        {
            Table table = new Table();

            table.Add(hands[Player.P1][0]);
            table.Add(hands[Player.P2][0]);

            Console.WriteLine($"{Player.P1} pulled out the {hands[Player.P1][0].rank} of {hands[Player.P1][0].suit}!");
            Console.WriteLine($"{Player.P2} pulled out the {hands[Player.P2][0].rank} of {hands[Player.P2][0].suit}!");

            Player? winner = RoundWinner(hands[Player.P1][0], hands[Player.P2][0]);
            hands[Player.P1].RemoveAt(0);
            hands[Player.P2].RemoveAt(0);
            while (winner == null)
            {
                if (!hands[Player.P1].Any() && !hands[Player.P2].Any())
                {
                    Console.WriteLine("The players have run out of cards!");
                    return new Tuple<Player?, Table>(null, table);
                }
                else if (!hands[Player.P1].Any()) return new Tuple<Player?, Table>(Player.P2, table);
                else if (!hands[Player.P2].Any()) return new Tuple<Player?, Table>(Player.P1, table);

                Console.WriteLine("\nA draw!\n");

                Console.WriteLine($"Player {Player.P1} has {hands[Player.P1].Count()} cards.");
                Console.WriteLine($"Player {Player.P2} has {hands[Player.P2].Count()} cards.\n");

                Console.WriteLine("Press Enter to make a move.\n");
                Console.ReadLine();

                table.Add(hands[Player.P1][0]);
                table.Add(hands[Player.P2][0]);

                Console.WriteLine($"{Player.P1} pulled out the {hands[Player.P1][0].rank} of {hands[Player.P1][0].suit}!");
                Console.WriteLine($"{Player.P2} pulled out the {hands[Player.P2][0].rank} of {hands[Player.P2][0].suit}!");

                winner = RoundWinner(hands[Player.P1][0], hands[Player.P2][0]);
                hands[Player.P1].RemoveAt(0);
                hands[Player.P2].RemoveAt(0);
            }
            Console.WriteLine($"--------------------\nWinner of the round is: {winner}!\n--------------------\n");
            return new Tuple<Player?, Table>(winner, table);
        }

        // Полный цикл игры (возвращается победивший игрок)
        // в процессе игры печатаются ходы
        internal static Player? Game(Dictionary<Player, Hand> hands)
        {
            //Console.WriteLine("\n############################");
            //Console.WriteLine("P1 cards:");
            //foreach (var x in hands[Player.P1])
            //{
            //    Console.Write($"({x.rank},{x.suit}) ");
            //}
            //Console.WriteLine("\n");
            //Console.WriteLine("P2 cards:");
            //foreach (var x in hands[Player.P2])
            //{
            //    Console.Write($"({x.rank},{x.suit}) ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("############################\n");

            Console.WriteLine("The game has started!");

            Console.WriteLine($"\nPlayer {Player.P1} has {hands[Player.P1].Count()} cards.");
            Console.WriteLine($"Player {Player.P2} has {hands[Player.P2].Count()} cards.\n");

            Console.WriteLine("Press Enter to start a new round.\n");
            Console.ReadLine();

            while (true)
            {
                var winnerTable = Round(ref hands);

                if (winnerTable.Item1==null)
                {
                    return winnerTable.Item1;
                }

                //randomly taking the cards on the table

                var table = winnerTable.Item2;
                var randomizer = new Random();
                while (table.Count()!=0)
                {
                    int ind = randomizer.Next(table.Count());
                    hands[(Player)winnerTable.Item1!].Add(pop(ref table, ind));
                }

                // NOT randomly taking...
                //hands[(Player)winnerTable.Item1!].AddRange(winnerTable.Item2);

                Console.WriteLine($"Player {Player.P1} has {hands[Player.P1].Count()} cards.");
                Console.WriteLine($"Player {Player.P2} has {hands[Player.P2].Count()} cards.\n");

                Console.WriteLine("Press Enter to start a new round.\n");
                Console.ReadLine();

                //Console.WriteLine("\n############################");
                //Console.WriteLine("P1 cards:");
                //foreach (var x in hands[Player.P1])
                //{
                //    Console.Write($"({x.rank},{x.suit}) ");
                //}
                //Console.WriteLine("\n");
                //Console.WriteLine("P2 cards:");
                //foreach (var x in hands[Player.P2])
                //{
                //    Console.Write($"({x.rank},{x.suit}) ");
                //}
                //Console.WriteLine();
                //Console.WriteLine("############################\n");

                if (!hands[Player.P1].Any() || !hands[Player.P2].Any())
                {
                    return winnerTable.Item1;
                }
                
            }
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            if (winner == null) Console.WriteLine("\n****************************\n****************************\nThe game ended in a draw!");
            else Console.WriteLine($"\n****************************\n****************************\nThe winner of the game is: {winner}!");
        }
    }
}