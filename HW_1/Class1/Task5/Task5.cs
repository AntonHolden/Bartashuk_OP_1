using System.Numerics;

namespace Task5
{
    public class Task5
    {
        /*
         * Задание 5.1. Запустите функцию `main`, нажав зелёный треугольник слева от её имени. Это действие создаст
         * конфигурацию, которую позже можно отредактировать. В этом задании для ввода данных используются
         * либо параметры командной строки, либо консольный ввод. При запуске через Rider параметры командной строки
         * задаются в окне "Run/Edit configurations..." ("Program arguments:").
         */
        public static void Main(string[] args)
        {
            DemoInput(args);
            ComputeFib(args);
        }

        /*
         * Задание 5.2. Разберите код функции `demoInput`.
         */
        internal static void DemoInput(string[] args) //   Данная функция содержит входящий параметр: массив строк "args", содержащий
        {                                             // параметры командной строки;
            string name;                              //   Объявляем переменную "name" типа данных "string";
            if (args.Length > 0)                      //   Если массив "args", не пустой, т.е. если мы для ввода данных использовали
            {                                         // командную строку, то программа инициализирует переменную "name" данными из
                name = string.Join(' ', args);        // командной строки; Метод string.join(' ', args) возвращает строку,  
            }                                         // "склеенную" из элементов массива "args", разделённых между собой пробелами;
            else                                      //   Если же массив "args" пустой, тогда никаких параметров из командной строки не 
            {                                         // передавалось;
                Console.WriteLine("Введите имя:");    //
                name = Console.ReadLine()!;           //   Поэтому инициализируем переменную "name" данными, введёнными в консоль;
            }                                         //
                                                      //
            Console.WriteLine($"Привет, {name}!");    //   Печатаем приветствие человека с именем "name".
        }

        /*
         * Задание 5.3. Напишите программу, которая принимает на вход целое неотрицательное число
         * (либо параметром командной строки, либо с клавиатуры) и печатает число Фибоначчи:
         * https://ru.wikipedia.org/wiki/%D0%A7%D0%B8%D1%81%D0%BB%D0%B0_%D0%A4%D0%B8%D0%B1%D0%BE%D0%BD%D0%B0%D1%87%D1%87%D0%B8
         * с соответствующим номером. Мы считаем, что нулевым числом Фибоначчи является число 0, а первым — число 1.
         *
         * Для представления чисел Фибоначчи следует использовать длинные целые:
         * - https://docs.microsoft.com/ru-ru/dotnet/api/system.numerics.biginteger?view=net-6.0
         */

        internal static BigInteger Fib(int n)
        {
            BigInteger x1 = 0;
            BigInteger x2 = 1;
            BigInteger temp;
            if (n == 0) return 0;
            for (int i = 2; i <= n; i++)
            {
                temp = x2;
                x2 += x1;
                x1 = temp;
            }
            return x2;
        }

        internal static void ComputeFib(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(Fib(int.Parse(args[0])));
            }
            else
            {
                Console.WriteLine(Fib(int.Parse(Console.ReadLine())));
            }
        }
    }
}