using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using OOP15;
class Program
{
    static double CalculateA(double x, double y)
    {
        Console.WriteLine("Выполняется задача 1...");
        Task.Delay(1000).Wait();
        return x + y;
    }

    static double CalculateB(double x, double y)
    {
        Console.WriteLine("Выполняется задача 2...");
        Task.Delay(1500).Wait();
        return x * y;
    }

    static double CalculateC(double x, double y)
    {
        Console.WriteLine("Выполняется задача 3...");
        Task.Delay(2000).Wait();
        return x / y; 
    }
    static double FinalCalculation(double a, double b, double c)
    {
        Console.WriteLine("Выполняется четвертая задача...");
        Task.Delay(500).Wait();
        return a + b - c;
    }
    static void Eratosfen(CancellationToken token, int n = 10000)
    {
        bool[] isPrime = new bool[n + 1];
        for (int i = 2; i <= n; i++)
            isPrime[i] = true;

        for (int i = 2; i * i <= n; i++)
        {
            token.ThrowIfCancellationRequested();

            if (isPrime[i])
            {
                for (int j = i * i; j <= n; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        Console.WriteLine("Простые числа до " + n + ":");
        for (int i = 2; i <= n; i++)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("\nОперация прервана.");
                token.ThrowIfCancellationRequested();
            }

            if (isPrime[i])
                Console.Write(i + " ");
        }
        Console.WriteLine();
    }
    static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(10);
        int arrayCount = 5;
        int arraySize = 1000000;
        var timer = new Stopwatch();
        timer.Start();

        Task task1 = Task.Run(() => Eratosfen(cts.Token), cts.Token);

        try
        {
            task1.Wait();
        }
        catch (OperationCanceledException)
        { 
            Console.WriteLine("\nОперация была отменена.");
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
            {
                if (inner is OperationCanceledException)
                    Console.WriteLine("\nОперация была отменена (внутри AggregateException).");
                else
                    Console.WriteLine($"Произошла ошибка: {inner.Message}");
            }
        }
        finally
        {
            timer.Stop();
            Console.WriteLine($"Идентификатор задачи: {task1.Id}");
            Console.WriteLine($"Статус завершения: {task1.IsCompleted}");
            Console.WriteLine($"Потрачено времени: {timer.Elapsed}");
            cts.Dispose();
        }
        Console.WriteLine();
        Task<double> task = Task.Run(() => CalculateA(2, 3));
        Task<double> task2 = Task.Run(() => CalculateB(4, 5));
        Task<double> task3 = Task.Run(() => CalculateC(6, 7));

        Task.WaitAll(task, task2, task3);

        double result1 = task.Result;
        double result2 = task2.Result;
        double result3 = task3.Result;
        var taskAwaiter = task.GetAwaiter();
        Task<double> finalTask = Task.Run(() => FinalCalculation(result1, result2, result3));

        double finalResult = finalTask.Result;
        Task continuationTask = Task.WhenAll(task, task2, task3, finalTask)
            .ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Задача ContinueWith");
                }
            });
        taskAwaiter.OnCompleted(() =>
        {
            Console.WriteLine("Задача с GetAwaiter(), OnCompleted()");
        });
        continuationTask.Wait();
        Console.WriteLine($"Результат первой задачи: {result1}");
        Console.WriteLine($"Результат второй задачи: {result2}");
        Console.WriteLine($"Результат третьей задачи: {result3}");
        Console.WriteLine($"Результат четвертой задачи (итоговый): {finalResult}");
        Console.WriteLine();
        List<int[]> arrays = new List<int[]>(arrayCount);
        for (int i = 0; i < arrayCount; i++)
        {
            arrays.Add(new int[arraySize]);
        }

        Console.WriteLine("Генерация массивов с использованием Parallel.For...");
        Parallel.For(0, arrayCount, i =>
        {
            Random rand = new Random();
            for (int j = 0; j < arraySize; j++)
            {
                arrays[i][j] = rand.Next(1, 100);
            }
            Console.WriteLine($"Массив {i + 1} сгенерирован.");
        });

        Console.WriteLine("\nОбработка массивов с использованием Parallel.ForEach...");
        Parallel.ForEach(arrays, (array, state, index) =>
        {
            long sum = array.Sum(x => (long)x);
            Console.WriteLine($"Сумма элементов массива {index + 1}: {sum}");
        });
        Console.WriteLine();
        var cts2 = new CancellationTokenSource();
        Parallel.Invoke(
            () => Eratosfen(cts2.Token, 10),
            () => CalculateA(2, 2),
            () => CalculateB(1, 10)
        );
        Console.WriteLine();
        await Task.Run(async () => {
            Console.WriteLine("Начало задачи");
            await Task.Delay(1500);
            Console.WriteLine("Конец задачи");
        });
        Console.WriteLine();
        await Task7.init();
        Console.WriteLine("Программа завершена.");
    }
}