using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;

public interface IOperations<T>
{
    void Add(T item);
    void Remove(T item);
    IEnumerable<T> View(Func<T, bool> predicate);
}

public class CollectionType<T> : IOperations<T> where T : Vehicle
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public void Remove(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (!_items.Remove(item))
        {
            throw new ArgumentException("Элемент не найден в коллекции.");
        }
    }

    public IEnumerable<T> View(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return _items.Where(predicate);
    }


    public void SaveToFile(string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Отключаем Unicode-escaping
                Converters = { new VehicleJsonConverter() }
            };

            var json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция сохранения завершена.");
        }
    }
    public void LoadFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new VehicleJsonConverter() }
            };

            var json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new JsonException("Файл пуст или содержит недопустимый JSON.");
            }

            var items = JsonSerializer.Deserialize<List<T>>(json, options);
            if (items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка при обработке JSON: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("Операция загрузки завершена.");
        }
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, _items);
    }
}

public abstract class Vehicle
{
    public string Name { get; set; }
    public int Speed { get; set; }

    public Vehicle(string name, int speed)
    {
        Name = name;
        Speed = speed;
    }

    public virtual void Move()
    {
        Console.WriteLine($"{Name} движется со скоростью {Speed} км/ч.");
    }

    public virtual void Stop()
    {
        Console.WriteLine($"{Name} остановился.");
    }

    public abstract string GetInfo();

    public override string ToString()
    {
        return $"Транспортное средство: {Name}, Скорость: {Speed} км/ч";
    }
}

public class Ship : Vehicle
{
    public Ship(string name, int speed) : base(name, speed) { }

    public override string GetInfo()
    {
        return $"{Name} — это корабль, движущийся со скоростью {Speed} км/ч.";
    }

    public override string ToString()
    {
        return $"Корабль: {Name}, Скорость: {Speed} км/ч";
    }
}

public class Steamboat : Ship
{
    public Steamboat(string name, int speed) : base(name, speed) { }

    public override string GetInfo()
    {
        return $"{Name} — это пароход, движущийся со скоростью {Speed} км/ч.";
    }

    public override string ToString()
    {
        return $"Пароход: {Name}, Скорость: {Speed} км/ч";
    }
}

public class Sailboat : Ship
{
    public Sailboat(string name, int speed) : base(name, speed) { }

    public override string GetInfo()
    {
        return $"{Name} — это парусник, движущийся со скоростью {Speed} км/ч.";
    }

    public override string ToString()
    {
        return $"Парусник: {Name}, Скорость: {Speed} км/ч";
    }
}

public class VehicleJsonConverter : JsonConverter<Vehicle>
{
    public override Vehicle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Ожидался объект JSON.");
        }

        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("Type", out var typeProperty))
            {
                throw new JsonException("Отсутствует поле 'Type'.");
            }

            var type = typeProperty.GetString();
            if (string.IsNullOrEmpty(type))
            {
                throw new JsonException("Поле 'Type' пустое.");
            }

            return type switch
            {
                "Ship" => JsonSerializer.Deserialize<Ship>(root.GetRawText(), options),
                "Steamboat" => JsonSerializer.Deserialize<Steamboat>(root.GetRawText(), options),
                "Sailboat" => JsonSerializer.Deserialize<Sailboat>(root.GetRawText(), options),
                _ => throw new NotSupportedException($"Неизвестный тип транспорта: {type}")
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("Type", value.GetType().Name);

        foreach (var property in value.GetType().GetProperties())
        {
            writer.WritePropertyName(property.Name);
            JsonSerializer.Serialize(writer, property.GetValue(value), options);
        }

        writer.WriteEndObject();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var collection = new CollectionType<Vehicle>();

        collection.Add(new Ship("Титаник", 30));
        collection.Add(new Steamboat("Пароход", 20));
        collection.Add(new Sailboat("Яхта", 15));

        string filePath = "7lab.json";

        collection.SaveToFile(filePath);

        var loadedCollection = new CollectionType<Vehicle>();
        loadedCollection.LoadFromFile(filePath);
        Console.WriteLine(loadedCollection);
    }
}
