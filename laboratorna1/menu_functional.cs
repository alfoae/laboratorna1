using System;
using System.Collections.Generic;
using RentalService;
using static RentalService.DataManager;

namespace RentalService
{
    public static class PurchaseModule
    {

        public static void Run(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== Р О З Р А Х У Н О К   П О К У П К И ===\n");

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

            Console.Clear();
            Console.WriteLine("1 — продовжити\n0 — назад");

            string confirm = Console.ReadLine();
            if (confirm == "0") return;

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

            double result = Math.Sqrt(final);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nСума: {total}");
            Console.WriteLine($"Знижка: {k}%");
            Console.WriteLine($"Разом: {final}");
            Console.WriteLine($"Корінь: {result}");
            Console.ResetColor();

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

        public static void ShopInfo()
        {
            Console.Clear();
            Console.WriteLine("=== І Н Ф О Р М А Ц І Я ===");
            Console.WriteLine("Сервіс прокату авто, кращі ціни, найкращі машини.");
            Console.WriteLine("\nНатисніть будь-яку кнопку…");
            Console.ReadKey();
        }

        public static void Admin_AddProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== Д О Д А Т И   Т О В А Р ===");
            Console.WriteLine("Введіть 0 щоб скасувати.");

            int id = DataManager.GetNextProductId();


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

            var newProduct = new Product(id, name, price, count, true, DateTime.Now);

            products.Add(newProduct);
            DataManager.AppendProduct(newProduct);

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

            var found = products
                .Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (found.Count == 0)
            {
                Console.WriteLine("\nНічого не знайдено.");
                Console.ReadKey();
                return;
            }

            int wName = 10, wPrice = 12, wCnt = 10, wId = 5, wDate = 12;

            foreach (var p in found)
            {
                wName = Math.Max(wName, p.Name.Length + 2);
                wPrice = Math.Max(wPrice, (p.PricePerDay + " грн").Length + 2);
                wCnt = Math.Max(wCnt, p.AvailableCount.ToString().Length + 2);
                wId = Math.Max(wId, p.Id.ToString().Length + 2);
            }

            void line()
            {
                Console.WriteLine(new string('-', wName + wPrice + wCnt + wId + wDate + 6));
            }

            Console.WriteLine("\nЗнайдені товари:\n");
            line();
            Console.WriteLine(
                $"|{"Назва".PadRight(wName)}" +
                $"|{"Ціна/доба".PadRight(wPrice)}" +
                $"|{"Кількість".PadRight(wCnt)}" +
                $"|{"ID".PadRight(wId)}" +
                $"|{"Дата".PadRight(wDate)}|"
            );
            line();

            foreach (var p in found)
            {
                Console.WriteLine(
                    $"|{p.Name.PadRight(wName)}" +
                    $"|{(p.PricePerDay + " грн").PadRight(wPrice)}" +
                    $"|{p.AvailableCount.ToString().PadRight(wCnt)}" +
                    $"|{p.Id.ToString().PadRight(wId)}" +
                    $"|{p.Added:yyyy-MM-dd}".PadRight(wDate) + "|"
                );
            }

            line();
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

            DataManager.SaveProducts(products);


            Console.WriteLine("Видалено.");
            Console.ReadKey();


        }




        // ====================== ПРИХОВАНИЙ ВВІД ПАРОЛЯ ======================

        public static string ReadPassword()
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

