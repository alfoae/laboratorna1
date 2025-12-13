using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RentalService
{
    // ===== С Т Р У К Т У Р И =====

    public struct Product
    {
        public int Id;
        public string Name;
        public double PricePerDay;
        public int AvailableCount;
        public bool IsActive;
        public DateTime Added;

        public Product(int id, string name, double price, int count, bool active, DateTime added)
        {
            Id = id;
            Name = name;
            PricePerDay = price;
            AvailableCount = count;
            IsActive = active;
            Added = added;
        }

        public string ToCsv()
        {
            return $"{Id};{Escape(Name)};{PricePerDay.ToString(CultureInfo.InvariantCulture)};{AvailableCount};{IsActive};{Added.ToString("o", CultureInfo.InvariantCulture)}";
        }

        public static bool TryFromCsv(string line, out Product product)
        {
            product = default;
            if (string.IsNullOrWhiteSpace(line)) return false;
            string[] x = line.Split(';');
            if (x.Length < 6) return false;

            if (!int.TryParse(x[0], out int id)) return false;
            string name = Unescape(x[1]);
            if (!double.TryParse(x[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double price)) return false;
            if (!int.TryParse(x[3], out int cnt)) return false;
            if (!bool.TryParse(x[4], out bool active)) return false;
            if (!DateTime.TryParse(x[5], null, DateTimeStyles.RoundtripKind, out DateTime added)) return false;

            product = new Product(id, name, price, cnt, active, added);
            return true;
        }

        private static string Escape(string s) => s?.Replace("\n", "\\n").Replace("\r", "\\r").Replace(";", "\\;") ?? "";
        private static string Unescape(string s) => s?.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\;", ";") ?? "";
    }


    public static class DataManager
    {
        public static string ProductsFile = "products.csv";

        private static readonly string ProductsHeader = "Id;Name;PricePerDay;AvailableCount;IsActive;Added";

        private static void EnsureFile(string path, string header)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, header + Environment.NewLine);
                }
                else
                {
                    var lines = File.ReadAllLines(path);
                    if (lines.Length == 0 || !lines[0].Trim().Equals(header, StringComparison.OrdinalIgnoreCase))
                    {
                        var old = lines;
                        using (var sw = new StreamWriter(path, false))
                        {
                            sw.WriteLine(header);
                            foreach (var l in old.Where(s => !string.IsNullOrWhiteSpace(s)))
                                sw.WriteLine(l);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private static IEnumerable<string> ReadDataLines(string path, string header)
        {
            EnsureFile(path, header);

            string[] lines;

            try
            {
                lines = File.ReadAllLines(path);
            }
            catch
            {
                yield break;
            }

            foreach (var line in lines.Skip(1))
                yield return line;
        }


        private static void AppendLine(string path, string header, string line)
        {
            EnsureFile(path, header);
            try
            {
                using (var sw = new StreamWriter(path, true))
                    sw.WriteLine(line);
            }
            catch
            {

            }
        }


        private static void OverwriteFile(string path, string header, IEnumerable<string> dataLines)
        {
            EnsureFile(path, header);
            try
            {
                using (var sw = new StreamWriter(path, false))
                {
                    sw.WriteLine(header);
                    foreach (var l in dataLines)
                    {
                        if (!string.IsNullOrWhiteSpace(l))
                            sw.WriteLine(l);
                    }
                }
            }
            catch
            {

            }
        }

        public static int GenerateNextIdForFile(string path, string header, Func<string, int?> idExtractor)
        {
            EnsureFile(path, header);

            int max = 0;
            try
            {
                foreach (var line in ReadDataLines(path, header))
                {
                    try
                    {
                        int? id = idExtractor(line);
                        if (id.HasValue && id.Value > max) max = id.Value;
                    }
                    catch
                    {
                        
                    }
                }
            }
            catch
            {

            }
            return max + 1;
        }

        public static List<Product> LoadProducts()
        {
            var list = new List<Product>();
            foreach (var line in ReadDataLines(ProductsFile, ProductsHeader))
            {
                if (Product.TryFromCsv(line, out Product p))
                    list.Add(p);
                else
                    continue;
            }
            return list;
        }

        public static void SaveProducts(List<Product> products)
        {
            var lines = products.Select(p => p.ToCsv());
            OverwriteFile(ProductsFile, ProductsHeader, lines);
        }

        public static void AppendProduct(Product p)
        {
            AppendLine(ProductsFile, ProductsHeader, p.ToCsv());
        }

        public static int GetNextProductId()
        {
            return GenerateNextIdForFile(ProductsFile, ProductsHeader, line =>
            {
                var parts = line.Split(';');
                if (parts.Length == 0) return null;
                if (int.TryParse(parts[0], out int id)) return id;
                return null;
            });
        }


        public struct User
        {
            public int Id;
            public string Email;
            public string Login;
            public string PasswordHash;
            public DateTime Registered;

            public User(int id, string email, string login, string hash, DateTime reg)
            {
                Id = id;
                Email = email;
                Login = login;
                PasswordHash = hash;
                Registered = reg;
            }

            public string ToCsv()
            {
                return $"{Id};{Email};{Login};{PasswordHash};{Registered:o}";
            }

            public static bool TryFromCsv(string line, out User u)
            {
                u = default;

                if (string.IsNullOrWhiteSpace(line)) return false;

                var p = line.Split(';');
                if (p.Length < 5) return false;

                if (!int.TryParse(p[0], out int id)) return false;
                if (!DateTime.TryParse(p[4], null, DateTimeStyles.RoundtripKind, out DateTime reg)) return false;

                u = new User(id, p[1], p[2], p[3], reg);
                return true;
            }
        }

        public static string UsersFile = "users.csv";
        private static readonly string UsersHeader = "Id;Email;Login;PasswordHash;Registered";

        public static List<User> LoadUsers()
        {
            var list = new List<User>();
            foreach (var line in ReadDataLines(UsersFile, UsersHeader))
                if (User.TryFromCsv(line, out User u))
                    list.Add(u);

            return list;
        }

        public static void AppendUser(User u)
        {
            AppendLine(UsersFile, UsersHeader, u.ToCsv());
        }

        public static int GetNextUserId()
        {
            return GenerateNextIdForFile(UsersFile, UsersHeader, line =>
            {
                var p = line.Split(';');
                if (int.TryParse(p[0], out int id))
                    return id;

                return null;
            });
        }

        public static string Hash(string input)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

    }
}