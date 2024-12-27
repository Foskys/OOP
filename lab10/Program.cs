using System;
using System.Collections.Generic;
using System.Linq;

class Product 
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; } 

    public override string ToString() => $"{Name} - {Price} - {Quantity} - {IsAvailable}";
}

class Program
{
    static void Main()
    {
        string[] months = { "June", "July", "May", "December", "January", "February", "March", "April", "August", "September", "October", "November" };

        int n = 4; 

        var lengthQuery = months.Where(m => m.Length == n);
        Console.WriteLine("Месяцы длиной строки равной 4:");
        Console.WriteLine(string.Join(", ", lengthQuery));

        var summerWinter = months.Where(m => new[] { "June", "July", "August", "December", "January", "February" }.Contains(m));
        Console.WriteLine("Летние и зимние месяцы:");
        Console.WriteLine(string.Join(", ", summerWinter));

        var sortedMonths = months.OrderBy(m => m);
        Console.WriteLine("Месяцы в алфавитном порядке:");
        Console.WriteLine(string.Join(", ", sortedMonths));

        var countQuery = months.Count(m => m.Contains('u') && m.Length >= 4);
        Console.WriteLine($"Количество месяцев с буквой 'u' и длиной не менее 4: {countQuery}");

        var products = new List<Product>
        {
            new Product { Name = "Milk", Price = 1.5, Quantity = 10, IsAvailable = true },
            new Product { Name = "Bread", Price = 1.0, Quantity = 20, IsAvailable = true },
            new Product { Name = "Cheese", Price = 3.5, Quantity = 5, IsAvailable = false },
            new Product { Name = "Butter", Price = 2.5, Quantity = 7, IsAvailable = true },
            new Product { Name = "Eggs", Price = 2.0, Quantity = 12, IsAvailable = true },
            new Product { Name = "Juice", Price = 1.8, Quantity = 8, IsAvailable = false },
            new Product { Name = "Chocolate", Price = 2.5, Quantity = 15, IsAvailable = true },
            new Product { Name = "Cereal", Price = 4.0, Quantity = 6, IsAvailable = true },
            new Product { Name = "Coffee", Price = 5.0, Quantity = 3, IsAvailable = true },
            new Product { Name = "Tea", Price = 1.5, Quantity = 25, IsAvailable = true },
        };

        var availableProducts = products.Where(p => p.IsAvailable && p.Price < 3.0).OrderBy(p => p.Name);
        Console.WriteLine("Доступные продукты с ценой менее 3.0:");
        foreach (var product in availableProducts)
        {
            Console.WriteLine(product);
        }

        var sets = new List<List<int>>
        {
            new List<int> { 1, 2, 3 },
            new List<int> { -1, -2, -3 },
            new List<int> { 5, 6, 7 },
            new List<int> { 0, 4, -8 },
            new List<int> { 9, 10, 11 },
        };

        var minSumSet = sets.OrderBy(s => s.Sum()).First();
        var maxSumSet = sets.OrderByDescending(s => s.Sum()).First();
        Console.WriteLine($"Множество с наименьшей суммой: {string.Join(", ", minSumSet)}");
        Console.WriteLine($"Множество с наибольшей суммой: {string.Join(", ", maxSumSet)}");

        var setsWithNegatives = sets.Where(s => s.Any(e => e < 0));
        Console.WriteLine("Множества с отрицательными элементами:");
        foreach (var set in setsWithNegatives)
        {
            Console.WriteLine(string.Join(", ", set));
        }

        int target = 3;
        var countContainingTarget = sets.Count(s => s.Contains(target));
        Console.WriteLine($"Количество множеств, содержащих {target}: {countContainingTarget}");

        var maxSet = sets.OrderByDescending(s => s.Max()).First();
        Console.WriteLine($"Множество с максимальным элементом: {string.Join(", ", maxSet)}");

        var firstWithTarget = sets.FirstOrDefault(s => s.Contains(target));
        Console.WriteLine($"Первое множество с элементом {target}: {string.Join(", ", firstWithTarget)}");

        var orderedSets = sets.OrderBy(s => s.First());
        Console.WriteLine("Упорядоченные множества по первому элементу:");
        foreach (var set in orderedSets)
        {
            Console.WriteLine(string.Join(", ", set));
        }

        var complexQuery = products
            .Where(p => p.Quantity > 5)
            .OrderByDescending(p => p.Price)
            .GroupBy(p => p.IsAvailable)
            .Select(g => new { Availability = g.Key, TotalPrice = g.Sum(p => p.Price) })
            .Any(g => g.TotalPrice > 20);

        Console.WriteLine($"Есть ли группа с суммой более 20: {complexQuery}");

        var categories = new List<(string ProductName, string Category)>
        {
            ("Milk", "Dairy"),
            ("Bread", "Bakery"),
            ("Cheese", "Dairy"),
            ("Juice", "Beverages"),
        };

        var joinedQuery = products
            .Join(categories, p => p.Name, c => c.ProductName, (p, c) => new { p.Name, p.Price, c.Category });

        Console.WriteLine("Продукты с категориями:");
        foreach (var item in joinedQuery)
        {
            Console.WriteLine($"{item.Name} - {item.Price} - {item.Category}");
        }
    }
}