        public static void Admin_EditProduct(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("=== Р Е Д А Г У В А Н Н Я   Т О В А Р У ===\n");

            if (products.Count == 0)
            {
                Console.WriteLine("Порожньо.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < products.Count; i++)
                Console.WriteLine($"{products[i].Id}. {products[i].Name}");

            Console.Write("\nВведіть ID товару: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
                return;

            var p = products.FirstOrDefault(x => x.Id == id);
            if (p.Id == 0)
            {
                Console.WriteLine("Товар не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Р Е Д А Г У В А Н Н Я ===\n");

            // --- РЕДАГУВАННЯ НАЗВИ ---
            Console.WriteLine($"Поточна назва: {p.Name}");
            Console.Write("Нова назва (0 — залишити): ");
            string newName = Console.ReadLine();
            if (newName != "0" && !string.IsNullOrWhiteSpace(newName))
                p.Name = newName;


            // --- РЕДАГУВАННЯ ЦІНИ ---
            Console.WriteLine($"\nПоточна ціна: {p.PricePerDay}");
            Console.Write("Нова ціна (0 — залишити): ");
            string priceInput = Console.ReadLine();

            if (priceInput != "0")
            {
                if (double.TryParse(priceInput, out double newPrice) && newPrice >= 0)
                    p.PricePerDay = newPrice;
                else
                    Console.WriteLine("❗ Невірна ціна — залишено як було.");
            }


            // --- РЕДАГУВАННЯ КІЛЬКОСТІ ---
            Console.WriteLine($"\nПоточна кількість: {p.AvailableCount}");
            Console.Write("Нова кількість (0 — залишити): ");
            string countInput = Console.ReadLine();

            if (countInput != "0")
            {
                if (int.TryParse(countInput, out int newCount) && newCount >= 0)
                    p.AvailableCount = newCount;
                else
                    Console.WriteLine("❗ Невірна кількість — залишено як було.");
            }


            for (int i = 0; i < products.Count; i++)
                if (products[i].Id == p.Id)
                {
                    products[i] = p;
                    break;
                }

            // ЗБЕРЕЖЕННЯ У CSV
            SaveProducts(products);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nТовар успішно оновлено!");
            Console.ResetColor();

            Console.ReadKey();
        }

        public static void Register()
        {
            Console.Clear();
            Console.WriteLine("=== С Т В О Р Е Н Н Я   А К А У Н Т У ===\n");

            var users = DataManager.LoadUsers();

            // Заборонені символи
            string forbidden = " !#$%^&*()+=[]{}|;:'\",<>/?`~\\";

            // EMAIL
            string email;
            while (true)
            {
                Console.Write("Email (приклад: google@gmail.com): ");
                email = Console.ReadLine();

                if (!email.Contains('@') || !email.Contains('.'))
                {
                    Console.WriteLine("Невірний формат email.");
                    continue;
                }

                if (users.Any(u => u.Email == email))
                {
                    Console.WriteLine("Такий email вже існує.");
                    continue;
                }

                if (email.Any(ch => forbidden.Contains(ch)))
                {
                    Console.WriteLine($"Не можна використовувати такі символи: {forbidden}");
                    continue;
                }

                break;
            }

            // LOGIN
            string login;
            while (true)
            {
                Console.Write("Логін (мін 4 символи): ");
                login = Console.ReadLine();

                if (login.Length < 4)
                {
                    Console.WriteLine("Закороткий.");
                    continue;
                }

                if (login.Any(ch => forbidden.Contains(ch)))
                {
                    Console.WriteLine($"Не можна використовувати такі символи: {forbidden}");
                    continue;
                }

                if (users.Any(u => u.Login == login))
                {
                    Console.WriteLine("Такий логін вже існує.");
                    continue;
                }

                break;
            }

            // PASSWORD
            string pass;
            while (true)
            {
                Console.Write("Пароль (мін 4 символи): ");
                pass = Console.ReadLine();

                if (pass.Length < 4)
                {
                    Console.WriteLine("Закороткий.");
                    continue;
                }

                if (pass.Any(ch => forbidden.Contains(ch)))
                {
                    Console.WriteLine($"Не можна використовувати такі символи: {forbidden}");
                    continue;
                }

                break;
            }

            int id = DataManager.GetNextUserId();
            string hash = Hash(pass);

            User u = new User(id, email, login, hash, DateTime.Now);
            DataManager.AppendUser(u);

            Console.WriteLine("\nАкаунт створено!");
            Console.ReadKey();
        }

        public static void Login()
        {
            Console.Clear();
            Console.WriteLine("=== В Х І Д ===\n");

            var users = DataManager.LoadUsers();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            var user = users.FirstOrDefault(u => u.Email == email);

            if (user.Id == 0)
            {
                Console.WriteLine("Такого email немає.");
                Console.ReadKey();
                return;
            }

            Console.Write("Логін: ");
            string login = Console.ReadLine();

            if (user.Login != login)
            {
                Console.WriteLine("Невірний логін.");
                Console.ReadKey();
                return;
            }

            Console.Write("Пароль: ");
            
            string pass = ReadPassword();

            if (user.PasswordHash != Hash(pass))
            {
                Console.WriteLine("Невірний пароль.");
                Console.ReadKey();
                return;
            }

            if (email == "admin@gmail.com" && login == "admin" && pass == "1234")
            {
                Program.IsLoggedIn = true;
                Program.IsAdmin = true;
                Console.WriteLine("\nВхід виконано. Ви увійшли як адміністратор.");
                Console.ReadKey();
                return;
            }

            Program.IsLoggedIn = true;
            Program.IsAdmin = false;
            Console.WriteLine("\nВхід виконано.");
            Console.ReadKey();
        }

    }
}