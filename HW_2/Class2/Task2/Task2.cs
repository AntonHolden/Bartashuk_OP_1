using System.Text;

namespace Task2
{
    public class Task2
    {

        /*
         * В этих заданиях * рекомендуется всюду использовать класс StringBuilder.
         * Документация: https://docs.microsoft.com/ru-ru/dotnet/api/system.text.stringbuilder?view=net-6.0
         */

        /*
         * Задание 2.1. Дана непустая строка S и целое число N (> 0). Вернуть строку, содержащую символы
         * строки S, между которыми вставлено по N символов «*» (звездочка).
         */
        internal static string FillWithAsterisks(string s, int n)
        {
            StringBuilder res = new StringBuilder(s[0].ToString());
            for (int i = 1; i < s.Length; i++)
            {
                res.Append(String.Concat(Enumerable.Repeat('*', n))).Append(s[i]);
            }
            return res.ToString();
        }

        /*
         * Задание 2.2. Сформировать таблицу квадратов чисел от 1 до заданного числа N.
         * Например, для N=4 должна получиться следующая строка:

        1  1
        2  4
        3  9
        4 16

         * Обратите внимание на выравнивание: числа в первом столбце выравниваются по левому краю,
         * а числа во втором -- по правому, причём между числами должен оставаться как минимум один
         * пробел. В решении можно использовать функции Pad*.
         */
        internal static string TabulateSquares(int n)
        {
            int gap = n.ToString().Length + (n * n).ToString().Length + 1;
            StringBuilder table = new StringBuilder(1.ToString()).Append(1.ToString().PadLeft(gap - 1));
            for (int currentN = 2; currentN <= n; currentN++)
            {
                var strN = currentN.ToString();
                table.Append('\n').Append(strN).Append((currentN * currentN).ToString().PadLeft(gap - strN.Length));
            }
            return table.ToString();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(FillWithAsterisks("abc", 2));
            Console.WriteLine(TabulateSquares(4));
        }
    }
}