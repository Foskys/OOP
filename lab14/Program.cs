using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MultiTaskApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nВыберите задание (1-5) или введите 0 для выхода: ");
                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Ошибка: Введите корректное число.");
                    continue;
                }

                if (choice == 0)
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                switch (choice)
                {
                    case 1:
                        ListProcesses();
                        break;
                    case 2:
                        ManageAppDomain();
                        break;
                    case 3:
                        RunPrimeNumbersTask();
                        break;
                    case 4:
                        RunEvenOddThreads();
                        break;
                    case 5:
                        TimerTask();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Пожалуйста, выберите число от 1 до 5.");
                        break;
                }
            }
        }

        // ЗАДАНИЕ 1: Вывод запущенных процессов (обновленный)
        static void ListProcesses()
        {
            try
            {
                var processes = Process.GetProcesses();
                using (StreamWriter writer = new StreamWriter("processes.txt"))
                {
                    string header = string.Format("{0,-10} {1,-30} {2,-15} {3,-25} {4,-20} {5,-20}",
                        "ID", "Имя процесса", "Приоритет", "Время запуска", "Состояние", "Время CPU");
                    Console.WriteLine(header);
                    Console.WriteLine(new string('-', header.Length));
                    writer.WriteLine(header);
                    writer.WriteLine(new string('-', header.Length));

                    foreach (var process in processes)
                    {
                        try
                        {
                            string startTime = "Нет доступа"; 
                            string cpuTime = "Нет доступа"; 
                            string state = process.Responding ? "Работает" : "Не отвечает";

                            try
                            {
                                startTime = process.StartTime.ToString();
                            }
                            catch { }

                            try
                            {
                                cpuTime = process.TotalProcessorTime.ToString();
                            }
                            catch { }

                            string processInfo = string.Format("{0,-10} {1,-30} {2,-15} {3,-25} {4,-20} {5,-20}",
                                process.Id, process.ProcessName, process.BasePriority, startTime, state, cpuTime);
                            Console.WriteLine(processInfo);
                            writer.WriteLine(processInfo);
                        }
                        catch
                        {

                        }
                    }
                }
                Console.WriteLine("Информация о процессах записана в файл processes.txt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке процессов: {ex.Message}");
            }
        }

        // ЗАДАНИЕ 2: Работа с доменом приложения
        static void ManageAppDomain()
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                Console.WriteLine($"Имя домена: {currentDomain.FriendlyName}");
                Console.WriteLine("Загруженные сборки:");
                foreach (var assembly in currentDomain.GetAssemblies())
                {
                    Console.WriteLine(assembly.FullName);
                }

                AppDomain newDomain = AppDomain.CreateDomain("NewDomain");
                Console.WriteLine($"Создан новый домен: {newDomain.FriendlyName}");

                newDomain.Load(Assembly.GetExecutingAssembly().FullName);
                Console.WriteLine("Сборка загружена в новый домен.");

                AppDomain.Unload(newDomain);
                Console.WriteLine("Новый домен выгружен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка работы с доменом приложения: {ex.Message}");
            }
        }

        // ЗАДАНИЕ 3: Расчет простых чисел
        static void RunPrimeNumbersTask()
        {
            Console.WriteLine("Введите n:");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Ошибка: Введите положительное число.");
                return;
            }

            Thread primeThread = new Thread(() =>
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter("primes.txt"))
                    {
                        for (int i = 1; i <= n; i++)
                        {
                            if (IsPrime(i))
                            {
                                Console.WriteLine(i);
                                writer.WriteLine(i);
                                Thread.Sleep(100); 
                            }
                        }
                    }
                    Console.WriteLine("Простые числа записаны в файл primes.txt.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в потоке расчета простых чисел: {ex.Message}");
                }
            })
            {
                Name = "PrimeThread",
                Priority = ThreadPriority.Normal
            };

            primeThread.Start();

            while (primeThread.IsAlive)
            {
                Console.WriteLine($"Статус потока: {primeThread.ThreadState}, Приоритет: {primeThread.Priority}");
                Thread.Sleep(500);
            }
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        // ЗАДАНИЕ 4: Четные и нечетные числа
        static void RunEvenOddThreads()
        {
            Console.WriteLine("Выберите режим:");
            Console.WriteLine("1 - Одновременный вывод");
            Console.WriteLine("2 - Сначала четные, потом нечетные");

            if (!int.TryParse(Console.ReadLine(), out int mode) || (mode != 1 && mode != 2))
            {
                Console.WriteLine("Ошибка: Выберите 1 или 2.");
                return;
            }

            Console.WriteLine("Введите n:");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Ошибка: Введите положительное число.");
                return;
            }

            object lockObj = new object();

            if (mode == 1)
            {
                Thread evenThread = new Thread(() =>
                {
                    for (int i = 0; i <= n; i += 2)
                    {
                        lock (lockObj)
                        {
                            Console.WriteLine($"Четное: {i}");
                            File.AppendAllText("numbers.txt", $"Четное: {i}\n");
                            Thread.Sleep(200);
                        }
                    }
                });

                Thread oddThread = new Thread(() =>
                {
                    for (int i = 1; i <= n; i += 2)
                    {
                        lock (lockObj)
                        {
                            Console.WriteLine($"Нечетное: {i}");
                            File.AppendAllText("numbers.txt", $"Нечетное: {i}\n");
                            Thread.Sleep(300);
                        }
                    }
                });

                evenThread.Priority = ThreadPriority.Highest;
                evenThread.Start();
                oddThread.Start();

                evenThread.Join();
                oddThread.Join();
            }
            else if (mode == 2)
            {
                Thread evenThread = new Thread(() =>
                {
                    for (int i = 0; i <= n; i += 2)
                    {
                        lock (lockObj)
                        {
                            Console.WriteLine($"Четное: {i}");
                            File.AppendAllText("numbers.txt", $"Четное: {i}\n");
                            Thread.Sleep(200); 
                        }
                    }
                });

                Thread oddThread = new Thread(() =>
                {
                    evenThread.Join();
                    for (int i = 1; i <= n; i += 2)
                    {
                        lock (lockObj)
                        {
                            Console.WriteLine($"Нечетное: {i}");
                            File.AppendAllText("numbers.txt", $"Нечетное: {i}\n");
                            Thread.Sleep(300); 
                        }
                    }
                });

                evenThread.Start();
                oddThread.Start();

                evenThread.Join();
                oddThread.Join();
            }


            Console.WriteLine("Четные и нечетные числа записаны в файл numbers.txt.");
        }
        // ЗАДАНИЕ 5: Повторяющаяся задача с использованием Timer
        static void TimerTask()
        {
            try
            {
                Timer timer = new Timer(PrintCurrentTime, null, 0, 1000);
                Console.WriteLine("Нажмите Enter для завершения...");
                Console.ReadLine();
                timer.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с таймером: {ex.Message}");
            }
        }

        static void PrintCurrentTime(object state)
        {
            Console.WriteLine($"Текущее время: {DateTime.Now}");
        }
    }
}
