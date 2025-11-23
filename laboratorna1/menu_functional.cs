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
    }
}
