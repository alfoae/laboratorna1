using System;
using System.Collections.Generic;
using System.Linq;
using RentalService;

class Program
{
    static List<Product> products = new List<Product>();
    static List<Client> clients = new List<Client>();
    static List<Booking> bookings = new List<Booking>();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        products = DataManager.LoadProducts();

        MainMenu();
    }

    // ===================== ГОЛОВНЕ МЕНЮ =====================

    static void MainMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== ЛАСКАВО ПРОСИМО ДО СЕРВІСУ ПРОКАТУ АВТО ===\n");
            Console.WriteLine("Виберіть що вам потрібно:");
            Console.WriteLine("1. Перегляд товарів");
            Console.WriteLine("2. Розрахунок покупки");
            Console.WriteLine("3. Інформація про магазин");
            Console.WriteLine("4. Налаштування");
            Console.WriteLine("5. Добавити свій товар");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ShowProductsPage();
                    break;

                case "2":
                    PurchaseCalc();
                    break;

                case "3":
                    ShopInfo();
                    break;

                case "4":
                    SettingsPage();
                    break;

                case "5":
                    AdminLoginMenu();
                    break;

                case "0":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // ====================== ОСНОВНІ СТОРІНКИ ======================

    static void ShowProductsPage()
    {
        Console.Clear();
        Console.WriteLine("=== Т О В А Р И ===");

        if (products.Count == 0)
            Console.WriteLine("Порожньо.");
        else
            foreach (var p in products)
                Console.WriteLine(p);

        Console.WriteLine("\nНатисніть будь-яку кнопку для повернення в меню…");
        Console.ReadKey();
    }

    static void PurchaseCalc()
    {
        Console.Clear();
        Console.WriteLine("=== Р О З Р А Х У Н О К   П О К У П К И ===");
        Console.WriteLine("Функція ще не реалізована.");
        Console.WriteLine("\nНатисніть будь-яку кнопку…");
        Console.ReadKey();
    }

    static void ShopInfo()
    {
        Console.Clear();
        Console.WriteLine("=== І Н Ф О Р М А Ц І Я ===");
        Console.WriteLine("Сервіс прокату авто, кращі ціни, найкращі машини.");
        Console.WriteLine("\nНатисніть будь-яку кнопку…");
        Console.ReadKey();
    }

    static void SettingsPage()
    {
        Console.Clear();
        Console.WriteLine("=== Н А Л А Ш Т У В А Н Н Я ===");
        Console.WriteLine("Функція поки що порожня.");
        Console.WriteLine("\nНатисніть будь-яку кнопку…");
        Console.ReadKey();
    }

    // ====================== АДМІНСЬКИЙ ВХІД ======================

    static void AdminLoginMenu()
    {
        Console.Clear();
        Console.WriteLine("=== В Х І Д  А Д М І Н А ===");

        string login = "admin";
        string pass = "1234";
        int attempts = 0;

        while (attempts < 3)
        {
            Console.Write("\nЛогін: ");
            string l = Console.ReadLine();

            Console.Write("Пароль: ");
            string p = ReadPassword();

            if (l == login && p == pass)
            {
                AdminMenu();
                return;
            }

            attempts++;
            Console.WriteLine($"\nНевірні дані. Спроба {attempts}/3");
        }

        Console.WriteLine("\nСпроби закінчилися. Повернення в головне меню…");
        Console.ReadKey();
    }

    // ====================== АДМІНСЬКЕ МЕНЮ ======================

    static void AdminMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("==== М Е Н Ю ====");
            Console.WriteLine("Виберіть що вам потрібно:");
            Console.WriteLine("1. Показати всі товари");
            Console.WriteLine("2. Додати товар");
            Console.WriteLine("3. Статистика");
            Console.WriteLine("4. Пошук товару");
            Console.WriteLine("5. Створити бронювання");
            Console.WriteLine("6. Зберегти у файл");
            Console.WriteLine("0. Вийти в головне меню");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Admin_ShowProducts();
                    break;

                case "2":
                    Admin_AddProduct();
                    break;

                case "3":
                    Admin_Stats();
                    break;

                case "4":
                    Admin_Search();
                    break;

                case "5":
                    Admin_Booking();
                    break;

                case "6":
                    DataManager.SaveProducts(products);
                    Console.WriteLine("Збережено.");
                    Console.ReadKey();
                    break;

                case "0":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // ====================== АДМІНСЬКІ ФУНКЦІЇ ======================

    static void Admin_ShowProducts()
    {
        Console.Clear();
        Console.WriteLine("=== Т О В А Р И ===");

        if (products.Count == 0)
            Console.WriteLine("Порожньо.");
        else
            foreach (var p in products)
                Console.WriteLine(p);

        Console.WriteLine("\nНатисніть будь-яку кнопку…");
        Console.ReadKey();
    }

    static void Admin_AddProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Д О Д А Т И   Т О В А Р ===");
        Console.WriteLine("Введіть 0 щоб скасувати.");

        int id = products.Count > 0 ? products.Max(x => x.Id) + 1 : 1;

        Console.Write("Назва: ");
        string name = Console.ReadLine();
        if (name == "0") return;

        double price;
        while (true)
        {
            Console.Write("Ціна: ");
            string s = Console.ReadLine();
            if (s == "0") return;

            if (double.TryParse(s, out price))
                break;

            Console.WriteLine("Помилка.");
        }

        int count;
        while (true)
        {
            Console.Write("Кількість: ");
            string s = Console.ReadLine();
            if (s == "0") return;

            if (int.TryParse(s, out count))
                break;

            Console.WriteLine("Помилка.");
        }

        products.Add(new Product(id, name, price, count, true, DateTime.Now));

        Console.WriteLine("\nТовар додано.");
        Console.ReadKey();
    }

    static void Admin_Stats()
    {
        Console.Clear();
        Console.WriteLine("=== С Т А Т И С Т И К А ===");

        if (products.Count == 0)
        {
            Console.WriteLine("Порожньо.");
            Console.ReadKey();
            return;
        }

        double total = products.Sum(p => p.PricePerDay * p.AvailableCount);
        double avg = products.Average(p => p.PricePerDay);
        double min = products.Min(p => p.PricePerDay);
        double max = products.Max(p => p.PricePerDay);

        Console.WriteLine($"Загальна вартість: {total}");
        Console.WriteLine($"Середня ціна: {avg}");
        Console.WriteLine($"Мінімальна: {min}");
        Console.WriteLine($"Максимальна: {max}");

        Console.WriteLine("\nНатисніть будь-яку кнопку…");
        Console.ReadKey();
    }

    static void Admin_Search()
    {
        Console.Clear();
        Console.WriteLine("=== П О Ш У К ===");
        Console.WriteLine("Введіть 0 для відміни.");
        Console.Write("Пошук: ");

        string q = Console.ReadLine();
        if (q == "0") return;

        foreach (var p in products)
        {
            if (p.Name.ToLower().Contains(q.ToLower()))
            {
                Console.WriteLine("Знайдено: " + p);
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("\nНічого не знайдено.");
        Console.ReadKey();
    }

    static void Admin_Booking()
    {
        Console.Clear();
        Console.WriteLine("=== Б Р О Н Ю В А Н Н Я ===");
        Console.WriteLine("Введіть 0 щоб скасувати.");

        if (products.Count == 0)
        {
            Console.WriteLine("Немає товарів.");
            Console.ReadKey();
            return;
        }

        Console.Write("Ім'я клієнта: ");
        string name = Console.ReadLine();
        if (name == "0") return;

        int cid = clients.Count + 1;
        clients.Add(new Client(cid, name, "", "", DateTime.Now, false));

        Admin_ShowProducts();

        int pid;
        while (true)
        {
            Console.Write("ID товару: ");
            string s = Console.ReadLine();
            if (s == "0") return;

            if (int.TryParse(s, out pid) && products.Any(x => x.Id == pid))
                break;

            Console.WriteLine("Помилка.");
        }

        int days;
        while (true)
        {
            Console.Write("Днів оренди: ");
            string s = Console.ReadLine();
            if (s == "0") return;

            if (int.TryParse(s, out days) && days > 0)
                break;

            Console.WriteLine("Помилка.");
        }

        var prod = products.First(p => p.Id == pid);
        double total = prod.PricePerDay * days;

        int bid = bookings.Count + 1;
        bookings.Add(new Booking(bid, cid, pid, days, total, DateTime.Now));

        Console.WriteLine("\nБронювання створено!");
        Console.ReadKey();
    }


    // ====================== ПРИХОВАНИЙ ВВІД ПАРОЛЯ ======================

    static string ReadPassword()
    {
        string pass = "";
        ConsoleKeyInfo k;

        while ((k = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (!char.IsControl(k.KeyChar))
            {
                pass += k.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return pass;
    }
}
