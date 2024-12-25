using System;
using System.Collections.Generic;
using System.Linq;

// Перечисление для типа транспорта
public enum TransportType
{
    Ship,
    Boat,
    Sailboat,
    Steamboat,
    Corvette
}

// Структура для параметров судна
public struct VesselParameters
{
    public int Displacement; // Водоизмещение
    public int Capacity; // Количество посадочных мест

    public VesselParameters(int displacement, int capacity)
    {
        Displacement = displacement;
        Capacity = capacity;
    }

    public override string ToString()
    {
        return $"Водоизмещение: {Displacement} тонн, Посадочных мест: {Capacity}";
    }
}

// Интерфейс IVehicle
public interface IVehicle
{
    void Move();
    void Stop();
    string GetInfo();
}

// Partial класс Vehicle, первый файл
public abstract partial class Vehicle : IVehicle
{
    public TransportType Type { get; set; } // Тип транспорта
    public VesselParameters Parameters { get; set; } // Параметры судна

    public Vehicle(string name, int speed, TransportType type, VesselParameters parameters)
    {
        Name = name;
        Speed = speed;
        Type = type;
        Parameters = parameters;
    }
}

// Partial класс Vehicle, второй файл
public abstract partial class Vehicle
{
    public string Name { get; set; }
    public int Speed { get; set; }

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
        return $"Транспортное средство: {Name}, Скорость: {Speed} км/ч, Тип: {Type}, Параметры: {Parameters}";
    }
}

// Классы транспорта
public class Ship : Vehicle
{
    public Captain AssignedCaptain { get; set; } // Назначенный капитан

    public Ship(string name, int speed, TransportType type, VesselParameters parameters)
        : base(name, speed, type, parameters)
    {
    }

    public override string GetInfo()
    {
        return $"{Name} — это корабль, движущийся со скоростью {Speed} км/ч.";
    }
}

public class Steamboat : Ship
{
    public Steamboat(string name, int speed, TransportType type, VesselParameters parameters)
        : base(name, speed, type, parameters)
    {
    }

    public override string GetInfo()
    {
        return $"{Name} — это пароход, движущийся со скоростью {Speed} км/ч.";
    }
}

public class Sailboat : Ship
{
    public Sailboat(string name, int speed, TransportType type, VesselParameters parameters)
        : base(name, speed, type, parameters)
    {
    }

    public override string GetInfo()
    {
        return $"{Name} — это парусник, использующий ветер для передвижения, со скоростью {Speed} км/ч.";
    }
}

public sealed class Corvette : Ship
{
    public Corvette(string name, int speed, TransportType type, VesselParameters parameters)
        : base(name, speed, type, parameters)
    {
    }

    public override string GetInfo()
    {
        return $"{Name} — это корвет, быстрое военное судно, движущееся со скоростью {Speed} км/ч.";
    }
}

public class Boat : Vehicle
{
    public Boat(string name, int speed, TransportType type, VesselParameters parameters)
        : base(name, speed, type, parameters)
    {
    }

    public override string GetInfo()
    {
        return $"{Name} — это лодка, движущаяся со скоростью {Speed} км/ч.";
    }
}

// Класс Captain
public class Captain
{
    public string Name { get; set; }
    public int Experience { get; set; }

    public Captain(string name, int experience)
    {
        Name = name;
        Experience = experience;
    }

    public override string ToString()
    {
        return $"Капитан: {Name}, Опыт: {Experience} лет.";
    }
}

// Контейнер для хранения объектов
public class PortContainer<T> where T : IVehicle
{
    private List<T> vehicles = new List<T>();

    public void Add(T vehicle) => vehicles.Add(vehicle);

    public void Remove(T vehicle) => vehicles.Remove(vehicle);

    public T Get(int index) => vehicles[index];

    public void Set(int index, T vehicle) => vehicles[index] = vehicle;

    public void DisplayAll()
    {
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine(vehicle);
        }
    }

    public List<T> GetAll() => vehicles;
}

// Контроллер
public class PortController
{
    private PortContainer<IVehicle> container;

    public PortController(PortContainer<IVehicle> container)
    {
        this.container = container;
    }

    // Найти среднее водоизмещение всех парусников
    public double GetAverageDisplacementOfSailboats()
    {
        var sailboats = container.GetAll().OfType<Sailboat>();
        return sailboats.Any() ? sailboats.Average(s => s.Parameters.Displacement) : 0;
    }

    // Найти среднее количество посадочных мест на пароходах
    public double GetAverageCapacityOfSteamboats()
    {
        var steamboats = container.GetAll().OfType<Steamboat>();
        return steamboats.Any() ? steamboats.Average(s => s.Parameters.Capacity) : 0;
    }

    // Все транспортные средства, на которых плавают капитаны моложе 35 лет
    public List<IVehicle> GetVehiclesWithYoungCaptains()
    {
        return container.GetAll().Where(v => (v as Ship)?.AssignedCaptain?.Experience < 35).ToList();
    }
}

// Главная программа
public class Program
{
    public static void Main(string[] args)
    {
        var port = new PortContainer<IVehicle>();

        port.Add(new Ship("Корабль", 40, TransportType.Ship, new VesselParameters(5000, 200))
        {
            AssignedCaptain = new Captain("Капитан Иван", 5)
        });
        port.Add(new Steamboat("Пароход", 29, TransportType.Steamboat, new VesselParameters(3000, 150))
        {
            AssignedCaptain = new Captain("Капитан Сергей", 10)
        });
        port.Add(new Sailboat("Парусник", 17, TransportType.Sailboat, new VesselParameters(2000, 10)));
        port.Add(new Corvette("Корвет", 50, TransportType.Corvette, new VesselParameters(4000, 50)));
        port.Add(new Boat("Лодка", 8, TransportType.Boat, new VesselParameters(500, 2)));

        var controller = new PortController(port);

        Console.WriteLine("Все транспортные средства в порту:");
        port.DisplayAll();

        Console.WriteLine($"\nСреднее водоизмещение парусников: {controller.GetAverageDisplacementOfSailboats()} тонн");
        Console.WriteLine($"Среднее количество посадочных мест на пароходах: {controller.GetAverageCapacityOfSteamboats()} мест");

        Console.WriteLine("\nТранспортные средства, на которых плавают капитаны моложе 35 лет:");
        var youngCaptainVehicles = controller.GetVehiclesWithYoungCaptains();
        foreach (var vehicle in youngCaptainVehicles)
        {
            Console.WriteLine(vehicle);
        }
    }
}
