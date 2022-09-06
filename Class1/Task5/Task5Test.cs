using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task5.Task5;

namespace Task5;

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
    public void Test1()
    {
        ComputeFib(new[] { "1" });
        AssertOut("1");
    }

    [Test]
    public void Test2()
    {
        Console.SetIn(new StringReader("10"));
        ComputeFib(Array.Empty<string>());
        AssertOut("55");
    }

    [Test]
    public void Test3()
    {
        ComputeFib(new[] { "150" });
        AssertOut("9969216677189303386214405760200");
    }

    [Test]
    public void Test4()
    {
        Console.SetIn(new StringReader("42"));
        ComputeFib(Array.Empty<string>());
        AssertOut("267914296");
    }

    private void AssertOut(String expected)
    {
        That(_stringWriter.ToString().TrimEnd(Environment.NewLine.ToCharArray()), Is.EqualTo(expected));
    }
}