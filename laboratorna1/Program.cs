using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== СЕРВІС ПРОКАТУ АВТО ===\n");
        Console.ResetColor();

        double zhiguli = 700;
        double uaz = 1200;
        double buhanka = 1000;
        double corolla = 1800;

        Console.WriteLine("1. Жигулі – 700 грн/доба");
        Console.WriteLine("2. УАЗ – 1200 грн/доба");
        Console.WriteLine("3. Буханка – 1000 грн/доба");
        Console.WriteLine("4. Тойота – 1800 грн/доба\n");

        Console.Write("Введіть кількість днів оренди для Жигулі: ");
        int d1 = int.Parse(Console.ReadLine());

        Console.Write("Введіть кількість днів оренди для УАЗ: ");
        int d2 = int.Parse(Console.ReadLine());

        Console.Write("Введіть кількість днів оренди для Буханки: ");
        int d3 = int.Parse(Console.ReadLine());

        Console.Write("Введіть кількість днів оренди для Corolla: ");
        int d4 = int.Parse(Console.ReadLine());

        double s1 = zhiguli * d1;
        double s2 = uaz * d2;
        double s3 = buhanka * d3;
        double s4 = corolla * d4;

        double total = s1 + s2 + s3 + s4;

        Random rnd = new Random();
        double discount = rnd.Next(0, 11);
        double discountAmount = total * discount / 100;
        double finalTotal = total - discountAmount;

        finalTotal = Math.Round(finalTotal, 2);
        double sqrtBonus = Math.Sqrt(finalTotal);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n=== РЕЗУЛЬТАТИ ===");
        Console.ResetColor();

        Console.WriteLine($"Жигулі: {s1} грн");
        Console.WriteLine($"УАЗ: {s2} грн");
        Console.WriteLine($"Буханка: {s3} грн");
        Console.WriteLine($"Тойота: {s4} грн");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nЗагальна сума без знижки: {total} грн");
        Console.WriteLine($"Знижка: {discount}% (-{discountAmount} грн)");
        Console.WriteLine($"Підсумкова сума до оплати: {finalTotal} грн");
        Console.ResetColor();

        Console.WriteLine($"\n(√ із суми для перевірки: {Math.Round(sqrtBonus, 2)})");
        Console.WriteLine("\nДякуємо, що обрали наш гаражний сервіс прокату!");
    }
}