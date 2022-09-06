using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void Min3Test1()
    {
        That(Min3(2, 0, 3), Is.EqualTo(0));
    }

    [Test]
    public void Min3Test2()
    {
        That(Min3(-100, -99, 0), Is.EqualTo(-100));
    }

    [Test]
    public void Min3Test3()
    {
        That(Min3(1, 1, 2), Is.EqualTo(1));
    }

    [Test]
    public void Max3Test1()
    {
        That(Max3(7, 7, 7), Is.EqualTo(7));
    }

    [Test]
    public void Max3Test2()
    {
        That(Max3(0, 1, -1), Is.EqualTo(1));
    }

    [Test]
    public void Max3Test3()
    {
        That(Max3(-100, -9, -2), Is.EqualTo(-2));
    }

    [Test]
    public void Deg2RadTest1()
    {
        That(Deg2Rad(180.0), Is.EqualTo(Math.PI).Within(1e-5));
        That(Deg2Rad(2 * 360 + 180.0), Is.EqualTo(5 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest1()
    {
        That(Rad2Deg(Math.PI), Is.EqualTo(180.0).Within(1e-5));
        That(Rad2Deg(5 * Math.PI), Is.EqualTo(5 * 180.0).Within(1e-5));
    }

    [Test]
    public void MoreRadDegTests()
    {
        That(Deg2Rad(1), Is.EqualTo(0.017453292519943295).Within(1e-5));
        That(Deg2Rad(90), Is.EqualTo(Math.PI/2).Within(1e-5));
        That(Deg2Rad(114.59156), Is.EqualTo(2).Within(1e-5));
        That(Deg2Rad(572.95779), Is.EqualTo(10).Within(1e-5));
        That(Deg2Rad(0), Is.EqualTo(0).Within(1e-5));

        That(Rad2Deg(0.5), Is.EqualTo(90/Math.PI).Within(1e-5));
        That(Rad2Deg(1), Is.EqualTo(180/Math.PI).Within(1e-5));
        That(Rad2Deg(0), Is.EqualTo(0).Within(1e-5));
        That(Rad2Deg(1432.00774468), Is.EqualTo(82048).Within(1e-5));
        That(Rad2Deg(6.283185308), Is.EqualTo(360).Within(1e-5));
    }
}