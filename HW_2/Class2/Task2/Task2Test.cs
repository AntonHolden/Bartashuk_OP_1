using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void FillWithAsterisksTest()
    {
        That(FillWithAsterisks("ab", 3), Is.EqualTo("a***b"));
        That(FillWithAsterisks("abcd", 2), Is.EqualTo("a**b**c**d"));
        That(FillWithAsterisks("*", 5), Is.EqualTo("*"));
    }

    [Test]
    public void TabulateSquaresTest()
    {
        That(TabulateSquares(3), Is.EqualTo(
@"1 1
2 4
3 9"));
        That(TabulateSquares(4), Is.EqualTo(
@"1  1
2  4
3  9
4 16"));
        That(TabulateSquares(10), Is.EqualTo(
@"1    1
2    4
3    9
4   16
5   25
6   36
7   49
8   64
9   81
10 100"));
        
    }
}