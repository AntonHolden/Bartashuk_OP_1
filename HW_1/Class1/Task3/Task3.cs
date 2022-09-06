namespace Task3
{
    public class Task3
    {
/*
 * Прежде чем приступать к выполнению заданий, допишите к ним тесты.
 */

/*
 * Задание 3.1. Для данного вещественного x найти значение следующей функции f, принимающей значения целого типа:
 * 	        0,	если x < 0,
 * f(x) = 	1,	если x принадлежит [0, 1), [2, 3), …,
           −1,	если x принадлежит [1, 2), [3, 4), … .
 */
        internal static double F(double x) => x < 0 ? 0 : ((Math.Floor(x) % 2 != 0) ? -1 : 1);

/*
 * Задание 3.2. Дан номер года (положительное целое число). Определить количество дней в этом году,
 * учитывая, что обычный год насчитывает 365 дней, а високосный — 366 дней. Високосным считается год,
 * делящийся на 4, за исключением тех годов, которые делятся на 100 и не делятся на 400
 * (например, годы 300, 1300 и 1900 не являются високосными, а 1200 и 2000 — являются).
 */
        internal static int NumberOfDays(int year) => (year % 4 == 0) && (!((year % 100 == 0) && (year % 400 != 0))) ? 366 : 365;

/*
 * Задание 3.3. Локатор ориентирован на одну из сторон света («С» — север, «З» — запад,
 * «Ю» — юг, «В» — восток) и может принимать три цифровые команды поворота:
 * 1 — поворот налево, −1 — поворот направо, 2 — поворот на 180°.
 * Дан символ C — исходная ориентация локатора и целые числа N1 и N2 — две посланные команды.
 * Вернуть ориентацию локатора после выполнения этих команд.
 */
        internal static char Rotate2(char orientation, int cmd1, int cmd2)
        {
            static int GetIndex(char side)
            {
                var directions_to_index = new Dictionary<char, int>() { { 'С', 3 }, { 'В', 2 }, { 'Ю', 1 }, { 'З', 0 } };
                return directions_to_index[side];
            }
            static Dictionary<int,char> Directions()
            {
                var index_to_directions = new Dictionary<int, char>() { { 3, 'С' }, { 2, 'В' }, { 1, 'Ю' }, { 0, 'З' }, { -1, 'С' }, { -2, 'В' }, { -3, 'Ю' } };
                return index_to_directions;
            }

            char rotate1(char orientation, int cmd) => Directions()[(GetIndex(orientation) + cmd) % 4];

            return rotate1(rotate1(orientation,cmd1),cmd2);
        }

/*
 * Задание 3.4. Дано целое число в диапазоне 20–69, определяющее возраст (в годах).
 * Вернуть строку-описание указанного возраста, обеспечив правильное согласование
 * числа со словом «год», например: 20 — «двадцать лет», 32 — «тридцать два года»,
 * 41 — «сорок один год».
 *
 * Пожалуйста, научитесь делать такие вещи очень аккуратно. Программное обеспечение
 * переполнено некорректными с точки зрения русского языка решениями.
 */
        internal static String AgeDescription(int age)
        {
            var beginnings_of_tens = new Dictionary<int, string>() { { 2, "два" }, { 3, "три" }, { 4, "сорок" }, { 5, "пять" }, { 6, "шесть" } };
            var endings_of_tens = new Dictionary<int, string>() { {2,"дцать" }, { 3, "дцать" }, { 4, "" }, {5, "десят" }, { 6, "десят" }};
            var units = new Dictionary<int, string>() { { 0, "" }, { 1, " один" }, { 2, " два" }, { 3, " три" }, { 4, " четыре" }, { 5, " пять" }, { 6, " шесть" }, { 7, " семь" }, { 8, " восемь" }, { 9, " девять" }};
            var name_of_age = new Dictionary<int, string>() { { 0, " лет" }, { 1, " год" } };
            for (int i = 2;i<5;i++)
            {
                name_of_age.Add(i, " года");
            }
            for (int i = 5; i < 10; i++)
            {
                name_of_age.Add(i, " лет");
            }
            return beginnings_of_tens[age/10] + endings_of_tens[age/10] + units[age%10] + name_of_age[age%10];
        }

        public static void Main(string[] args)
        {
            F(2.01);
            NumberOfDays(2025);
            Rotate2('В', 2, -1);
            AgeDescription(56);
        }
    }
}