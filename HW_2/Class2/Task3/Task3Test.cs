using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task3.Task3;

namespace Task3;

public class Tests
{
    [Test]
    public void AllDigitsTest()
    {
        That(AllDigits("0123"), Is.True);
        That(AllDigits("9876543210"), Is.True);
        That(AllDigits("0123x"), Is.False);
        That(AllDigits("xxx"), Is.False);
    }

    [Test]
    public void ContainsABCTest()
    {
        That(ContainsABC("01aBC"), Is.True);
        That(ContainsABC("ABC"), Is.True);
        That(ContainsABC("xabcx"), Is.True);
        That(ContainsABC("abD"), Is.False);
        That(ContainsABC("bcbcbca"), Is.False);
    }

    [Test]
    public void FindDigitalSubstringTest()
    {
        That(FindDigitalSubstring("000"), Is.EqualTo("000"));
        That(FindDigitalSubstring("abc123def456"), Is.EqualTo("123"));
        That(FindDigitalSubstring("1234def456"), Is.EqualTo("1234"));
        That(FindDigitalSubstring("abcde"), Is.EqualTo(""));
        That(FindDigitalSubstring(""), Is.EqualTo(""));
    }

    [Test]
    public void HideDigitsTest()
    {
        That(HideDigits("abc00def", "xxx"), Is.EqualTo("abcxxxdef"));
        That(HideDigits("abc00def09876543ghe", "xxx"), Is.EqualTo("abcxxxdefxxxghe"));
        That(HideDigits("abcdef", "xxx"), Is.EqualTo("abcdef"));
    }
}