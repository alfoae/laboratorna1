using System;
using System.Collections.Generic;
using RentalService;

namespace RentalService
{
    public static class PurchaseModule
    {
        public static void Run(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== Р О З Р А Х У Н О К   П О К У П К И ===\n");

            //Авто-очистка товарів з 0 кількістю
            for (int i = products.Count - 1; i >= 0; i--)
            {
                if (products[i].AvailableCount <= 0)
                    products.RemoveAt(i);
            }

            if (products.Count == 0)
            {
                Console.WriteLine("Немає товарів для покупки.");
                Console.ReadKey();
                return;
            }

            List<(Product prod, int count, int days)> cart = new();

            foreach (var product in products)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== Р О З Р А Х У Н О К   П О К У П К И ===\n");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{product.Id}. {product.Name} — {product.PricePerDay} грн/доба — доступно: {product.AvailableCount}");
                    Console.ResetColor();

                    Console.Write("\nВведіть кількість машин (0 — пропустити): ");
                    string cInput = Console.ReadLine();

                    if (!int.TryParse(cInput, out int carCount) || carCount < 0)
                    {
                        Error("Кількість машин має бути числом ≥ 0");
                        continue;
                    }

                    if (carCount == 0)
                    {
                        cart.Add((product, 0, 0));
                        break;
                    }

                    if (carCount > product.AvailableCount)
                    {
                        Error($"На складі є тільки {product.AvailableCount}");
                        continue;
                    }

                    Console.Write("Кількість днів: ");
                    string dInput = Console.ReadLine();

                    if (!int.TryParse(dInput, out int days) || days < 1)
                    {
                        Error("Дні мають бути ≥ 1");
                        continue;
                    }

