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
            Console.WriteLine("5. Пошук товару");
            Console.WriteLine("6. Добавити свій товар");
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
                    PurchaseModule.ShopInfo(products);
                    break;

                case "4":
                    SettingsPage(products);
                    break;

                case "5":
                    PurchaseModule.Admin_Search(products);
                    break;

                case "6":
                    PurchaseModule.AdminLoginMenu(products, AdminMenu);

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






    public static void SettingsPage(List<Product> products)
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
        byte i = 0;
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
            case "0": i = 1; break;
            default: Console.WriteLine("Невірний вибір."); return;
        }
        if (i == 1)
        { SettingsPage(products); }
        else
        {
            Console.WriteLine("\nГотово! Натисніть будь-що…");
            Console.ReadKey();
        }
    }
}