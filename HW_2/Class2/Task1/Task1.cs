using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Task1
{
    public class Task1
    {
/*
 * Задание 1.1. Дана строка. Верните строку, содержащую текст "Длина: NN",
 * где NN — длина заданной строки. Например, если задана строка "hello",
 * то результатом должна быть строка "Длина: 5".
 */
        internal static int StringLength(string s)
        {
            return s.Length;
        }

/*
 * Задание 1.2. Дана непустая строка. Вернуть коды ее первого и последнего символов.
 * Рекомендуется найти специальные функции для вычисления соответствующих символов и их кодов.
 */
        internal static Tuple<int?, int?> FirstLastCodes(string s)
        {
            return new Tuple<int?, int?>(Code(First(s)), Code(Last(s)));
        }
        
        private static char? First(string s) => s[0];
        private static char? Last(string s) => s[s.Length-1];

        //private static int? Code(char? c) => (int)c;
        //private static int? Code(char? c) => Encoding.ASCII.GetBytes(c.ToString())[0];
        //private static int? Code(char? c) => Char.ConvertToUtf32(c.ToString(), 0);
        private static int? Code(char? c) => Convert.ToInt32(c);

        /*
         * Задание 1.3. Дана строка. Подсчитать количество содержащихся в ней цифр.
         * В решении необходимо воспользоваться циклом for.
         */
        internal static int CountDigits(string s)
        {
            int rez = 0;
            for (int i=0;i<s.Length;i++)
            {
                if (int.TryParse(s[i].ToString(),out _)) rez++;
            }
            return rez;
        }

/*
 * Задание 1.4. Дана строка. Подсчитать количество содержащихся в ней цифр.
 * В решении необходимо воспользоваться методом Count:
 * https://docs.microsoft.com/ru-ru/dotnet/api/system.linq.enumerable.count?view=net-6.0#system-linq-enumerable-count-1(system-collections-generic-ienumerable((-0))-system-func((-0-system-boolean)))
 * и функцией Char.IsDigit:
 * https://docs.microsoft.com/ru-ru/dotnet/api/system.char.isdigit?view=net-6.0
 */
        internal static int CountDigits2(string s)
        {
            return s.Count(symbol=>char.IsDigit(symbol));
        }
        
/*
 * Задание 1.5. Дана строка, изображающая арифметическое выражение вида «<цифра>±<цифра>±…±<цифра>»,
 * где на месте знака операции «±» находится символ «+» или «−» (например, «4+7−2−8»). Вывести значение
 * данного выражения (целое число).
 */
        internal static int CalcDigits(string expr) {
            int rez = 0;
            string[] nums_sum = expr.Split('+');
            for (int i = 0; i < nums_sum.Length; i++)
            {
                string[] nums_substract = nums_sum[i].Split('-');
                rez += int.Parse(nums_substract[0]);
                for (int j=1;j<nums_substract.Length;j++)
                {
                    rez -= int.Parse(nums_substract[j]);
                }
            }
            return rez;
        }

/*
 * Задание 1.6. Даны строки S, S1 и S2. Заменить в строке S первое вхождение строки S1 на строку S2.
 */
        internal static string ReplaceWithString(string s, string s1, string s2) {
            string[] splitter = s.Split(s1);
            string rez = splitter[0];
            if (splitter.Length > 1)
            {
                rez = String.Concat(splitter[0], s2, splitter[1]);
                for (int i = 2; i < splitter.Length; i++)
                {
                    rez = String.Concat(rez, s1, splitter[i]);
                }
            }
            return rez;
        }
        

        public static void Main(string[] args)
        {
            StringLength("Hello!");
            FirstLastCodes("My name is Robert");
            CountDigits("I'm 21 years old.");
            CountDigits2("I've been living in New York for 4 years.");
            CalcDigits("1+2-3+15-0+124-22-2+1+40");
            ReplaceWithString("My favourite food is pasta","pasta","pizza");
        }
    }
}