                    cart.Add((product, carCount, days));
                    break;
                }
            }

            // Підтвердження
            Console.Clear();
            Console.WriteLine("1 — продовжити\n0 — назад");

            string confirm = Console.ReadLine();
            if (confirm == "0") return;

            // Обчислення
            double total = 0;

            Console.Clear();
            Console.WriteLine("=== Р Е З У Л Ь Т А Т ===\n");

            foreach (var item in cart)
            {
                if (item.count == 0) continue;

                double sum = item.count * item.days * item.prod.PricePerDay;
                total += sum;

                Console.WriteLine($"{item.prod.Name}: {sum} грн");
            }

            Random rnd = new Random();
            double k = rnd.Next(0, 11);

            double final = total - (total * k / 100);
            final = Math.Round(final, 2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nСума: {total}");
            Console.WriteLine($"Знижка: {k}%");
            Console.WriteLine($"Разом: {final}");
            Console.ResetColor();

            // Списання
            for (int i = 0; i < products.Count; i++)
            {
                foreach (var c in cart)
                {
                    if (products[i].Id == c.prod.Id)
                    {
                        Product p = products[i];
                        p.AvailableCount -= c.count;
                        products[i] = p;
                    }
                }
            }

            // Видалення товарів з 0
            for (int i = products.Count - 1; i >= 0; i--)
            {
                if (products[i].AvailableCount <= 0)
                    products.RemoveAt(i);
            }

            DataManager.SaveProducts(products);

            Console.WriteLine("\nEnter — назад...");
            Console.ReadKey();
        }

        private static void Error(string msg)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ПОМИЛКА:");
            Console.WriteLine(msg);
            Console.ResetColor();
            Console.ReadKey();
        }

        //==========ПОКАЗАТИ ВСІ ТОВАРИ==========

        public static void ShowProductsPage(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== Т О В А Р И ===\n");

            if (products.Count == 0)
            {
                Console.WriteLine("Порожньо.");
                Console.ReadKey();
                return;
            }

            int wName = 10, wPrice = 12, wCnt = 10, wId = 5, wDate = 12;

            for (int i = 0; i < products.Count; i++)
            {
                wName = Math.Max(wName, products[i].Name.Length + 2);
                wPrice = Math.Max(wPrice, products[i].PricePerDay.ToString().Length + 4);
                wCnt = Math.Max(wCnt, products[i].AvailableCount.ToString().Length + 2);
                wId = Math.Max(wId, products[i].Id.ToString().Length + 2);
            }

            void line()
            {
                Console.WriteLine(new string('-', wName + wPrice + wCnt + wId + wDate + 10));
            }

            line();
            Console.WriteLine(
                $"|{"Назва".PadRight(wName)}" +
                $"|{"Ціна/доба".PadRight(wPrice)}" +
                $"|{"Кількість".PadRight(wCnt)}" +
                $"|{"ID".PadRight(wId)}" +
                $"|{"Дата".PadRight(wDate)}|"
            );
            line();

            for (int i = 0; i < products.Count; i++)
            {
                Product p = products[i];
                Console.WriteLine(
                    $"|{p.Name.PadRight(wName)}" +
                    $"|{(p.PricePerDay + " грн").PadRight(wPrice)}" +
                    $"|{p.AvailableCount.ToString().PadRight(wCnt)}" +
                    $"|{p.Id.ToString().PadRight(wId)}" +
                    $"|{p.Added.ToString("yyyy-MM-dd").PadRight(wDate)}|"
                );
            }

            line();

            Console.WriteLine("\nНатисніть будь-яку кнопку…");
            Console.ReadKey();
        }

        public static void ShopInfo(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== І Н Ф О Р М А Ц І Я ===");
            Console.WriteLine("Сервіс прокату авто, кращі ціни, найкращі машини.");
            Console.WriteLine("\nНатисніть будь-яку кнопку…");
            Console.ReadKey();
        }

        // ====================== АДМІНСЬКИЙ ВХІД ======================

        public static void AdminLoginMenu(List<Product> products, Action adminMenuCallback)
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
                string p = ReadPassword(products);

                if (l == login && p == pass)
                {
                    adminMenuCallback();
                    return;
                }

                attempts++;
                Console.WriteLine($"\nНевірні дані. Спроба {attempts}/3");
            }

            Console.WriteLine("\nСпроби закінчилися. Повернення в головне меню…");
            Console.ReadKey();
        }

        public static void Admin_AddProduct(List<Product> products)
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

        public static void Admin_Stats(List<Product> products)
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

            Console.WriteLine($"Різновидів товару: {products.Count}");
            Console.WriteLine($"Загальна вартість: {total}");
            Console.WriteLine($"Середня ціна: {avg}");
            Console.WriteLine($"Мінімальна: {min}");
            Console.WriteLine($"Максимальна: {max}");

            Console.WriteLine("\nНатисніть будь-яку кнопку…");
            Console.ReadKey();
        }

        public static void Admin_Search(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== П О Ш У К ===");
            Console.WriteLine("Введіть 0 для відміни.");
            Console.Write("Пошук: ");

            string q = Console.ReadLine();
            if (q == "0") return;

            bool foundAny = false;
            Console.WriteLine("\nЗнайдені товари:\n");

            foreach (var p in products)
            {
                if (p.Name.ToLower().Contains(q.ToLower()))
                {
                    Console.WriteLine(p);
                    foundAny = true;
                }
            }

            if (!foundAny)
            {
                Console.WriteLine("Нічого не знайдено.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }



        public static void Admin_DeleteProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== В И Д А Л И Т И   Т О В А Р ===\n");

            if (products.Count == 0)
            {
                Console.WriteLine("Порожньо.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < products.Count - 1; i++)
                for (int j = i + 1; j < products.Count; j++)
                    if (string.Compare(products[i].Name, products[j].Name) > 0)
                    {
                        Product tmp = products[i];
                        products[i] = products[j];
                        products[j] = tmp;
                    }

            for (int i = 0; i < products.Count; i++)
                Console.WriteLine($"{i + 1}. {products[i].Name}");

            Console.Write("\nВведіть номер: ");

            if (!int.TryParse(Console.ReadLine(), out int idx)) return;

            idx--;

            if (idx < 0 || idx >= products.Count) return;

            products.RemoveAt(idx);

            Console.WriteLine("Видалено.");
            Console.ReadKey();
        }




        // ====================== ПРИХОВАНИЙ ВВІД ПАРОЛЯ ======================

        public static string ReadPassword(List<Product> products)
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


        public static void SortProductsByName(List<Product> products)
        {
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = i + 1; j < products.Count; j++)
                {
                    if (string.Compare(products[i].Name, products[j].Name, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        Product tmp = products[i];
                        products[i] = products[j];
                        products[j] = tmp;
                    }
                }
            }
        }

        public static void SortProductsByPrice(List<Product> products)
        {
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = i + 1; j < products.Count; j++)
                {
                    if (products[i].PricePerDay > products[j].PricePerDay)
                    {
                        Product tmp = products[i];
                        products[i] = products[j];
                        products[j] = tmp;
                    }
                }
            }
        }

        public static void SortProductsByDate(List<Product> products)
        {
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = i + 1; j < products.Count; j++)
                {
                    if (products[i].Added > products[j].Added)
                    {
                        Product tmp = products[i];
                        products[i] = products[j];
                        products[j] = tmp;
                    }
                }
            }
        }

        public static void SortProductsById(List<Product> products)
        {
            for (int i = 0; i < products.Count - 1; i++)
            {
                for (int j = i + 1; j < products.Count; j++)
                {
                    if (products[i].Id > products[j].Id)
                    {
                        Product tmp = products[i];
                        products[i] = products[j];
                        products[j] = tmp;
                    }
                }
            }
        }
    }
}