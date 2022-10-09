using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;
using System.Globalization;
using System.Runtime.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Task2
{
    public class Task2
    {

        /*
         * В этом задании необходимо написать программу, способную табулировать сразу несколько
         * функций одной вещественной переменной на одном заданном отрезке.
         */


        // Сформируйте набор как минимум из десяти вещественных функций одной переменной
        internal static Dictionary<FunctionName, OneVariableFunction> AvailableFunctions =
                    new Dictionary<FunctionName, OneVariableFunction>
                    {
                { "sqr", x => x * x },
                { "sin", Math.Sin },
                { "cos", Math.Cos },
                { "tg", Math.Tan },
                { "ctg", x => 1.0/Math.Cos(x) },
                { "abs", Math.Abs },
                { "ln", Math.Log },
                { "sqrt", Math.Sqrt },
                { "cbr", x => x * x * x },
                { "cbrt", Math.Cbrt }
                    };

        // Тип данных для представления входных данных
        internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

        // Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {
            var formatterDot = new NumberFormatInfo { NumberDecimalSeparator = "." };
            var formatterComma = new NumberFormatInfo { NumberDecimalSeparator = "," };

            string[] mainArgs;
            if (args.Length > 0)
            {
                mainArgs = args;
            }
            else
            {
                mainArgs = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (mainArgs.Length < 3)
            {
                Console.WriteLine("Вы ввели слишком мало чисел!");
                return null;
            }

            double fromX, toX;
            if ((!double.TryParse(mainArgs[0], NumberStyles.Float, formatterDot, out fromX)) && (!double.TryParse(mainArgs[0], NumberStyles.Float, formatterComma, out fromX)))
            {
                Console.WriteLine("Вы некорректно ввели первое число!");
                return null;
            }
            if ((!double.TryParse(mainArgs[1], NumberStyles.Float, formatterDot, out toX)) && (!double.TryParse(mainArgs[1], NumberStyles.Float, formatterComma, out toX)))
            {
                Console.WriteLine("Вы некорректно ввели второе число!");
                return null;
            }

            int numberOfPoints;
            if ((!int.TryParse(mainArgs[2], out numberOfPoints)) || (numberOfPoints < 0) || (numberOfPoints > 15))
            {
                Console.WriteLine("Вы ввели некорректно ввели количество знаков после десятичной точки!");
                return null;
            }

            var functionNames = new List<string>();

            for (int i = 3; i < mainArgs.Length; i++)
            {
                functionNames.Add(mainArgs[i]);
            }

            return new InputData(fromX, toX, numberOfPoints, functionNames);

        }

        // Тип данных для представления таблицы значений функций
        // с заголовками столбцов и строками (первый столбец --- значение x,
        // остальные столбцы --- значения функций). Одно из полей --- количество знаков
        // после десятичной точки.
        internal record FunctionTable
        {
            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.

            private int blank, numberOfPoints, numberOfFunctions;
            private StringBuilder table = new StringBuilder();

            public FunctionTable(InputData input)
            {
                double sample = input.ToX;
                if (input.FunctionNames.Contains("cbr"))
                {
                    sample *= sample * sample;
                }
                else if (input.FunctionNames.Contains("sqr"))
                {
                    sample *= sample;
                }
                blank = Math.Max(Math.Floor(sample).ToString().Length + input.NumberOfPoints + 2, 6);

                numberOfPoints = input.NumberOfPoints;
                numberOfFunctions = input.FunctionNames.Count;

                table.Append(String.Concat(Enumerable.Repeat('-', blank * (numberOfFunctions + 1) + numberOfFunctions + 2))).Append('\n');

                int currentBlank = blank / 2;
                table.Append('|').Append(String.Concat(Enumerable.Repeat(' ', (blank % 2 == 0) ? (currentBlank - 1) : currentBlank))).Append("x").Append(String.Concat(Enumerable.Repeat(' ', currentBlank)));

                foreach (string function in input.FunctionNames)
                {
                    currentBlank = blank - function.Length;
                    table.Append('|').Append(String.Concat(Enumerable.Repeat(' ', (currentBlank % 2 == 0) ? (currentBlank / 2) : ((currentBlank / 2) + 1)))).Append(function).Append(String.Concat(Enumerable.Repeat(' ', (currentBlank % 2 == 0) ? (currentBlank / 2) : (currentBlank - ((currentBlank / 2) + 1)))));
                }
            }

            public void AddValue(double x)
            {
                table.Append('|').Append('\n').Append(String.Concat(Enumerable.Repeat('-', blank * (numberOfFunctions + 1) + numberOfFunctions + 2))).Append('\n');

                table.Append('|').Append(x.ToString().PadRight(blank));
            }
            public void AddFuncValue(double x)
            {
                double res = Math.Round(x, numberOfPoints, MidpointRounding.AwayFromZero);

                table.Append('|').Append(res.ToString().PadLeft(blank));
            }

            public void DrawLastHorizontalLine()
            {
                table.Append('|').Append('\n').Append(String.Concat(Enumerable.Repeat('-', blank * (numberOfFunctions + 1) + numberOfFunctions + 2)));
            }

            public override string ToString()
            {
                return table.ToString();
            }
        }

        /*
         * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
         * с заданным количеством точек.
         */
        internal static FunctionTable? tabulate(InputData input)
        {
            var table = new FunctionTable(input);
            try
            {
                for (double currentX = Math.Floor(input.FromX); currentX <= Math.Ceiling(input.ToX); currentX = Math.Round(currentX + 1))
                {
                    table.AddValue(currentX);
                    foreach (string function in input.FunctionNames)
                    {
                        table.AddFuncValue(AvailableFunctions[function](currentX));
                    }
                }
            }
            catch
            {
                Console.WriteLine("Вы некорректно ввели название какой-то функции!");
                return null;
            }

            table.DrawLastHorizontalLine();
            return table;
        }

        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            if (args.Length == 0)
            {
                Console.WriteLine("Введите границы отрезка и количество знаков (от 0 до 15 включительно) после десятичной точки (через пробел).\n");
                Console.WriteLine("После этого введите одно или несколько из следующих названий функций:\n");
                Console.WriteLine("sqr (квадрат числа)\n");
                Console.WriteLine("sqrt (квадратный корень из числа)\n");
                Console.WriteLine("cqr (куб числа)\n");
                Console.WriteLine("cqrt (кубический корень из числа)\n");
                Console.WriteLine("sin (синус)\n");
                Console.WriteLine("cos (косинус)\n");
                Console.WriteLine("tg (тангенс)\n");
                Console.WriteLine("ctg (котангенс)\n");
                Console.WriteLine("abs (модуль)\n");
                Console.WriteLine("ln (натуральный логарифм)\n\n");
            }

            var input = prepareData(args);
            if (input == null) return;

            var table = tabulate(input);

            if (table == null) return;
            else
            {
                // Собственно табулирование и печать результата (что надо поменять в этой строке?):
                Console.WriteLine(table.ToString());
            }
        }

    }
}