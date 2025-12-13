using System;
using System.Collections.Generic;
using System.Linq;
using RentalService;



class Program
{
    public static bool IsLoggedIn = false;
    public static bool IsAdmin = false;
    static List<Product> products = new List<Product>();

    static void Main()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        products = DataManager.LoadProducts();

        MainMenu();
    }

    // ===================== ГОЛОВНЕ МЕНЮ =====================

    static void MainMenu()
    {

        while (!Program.IsLoggedIn)
        {
            Console.Clear();
            Console.WriteLine("=== В І Т А Ю ===\n");
            Console.WriteLine("1 — Увійти");
            Console.WriteLine("2 — Створити акаунт");
            Console.WriteLine("0 — Вихід\n");
            Console.Write("Ваш вибір: ");

            string c = Console.ReadLine();

            if (c == "1")
            {
                PurchaseModule.Login();
            }
            else if (c == "2")
            {
                PurchaseModule.Register();
            }
            else if (c == "0")
            {
                Environment.Exit(0);
            }
        }


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
            Console.WriteLine("5. Пошук товару");
            Console.WriteLine("6. админ меню");
            Console.WriteLine("0. Вихід");
            Console.Write("\nВаш вибір: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PurchaseModule.ShowProductsPage(products);
                    break;

                case "2":
                    PurchaseModule.Run(products);
                    break;

                case "3":
                    PurchaseModule.ShopInfo();
                    break;

                case "4":
                    SettingsPage();
                    break;

                case "5":
                    PurchaseModule.Admin_Search(products);
                    break;

                case "6":
                    {
                        if (!Program.IsAdmin)
                        {
                            Console.WriteLine("У вас немає дозволу для входу в адмін-меню.");
                            Console.ReadKey();
                        }
                        else
                        {
                            AdminMenu();
                        }
                        

                        break;
                    }
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
            Console.WriteLine("5. Зберегти у файл");
            Console.WriteLine("6. Видалити товар");
            Console.WriteLine("7. редагувати товар");
            Console.WriteLine("0. Вийти в головне меню");
            Console.Write("\nВаш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PurchaseModule.ShowProductsPage(products);
                    break;

                case "2":
                    PurchaseModule.Admin_AddProduct(products);
                    break;

                case "3":
                    PurchaseModule.Admin_Stats(products);
                    break;

                case "4":
                    PurchaseModule.Admin_Search(products);
                    break;

                case "5":
                    DataManager.SaveProducts(products);
                    Console.WriteLine("Збережено.");
                    Console.ReadKey();
                    break;

                case "6":
                    PurchaseModule.Admin_DeleteProduct(products);
                    break;

                case "7":
                    PurchaseModule.Admin_EditProduct(products);
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

    public static void SettingsPage()
    {
        Console.Clear();
        Console.WriteLine("=== Н А Л А Ш Т У В А Н Н Я ===");
        Console.WriteLine("\n1. Сортування");
        Console.WriteLine("0. Вихід");
        Console.Write("\nВаш вибір: ");

        string choice2 = Console.ReadLine();
        switch (choice2)
        {
            case "1":
                SortMenu();
                break;

            case "0":
                break;

            default:
                Console.WriteLine("Невірний вибір.");
                return;
        }
    }

    public static void SortMenu()
    {
        bool i = false;
        Console.Clear();
        Console.WriteLine("=== С О Р Т У В А Н Н Я ===\n");
        Console.WriteLine("1. За назвою (A → Я)");
        Console.WriteLine("2. За ціною (зростання)");
        Console.WriteLine("3. За датою додавання (старі → нові)");
        Console.WriteLine("4. За ID");
        Console.WriteLine("0. Назад");

        Console.Write("\nВаш вибір: ");
        string s = Console.ReadLine();

        switch (s)
        {
            
            case "1": PurchaseModule.SortProductsByName(products); break;
            case "2": PurchaseModule.SortProductsByPrice(products); break;
            case "3": PurchaseModule.SortProductsByDate(products); break;
            case "4": PurchaseModule.SortProductsById(products); break;
            case "0": i = true; break;
            default: Console.WriteLine("Невірний вибір."); return;
        }
        if (i == true)
        { SettingsPage(); }
        else
        {
            Console.WriteLine("\nГотово! Натисніть будь-що…");
            Console.ReadKey();
        }
    }
}