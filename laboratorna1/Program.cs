using static tovar;

class menu
{
    static void Main()
    {
        string i;
        short num = 0;
        do
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== ЛАСКАВО ПРОСИМО ДО СЕРВІСУ ПРОКАТУ АВТО ===\n");
            Console.ResetColor();



            switch (num)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Функція в розробці виберіть щось інше (1)\n");
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Ви ввели щось не то :/ спробуйте вибрати від 0 до 4\n");
                    break;
                case 0:
                    Console.Write("виберіть що вам потрібно\n");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. перегляд товарів\n");
            Console.WriteLine("2. розрахунок покупки\n");
            Console.WriteLine("3. інформація про магазин\n");
            Console.WriteLine("4. налаштування\n");
            Console.WriteLine("0. вихід\n");
            i = Console.ReadLine();
            switch (i)
            {
                case "1":
                    Console.Clear();
                    tovaro();
                    break;
                case "2":
                    Console.Clear();
                    num = 1;
                    break;
                case "3":
                    Console.Clear();
                    num = 1;
                    break;
                case "4":
                    Console.Clear();
                    num = 1;
                    break;
                case "0":
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    num = 2;
                    break;
            }
        } while (i != "0");




        Console.WriteLine("\nДякуємо, що скористалися нашим сервісом!");
        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}