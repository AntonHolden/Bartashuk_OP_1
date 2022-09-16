namespace Task4
{
    public class Task4
    {
/*
 * В решениях следующих заданий предполагается использование циклов.
 */

/*
 * Задание 4.1. Пользуясь циклом for, посимвольно напечатайте рамку размера width x height,
 * состоящую из символов frameChar. Можно считать, что width>=2, height>=2.
 * Например, вызов printFrame(5,3,'+') должен напечатать следующее:

+++++
+   +
+++++
 */
        internal static void PrintFrame(int width, int height, char frameChar = '*')
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat(frameChar, width)));
            for (int i=0;i<height-2;i++)
            {
                Console.WriteLine((frameChar + String.Concat(Enumerable.Repeat(" ",width-2))+frameChar));
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat(frameChar, width)));
        }

/*
 * Задание 4.2. Выполните предыдущее задание, пользуясь циклом while.
 */
        internal static void PrintFrame2(int width, int height, char frameChar = '*')
        {
            int ind = 0;
            Console.WriteLine(String.Concat(Enumerable.Repeat(frameChar, width)));
            while (ind < height - 2)
            {
                Console.WriteLine((frameChar + String.Concat(Enumerable.Repeat(" ", width - 2)) + frameChar));
                ind++;
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat(frameChar, width)));
        }


/*
 * Задание 4.3. Даны целые положительные числа A и B. Найти их наибольший общий делитель (НОД),
 * используя алгоритм Евклида:
 * НОД(A, B) = НОД(B, A mod B),    если B ≠ 0;        НОД(A, 0) = A,
 * где «mod» обозначает операцию взятия остатка от деления.
 */
        internal static long Gcd(long a, long b)
        {
            long temp;
            while (b!=0)
            {
                temp = a;
                a = b;
                b = temp%b;
            }
            return a;
        }

/*
 * Задание 4.4. Дано вещественное число X и целое число N (> 0). Найти значение выражения
 * 1 + X + X^2/(2!) + … + X^N/(N!), где N! = 1·2·…·N.
 * Полученное число является приближенным значением функции exp в точке X.
 */
        internal static double ExpTaylor(double x, int n)
        {
            double rezult = 1;
            double denom = 1;
            for (int current_n=1;current_n<=n;current_n++)
            {
                denom *= current_n;
                rezult += (Math.Pow(x, current_n)) / denom;
            }
            return rezult;
        }

        public static void Main(string[] args)
        {
            PrintFrame(5, 3, '+');
            PrintFrame2(2, 2, '-');
            Gcd(50, 40);
            ExpTaylor(2.5,5);
        }
    }
}