using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegatesAndEvents
{
    public class User
    {
        public string Name { get; set; }
        public double Position { get; private set; }
        public double Size { get; private set; } = 1.0;

        public User(string name)
        {
            Name = name;
        }

        public event Action<double> OnMove;

        public event Action<double> OnCompress;

        public void Move(double offset)
        {
            OnMove?.Invoke(offset);
        }

        public void Compress(double factor)
        {
            OnCompress?.Invoke(factor);
        }

        public void ApplyMove(double offset)
        {
            Position += offset;
            Console.WriteLine($"Пользователь {Name} перемещён на {offset}. Новая позиция: {Position}");
        }

        public void ApplyCompress(double factor)
        {
            Size *= factor;
            Console.WriteLine($"Пользователь {Name} сжат с коэффициентом {factor}. Новый размер: {Size}");
        }
    }

    class Program
    {
        static string RemovePunctuation(string input) => new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
        static string AddSymbols(string input) => input + " ###";
        static string ToUpperCase(string input) => input.ToUpper();
        static string RemoveExtraSpaces(string input) => string.Join(" ", input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        static string ReplaceSpacesWithUnderscores(string input) => input.Replace(" ", "_");

        static void Main(string[] args)
        {
            string input = "  Пример: строки, с   пробелами и символами!  ";
            List<Func<string, string>> processors = new List<Func<string, string>>
            {
                RemovePunctuation,
                AddSymbols,
                ToUpperCase,
                RemoveExtraSpaces,
                ReplaceSpacesWithUnderscores
            };

            string result = processors.Aggregate(input, (current, processor) => processor(current));

            Console.WriteLine("Результат обработки строки: " + result);

            User user1 = new User("Иван");
            User user2 = new User("Анна");
            User user3 = new User("Олег");

            user1.OnMove += user1.ApplyMove;
            user1.OnCompress += user1.ApplyCompress;

            user2.OnMove += user2.ApplyMove;

            user3.OnCompress += user3.ApplyCompress;

            Console.WriteLine("\n-- События --");

            user1.Move(10);
            user1.Compress(0.8);

            user2.Move(-5);

            user3.Compress(0.5);
        }
    }
}




public delegate void MyDelegate(string message); 

class Program
{
    static void Main()
    {
        MyDelegate del = PrintMessage; 
        del("Привет, мир!");          
    }

    static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
}

