using System.Text;
using System.Text.Unicode;

namespace Task2
{
    public class Task2
    {

        public static void Main(string[] args)
        {
            try
            {
                if (File.Exists(args[0]))
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    File.WriteAllText(args[0], File.ReadAllText(args[0], Encoding.GetEncoding(args[1])), Encoding.GetEncoding(args[2]));
                }
            }
            catch
            {
                return;
            }
        }
    }
}
