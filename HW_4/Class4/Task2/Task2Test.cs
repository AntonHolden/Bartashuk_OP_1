using System;
using System.Text;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void Main1Test()
    {
        string dirPath = Directory.GetCurrentDirectory();
        dirPath = dirPath.Substring(0, dirPath.Length - 18) + "1" + @"\";

        var tmpFileName = Path.GetTempFileName();
        try
        {
            File.Copy(dirPath + "data/text-utf8.txt", tmpFileName, true);
            Main(new[] { tmpFileName, "utf-8", "windows-1251" });
            That(File.ReadAllBytes(tmpFileName), Is.EqualTo(File.ReadAllBytes(dirPath + "data/text-windows-1251.txt")));
        }
        finally
        {
            File.Delete(tmpFileName);
        }
    }

    [Test]
    public void Main2Test()
    {
        string dirPath = Directory.GetCurrentDirectory();
        dirPath = dirPath.Substring(0, dirPath.Length - 18) + "1" + @"\";

        var tmpFileName = Path.GetTempFileName();
        try
        {
            File.Copy(dirPath + "data/text-windows-1251.txt", tmpFileName, true);
            Main(new[] { tmpFileName, "windows-1251", "utf-8" });

            var tmpFileNameBytes = File.ReadAllBytes(tmpFileName);
            Array.Copy(tmpFileNameBytes, 3, tmpFileNameBytes, 0, tmpFileNameBytes.Length - 3);
            Array.Resize(ref tmpFileNameBytes, tmpFileNameBytes.Length - 3);

            That(tmpFileNameBytes, Is.EqualTo(File.ReadAllBytes(dirPath + "data/text-utf8.txt")));
        }
        finally
        {
            File.Delete(tmpFileName);
        }
    }

}
