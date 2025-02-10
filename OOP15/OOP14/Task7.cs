using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OOP15
{
    public class Task7
    {
        static async Task Supplier(BlockingCollection<string> warehouse, string product, int supplierId, CancellationToken token)
        {
            Random random = new Random();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (!warehouse.TryAdd(product))
                    {
                        Console.WriteLine($"Поставщик {supplierId} не может добавить товар, склад полон.");
                    }
                    else
                    {
                        Console.WriteLine($"Поставщик {supplierId} добавил товар: {product}");
                    }

                    await Task.Delay(random.Next(500, 1000), token);
                    PrintWarehouseStatus(warehouse);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Поставщик {supplierId} завершил работу.");
            }
        }

        static async Task Customer(BlockingCollection<string> warehouse, int customerId, CancellationToken token)
        {
            Random random = new Random();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (warehouse.Count == 0)
                    {
                        Console.WriteLine($"Покупатель {customerId} пришел, но товара нет на складе. Он уходит.");
                    }
                    else
                    {
                        string purchasedProduct = warehouse.Take();
                        Console.WriteLine($"Покупатель {customerId} купил товар: {purchasedProduct}");
                    }

                    await Task.Delay(random.Next(300, 700), token);
                    PrintWarehouseStatus(warehouse);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Покупатель {customerId} завершил работу.");
            }
        }

        static void PrintWarehouseStatus(BlockingCollection<string> warehouse)
        {
            Console.WriteLine($"Товары на складе ({warehouse.Count}): {string.Join(", ", warehouse)}");
        }

        public static async Task init()
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            BlockingCollection<string> warehouse = new BlockingCollection<string>(10);
            string[] products = { "Холодильник", "Стиральная машина", "Микроволновка", "Пылесос", "Телевизор" };

            var suppliers = new Task[5];
            for (int i = 0; i < 5; i++)
            {
                int index = i;
                suppliers[i] = Task.Run(() => Supplier(warehouse, products[index], index + 1, token));
            }

            var customers = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                customers[i] = Task.Run(() => Customer(warehouse, i + 1, token));
            }

            try
            {
                await Task.Delay(10000);
                cts.Cancel();
                await Task.WhenAll(suppliers);
                await Task.WhenAll(customers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine("Завершено выполнение программы.");
        }
    }
}
