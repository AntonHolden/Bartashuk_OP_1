using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void AvailableFunctionsAmountTest()
    {
        That(AvailableFunctions, Has.Count.GreaterThanOrEqualTo(10));
    }

    [Test]
    public void TabulateTest()
    {
        var funNames = new List<string> { "sqr", "sin" };
        var nOfPoints = 10;
        double fromX = 0.0;
        double toX = 10.0;
        var res = tabulate(new InputData(fromX, toX, nOfPoints, funNames));
        var lines = res.ToString().Split('\n');
        That(lines, Has.Length.EqualTo(2 * ((int)Math.Ceiling(toX) - (int)Math.Floor(fromX) + 2) + 1));
        foreach (var line in lines)
        {
            var splitter = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (splitter.Length > 1)
            {
                That(splitter, Has.Length.EqualTo(funNames.Count + 1));
                foreach (var strNum in splitter)
                {
                    double num;
                    if ((double.TryParse(strNum, out num)) && (Math.Round(num) != num))
                    {
                        That(Math.Abs(num - Math.Round(num, MidpointRounding.ToZero)).ToString(), Has.Length.EqualTo(nOfPoints + 2));
                    }
                }
            }
        }
    }
}