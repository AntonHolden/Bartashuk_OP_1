using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    [Test]
    public void StringLengthTest()
    {
        That(StringLength("hello"), Is.EqualTo(5));
        That(StringLength(""), Is.EqualTo(0));
        That(StringLength(new String('x', 1000)), Is.EqualTo(1000));
    }

    [Test]
    public void TestCountDigits()
    {
        That(CountDigits("xxx"), Is.EqualTo(0));
        That(CountDigits("x1x2x3"), Is.EqualTo(3));
        That(CountDigits("1a2b3c"), Is.EqualTo(3));
    }

    [Test]
    public void FirstLastCodeTest()
    {
        That(FirstLastCodes(" "), Is.EqualTo(new Tuple<int, int>(32, 32)));
        That(FirstLastCodes("xxxxxxxxyyyyyyyy"), Is.EqualTo(new Tuple<int, int>('x', 'y')));
    }

    [Test]
    public void CountDigits2Test()
    {
        That(CountDigits2("xxx"), Is.EqualTo(0));
        That(CountDigits2("x1x2x3"), Is.EqualTo(3));
        That(CountDigits2("1a2b3c"), Is.EqualTo(3));
    }
    
    [Test]
    public void CalcDigitsTest()
    {
        That(CalcDigits("3-3"), Is.EqualTo(0));
        That(CalcDigits("4+7-2-8"), Is.EqualTo(1));
        That(CalcDigits("2+8-4-7"), Is.EqualTo(-1));
    }

    [Test]
    public void ReplaceWithStringTest()
    {
        /*assertEquals("Миру война", replaceWithString("Миру мир", "мир", "война"))
        assertEquals("xxxcd", replaceWithString("abcd", "ab", "xxx"))
        assertEquals("xxxcdab", replaceWithString("abcdab", "ab", "xxx"))*/
        That(ReplaceWithString("Миру мир", "мир", "война"), Is.EqualTo("Миру война"));
        That(ReplaceWithString("abcd", "ab", "xxx"), Is.EqualTo("xxxcd"));
        That(ReplaceWithString("abcdab", "ab", "xxx"), Is.EqualTo("xxxcdab"));
    }
}