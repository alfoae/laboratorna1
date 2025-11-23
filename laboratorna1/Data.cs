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

        public override string ToString()
        {
            return $"{Id}. {Name} — {PricePerDay} грн/доба — в наявності: {AvailableCount} — {(IsActive ? "активний" : "неактивний")} — додано: {Added:yyyy-MM-dd}";
        }

        public string ToCsv()
        {
            return $"{Id};{Name};{PricePerDay.ToString(CultureInfo.InvariantCulture)};{AvailableCount};{IsActive};{Added:O}";
        }

        public static Product FromCsv(string line)
        {
            string[] x = line.Split(';');

            return new Product(
                int.Parse(x[0]),
                x[1],
                double.Parse(x[2], CultureInfo.InvariantCulture),
                int.Parse(x[3]),
                bool.Parse(x[4]),
                DateTime.Parse(x[5], null, DateTimeStyles.RoundtripKind)
            );
        }
    }

    public struct Client
    {
        public int ClientId;
        public string FullName;
        public string Phone;
        public string Email;
        public DateTime Registered;
        public bool IsVip;

        public Client(int id, string name, string phone, string email, DateTime reg, bool vip)
        {
            ClientId = id;
            FullName = name;
            Phone = phone;
            Email = email;
            Registered = reg;
            IsVip = vip;
        }
    }

    public struct Booking
    {
        public int BookingId;
        public int ClientId;
        public int ProductId;
        public int Days;
        public double TotalPrice;
        public DateTime BookingDate;

        public Booking(int bid, int cid, int pid, int days, double total, DateTime date)
        {
            BookingId = bid;
            ClientId = cid;
            ProductId = pid;
            Days = days;
            TotalPrice = total;
            BookingDate = date;
        }
    }

    // Д А Н І

    public static class DataManager
    {
        public static string FileName = "tovar.txt";

        public static void SaveProducts(List<Product> list)
        {
            using (StreamWriter sw = new StreamWriter(FileName, false))
            {
                foreach (var p in list)
                    sw.WriteLine(p.ToCsv());
            }
        }

        public static List<Product> LoadProducts()
        {
            List<Product> list = new List<Product>();

            if (!File.Exists(FileName))
                return list;

            foreach (string line in File.ReadAllLines(FileName))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    list.Add(Product.FromCsv(line));
                }
                catch
                {
                    continue;
                }
            }

            return list;
        }
    }
}
