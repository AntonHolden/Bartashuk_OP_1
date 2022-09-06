using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    private readonly TextReader _standartIn = Console.In;
    private readonly TextWriter _standartOut = Console.Out;
    private StringWriter _stringWriter = new StringWriter();

    [SetUp]
    public void Setup()
    {
        var stringWriter = new StringWriter();
        _stringWriter = stringWriter;
        Console.SetOut(_stringWriter);
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetIn(_standartIn);
        Console.SetOut(_standartOut);
        _stringWriter.Close();
    }

    [Test]
    public void TestSayHello()
    {
        SayHello("Vitaly");
        AssertOut("Hello Vitaly!");
    }

    private void AssertOut(String expected)
    {
        That(_stringWriter.ToString().TrimEnd(Environment.NewLine.ToCharArray()), Is.EqualTo(expected));
    }

    [Test]
    public void TestSayHelloNl()
    {
        SayHello("Vitaly\nBragilevsky");
        AssertOut("Hello Vitaly\nBragilevsky!");
    }

    [Test]
    public void TestHelloUser()
    {
        Console.SetIn(new StringReader("C#"));
        HelloUser();
        AssertOut("Hello C#!");
    }

    [Test]
    public void TestSquare()
    {
        runOnInputCheckLastLine("5", "25", Square);
    }

    [Test]
    public void TestSum()
    {
        runOnInputCheckLastLine("2\n3", "5", Sum);
        runOnInputCheckLastLine("100\n-100", "0", Sum);
        runOnInputCheckLastLine("50\n50", "100", Sum);
    }

    private void runOnInputCheckLastLine(string input, string expected, Action action) {
        Console.SetIn(new StringReader(input));
        action.Invoke();
        var newLineCharArray = Environment.NewLine.ToCharArray();
        That(
            _stringWriter.ToString().Split(newLineCharArray, StringSplitOptions.RemoveEmptyEntries) .Last(), 
            Is.EqualTo(expected));
    }
}