using NUnit.Framework;
using System.Numerics;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        var TestCard1 = new Card(Rank.A, Suit.Hearts);
        TestCard1.SetPlayer(Player.P1);

        var TestCard2 = new Card(Rank.A, Suit.Spades);
        TestCard2.SetPlayer(Player.P2);

        That(RoundWinner(TestCard1, TestCard2), Is.EqualTo(null));


        var TestCard3 = new Card(Rank.K, Suit.Hearts);
        TestCard3.SetPlayer(Player.P1);

        var TestCard4 = new Card(Rank.Six, Suit.Diamonds);
        TestCard4.SetPlayer(Player.P2);

        That(RoundWinner(TestCard3, TestCard4), Is.EqualTo(Player.P1));


        var TestCard5 = new Card(Rank.Six, Suit.Clubs);
        TestCard5.SetPlayer(Player.P1);

        var TestCard6 = new Card(Rank.Seven, Suit.Hearts);
        TestCard6.SetPlayer(Player.P2);

        That(RoundWinner(TestCard5, TestCard6), Is.EqualTo(Player.P2));
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        That(deck, Has.Count.EqualTo(DeckSize));
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                That(deck.Count(card => (card.rank == rank) && (card.suit == suit)), Is.EqualTo(1));
            }
        }
    }

    [Test]
    public void RoundTest()
    {
        var P1Card1 = new Card(Rank.Six, Suit.Clubs);
        P1Card1.SetPlayer(Player.P1);

        var P1Card2 = new Card(Rank.K, Suit.Diamonds);
        P1Card2.SetPlayer(Player.P1);

        var P2Card1 = new Card(Rank.Six, Suit.Spades);
        P2Card1.SetPlayer(Player.P2);

        var P2Card2 = new Card(Rank.A, Suit.Hearts);
        P2Card2.SetPlayer(Player.P2);


        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.P1, new List<Card> {P1Card1,P1Card2 } },
            { Player.P2, new List<Card> {P2Card1,P2Card2 } }
        };
        That(Round(ref hands), Is.EqualTo(new Tuple<Player?, List<Card>>(Player.P2, new List<Card>() { P1Card1, P2Card1, P1Card2, P2Card2 })));
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = new Card(Rank.Six, Suit.Hearts);
        six.SetPlayer(Player.P1);
        var ace = new Card(Rank.A, Suit.Clubs);
        ace.SetPlayer(Player.P2);
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.P1, new List<Card> {six} },
            { Player.P2, new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.P2));
    }

}