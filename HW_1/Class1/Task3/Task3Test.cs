using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task3.Task3;

namespace Task3;

public class Tests
{
    [Test]
    public void FTest()
    {
        That(F(0.0), Is.EqualTo(1.0).Within(1e-5));

        That(F(1.99999), Is.EqualTo(-1).Within(1e-5));
        That(F(-0.00001), Is.EqualTo(0).Within(1e-5));
        That(F(1), Is.EqualTo(-1).Within(1e-5));
        That(F(542.01), Is.EqualTo(1).Within(1e-5));
        That(F(-542.01), Is.EqualTo(0).Within(1e-5));
    }

    [Test]
    public void NumberOfDaysTest()
    {
        That(NumberOfDays(2021), Is.EqualTo(365));

        That(NumberOfDays(2024), Is.EqualTo(366));
        That(NumberOfDays(1800), Is.EqualTo(365));
        That(NumberOfDays(2200), Is.EqualTo(365));
        That(NumberOfDays(1900), Is.EqualTo(365));
        That(NumberOfDays(1600), Is.EqualTo(366));
        That(NumberOfDays(8), Is.EqualTo(366));
        That(NumberOfDays(1), Is.EqualTo(365));
        That(NumberOfDays(2), Is.EqualTo(365));
        That(NumberOfDays(2100), Is.EqualTo(365));
    }

    [Test]
    public void Rotate2Test()
    {
        That(Rotate2('С', 1, -1), Is.EqualTo('С'));

        That(Rotate2('Ю', 1, 2), Is.EqualTo('З'));
        That(Rotate2('В', 2, -1), Is.EqualTo('С'));
        That(Rotate2('З', -1, -1), Is.EqualTo('В'));
        That(Rotate2('Ю', 2, 2), Is.EqualTo('Ю'));
        That(Rotate2('В', 1, 1), Is.EqualTo('З'));
        That(Rotate2('С', 2, 1), Is.EqualTo('В'));
        That(Rotate2('З', -1, 2), Is.EqualTo('Ю'));
        That(Rotate2('Ю', -1, 1), Is.EqualTo('Ю'));
    }

    [Test]
    public void AgeDescriptionTest()
    {
        That(AgeDescription(42), Is.EqualTo("сорок два года"));

        That(AgeDescription(20), Is.EqualTo("двадцать лет"));
        That(AgeDescription(21), Is.EqualTo("двадцать один год"));
        That(AgeDescription(22), Is.EqualTo("двадцать два года"));
        That(AgeDescription(23), Is.EqualTo("двадцать три года"));
        That(AgeDescription(24), Is.EqualTo("двадцать четыре года"));
        That(AgeDescription(25), Is.EqualTo("двадцать пять лет"));
        That(AgeDescription(26), Is.EqualTo("двадцать шесть лет"));
        That(AgeDescription(27), Is.EqualTo("двадцать семь лет"));
        That(AgeDescription(28), Is.EqualTo("двадцать восемь лет"));
        That(AgeDescription(29), Is.EqualTo("двадцать девять лет"));
        That(AgeDescription(30), Is.EqualTo("тридцать лет"));
        That(AgeDescription(31), Is.EqualTo("тридцать один год"));
        That(AgeDescription(32), Is.EqualTo("тридцать два года"));
        That(AgeDescription(33), Is.EqualTo("тридцать три года"));
        That(AgeDescription(34), Is.EqualTo("тридцать четыре года"));
        That(AgeDescription(35), Is.EqualTo("тридцать пять лет"));
        That(AgeDescription(36), Is.EqualTo("тридцать шесть лет"));
        That(AgeDescription(37), Is.EqualTo("тридцать семь лет"));
        That(AgeDescription(38), Is.EqualTo("тридцать восемь лет"));
        That(AgeDescription(39), Is.EqualTo("тридцать девять лет"));
        That(AgeDescription(40), Is.EqualTo("сорок лет"));
        That(AgeDescription(41), Is.EqualTo("сорок один год"));
        That(AgeDescription(43), Is.EqualTo("сорок три года"));
        That(AgeDescription(44), Is.EqualTo("сорок четыре года"));
        That(AgeDescription(45), Is.EqualTo("сорок пять лет"));
        That(AgeDescription(46), Is.EqualTo("сорок шесть лет"));
        That(AgeDescription(47), Is.EqualTo("сорок семь лет"));
        That(AgeDescription(48), Is.EqualTo("сорок восемь лет"));
        That(AgeDescription(49), Is.EqualTo("сорок девять лет"));
        That(AgeDescription(50), Is.EqualTo("пятьдесят лет"));
        That(AgeDescription(51), Is.EqualTo("пятьдесят один год"));
        That(AgeDescription(52), Is.EqualTo("пятьдесят два года"));
        That(AgeDescription(53), Is.EqualTo("пятьдесят три года"));
        That(AgeDescription(54), Is.EqualTo("пятьдесят четыре года"));
        That(AgeDescription(55), Is.EqualTo("пятьдесят пять лет"));
        That(AgeDescription(56), Is.EqualTo("пятьдесят шесть лет"));
        That(AgeDescription(57), Is.EqualTo("пятьдесят семь лет"));
        That(AgeDescription(58), Is.EqualTo("пятьдесят восемь лет"));
        That(AgeDescription(59), Is.EqualTo("пятьдесят девять лет"));
        That(AgeDescription(60), Is.EqualTo("шестьдесят лет"));
        That(AgeDescription(61), Is.EqualTo("шестьдесят один год"));
        That(AgeDescription(62), Is.EqualTo("шестьдесят два года"));
        That(AgeDescription(63), Is.EqualTo("шестьдесят три года"));
        That(AgeDescription(64), Is.EqualTo("шестьдесят четыре года"));
        That(AgeDescription(65), Is.EqualTo("шестьдесят пять лет"));
        That(AgeDescription(66), Is.EqualTo("шестьдесят шесть лет"));
        That(AgeDescription(67), Is.EqualTo("шестьдесят семь лет"));
        That(AgeDescription(68), Is.EqualTo("шестьдесят восемь лет"));
        That(AgeDescription(69), Is.EqualTo("шестьдесят девять лет"));

    }

    [Test]
    public void MainTest()
    {
        Main(Array.Empty<string>());
    }
}