using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

public static class Reflector
{
    public static string GetAssemblyName(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        return type.Assembly.FullName;
    }
    public static bool HasPublicConstructors(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
    }

    public static IEnumerable<string> GetPublicMethods(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                   .Select(m => m.Name);
    }

    public static IEnumerable<string> GetFieldsAndProperties(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                         .Select(f => f.Name);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Select(p => p.Name);
        return fields.Concat(properties);
    }

    public static IEnumerable<string> GetImplementedInterfaces(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        return type.GetInterfaces().Select(i => i.Name);
    }

    public static IEnumerable<string> GetMethodsByParameterType(string className, string parameterType)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            throw new ArgumentException("Класс не найден.");
        }
        Type paramType = Type.GetType(parameterType);
        if (paramType == null)
        {
            throw new ArgumentException("Тип параметра не найден.");
        }
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                   .Where(m => m.GetParameters().Any(p => p.ParameterType == paramType))
                   .Select(m => m.Name);
    }

    public static object InvokeMethodFromFile(string filePath, object obj, string methodName)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Файл {filePath} не найден. Создаём пример файла...");
            CreateExampleFile(filePath, obj.GetType(), methodName);
            throw new FileNotFoundException($"Файл {filePath} создан. Пожалуйста, заполните его параметрами и перезапустите программу.");
        }

        string json = File.ReadAllText(filePath);
        object[] parameters;

        try
        {
            parameters = JsonConvert.DeserializeObject<object[]>(json);
        }
        catch (JsonReaderException)
        {
            Console.WriteLine($"Некорректный JSON в файле {filePath}. Пересоздаём файл с примером...");
            CreateExampleFile(filePath, obj.GetType(), methodName);
            throw new JsonReaderException($"Файл {filePath} содержал некорректные данные и был пересоздан. Пожалуйста, заполните его корректными параметрами.");
        }

        MethodInfo method = obj.GetType().GetMethod(methodName);
        if (method == null)
            throw new ArgumentException($"Метод {methodName} не найден.");

        var methodParameters = method.GetParameters();

        if (methodParameters.Length == 0)
        {
            if (parameters != null && parameters.Length > 0)
            {
                Console.WriteLine($"Метод {methodName} не принимает параметров. Загруженные параметры из файла будут проигнорированы.");
            }
            parameters = Array.Empty<object>();
        }
        else if (methodParameters.Length != parameters.Length)
        {
            throw new ArgumentException($"Количество параметров метода '{methodName}' не совпадает с переданными параметрами из файла.");
        }

        return method.Invoke(obj, parameters);
    }

    private static void CreateExampleFile(string filePath, Type type, string methodName)
    {
        var method = type.GetMethod(methodName);
        if (method == null)
            throw new ArgumentException($"Метод {methodName} не найден.");

        var exampleParams = method.GetParameters()
                                  .Select(p => Activator.CreateInstance(p.ParameterType) ?? "null")
                                  .ToArray();

        string json = JsonConvert.SerializeObject(exampleParams, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Console.WriteLine($"Пример файла для метода {methodName} создан: {filePath}");
    }

    public static void SaveToFile(string fileName, object data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(fileName, json);
    }

    public static T Create<T>()
    {
        Type type = typeof(T);
        ConstructorInfo constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
        if (constructor == null)
        {
            throw new InvalidOperationException("Не найден параметризованный конструктор без параметров.");
        }
        return (T)constructor.Invoke(null);
    }
}

public class Plant
{
    public string Name { get; set; }
    public string Type { get; set; }

    public Plant() { }

    public Plant(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public void Grow() => Console.WriteLine($"{Name} растет.");
}

public class Product
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Product() { }

    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public void DisplayInfo() => Console.WriteLine($"Продукт: {Name}, Цена: {Price}");
}

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Исследование класса Plant:");
            string plantClassName = typeof(Plant).FullName;

            Console.WriteLine("Сборка: " + Reflector.GetAssemblyName(plantClassName));
            Console.WriteLine("Есть публичные конструкторы: " + Reflector.HasPublicConstructors(plantClassName));
            Console.WriteLine("Публичные методы: " + string.Join(", ", Reflector.GetPublicMethods(plantClassName)));
            Console.WriteLine("Поля и свойства: " + string.Join(", ", Reflector.GetFieldsAndProperties(plantClassName)));
            Console.WriteLine("Реализованные интерфейсы: " + string.Join(", ", Reflector.GetImplementedInterfaces(plantClassName)));

            var methodsWithStringParam = Reflector.GetMethodsByParameterType(plantClassName, "System.String");
            Console.WriteLine("Методы с параметром типа string: " + string.Join(", ", methodsWithStringParam));

            var plant = Reflector.Create<Plant>();
            Reflector.InvokeMethodFromFile("plant_parameters.json", plant, "Grow");

            Console.WriteLine("\nИсследование класса Product:");
            string productClassName = typeof(Product).FullName;

            Console.WriteLine("Сборка: " + Reflector.GetAssemblyName(productClassName));
            Console.WriteLine("Есть публичные конструкторы: " + Reflector.HasPublicConstructors(productClassName));
            Console.WriteLine("Публичные методы: " + string.Join(", ", Reflector.GetPublicMethods(productClassName)));
            Console.WriteLine("Поля и свойства: " + string.Join(", ", Reflector.GetFieldsAndProperties(productClassName)));
            Console.WriteLine("Реализованные интерфейсы: " + string.Join(", ", Reflector.GetImplementedInterfaces(productClassName)));

            var product = Reflector.Create<Product>();
            Reflector.InvokeMethodFromFile("product_parameters.json", product, "DisplayInfo");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
