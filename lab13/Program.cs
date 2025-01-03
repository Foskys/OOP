using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

public interface ISerializer
{
    void Serialize<T>(T obj, string path);
    T Deserialize<T>(string path);
}

public class JsonCustomSerializer : ISerializer
{
    public void Serialize<T>(T obj, string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(path, json);
    }

    public T Deserialize<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException("Deserialization returned null");
    }
}

public class XmlCustomSerializer : ISerializer
{
    public void Serialize<T>(T obj, string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(fs, obj);
        }
    }

    public T Deserialize<T>(string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            return (T)(serializer.Deserialize(fs) ?? throw new InvalidOperationException("Deserialization returned null"));
        }
    }
}

public class Engine
{
    public int HorsePower { get; set; }

    public Engine() { }

    public Engine(int horsePower)
    {
        HorsePower = horsePower;
    }

    public override string ToString()
    {
        return $"{HorsePower} лошадок";
    }
}

public sealed class Car
{
    public string Brand { get; set; } = string.Empty; // Инициализация по умолчанию
    public string Model { get; set; } = string.Empty; // Инициализация по умолчанию
    public Engine Engine { get; set; } = new Engine(); // Инициализация по умолчанию

    [JsonIgnore]
    [XmlIgnore]
    public CarControl? Control { get; set; } // Свойство допускает значение null

    public Car() { }

    public Car(string brand, string model, Engine engine)
    {
        Brand = brand;
        Model = model;
        Engine = engine;
        Control = new CarControl();
    }

    public override string ToString()
    {
        string controlStatus = Control != null ? Control.ToString() : "Control не сериализован";
        return $"{Brand} {Model}, {Engine}, {controlStatus}";
    }
}

public class CarControl
{
    public void Accelerate()
    {
        Console.WriteLine("Ускоряемся");
    }

    public void Brake()
    {
        Console.WriteLine("Тормозим");
    }

    public override string ToString()
    {
        return "Контроль управления";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var car = new Car("Toyota", "Corolla", new Engine(150));
        var cars = new List<Car>
        {
            car,
            new Car("Honda", "Civic", new Engine(140)),
            new Car("Ford", "Focus", new Engine(160))
        };

        var jsonSerializer = new JsonCustomSerializer();
        var xmlSerializer = new XmlCustomSerializer();

        string jsonPath = "car.json";
        string xmlPath = "cars.xml";

        // JSON сериализация
        jsonSerializer.Serialize(car, jsonPath);
        var deserializedJsonCar = jsonSerializer.Deserialize<Car>(jsonPath);
        Console.WriteLine("JSON десериализация:");
        Console.WriteLine(deserializedJsonCar);

        // XML сериализация
        xmlSerializer.Serialize(cars, xmlPath);
        var deserializedXmlCars = xmlSerializer.Deserialize<List<Car>>(xmlPath);
        Console.WriteLine("XML десериализация:");
        foreach (var c in deserializedXmlCars)
        {
            Console.WriteLine(c);
        }
    }
}
