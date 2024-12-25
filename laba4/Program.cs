using System;

public interface IVehicle
{
    void Move();
    void Stop();
    string GetInfo();
}

public abstract class Vehicle : IVehicle
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

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != this.GetType())
            return false;
        Vehicle other = (Vehicle)obj;
        return Name == other.Name && Speed == other.Speed;
    }

    public override int GetHashCode()
    {
        return (Name, Speed).GetHashCode();
    }

    public override string ToString()
    {
        return $"Транспортное средство: {Name}, Скорость: {Speed} км/ч";
    }

    public virtual object Clone()
    {
        return this.MemberwiseClone();
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
        return $"{Name} — это парусник, использующий ветер для передвижения, со скоростью {Speed} км/ч.";
    }

    public override string ToString()
    {
        return $"Парусник: {Name}, Скорость: {Speed} км/ч";
    }
}

public sealed class Corvette : Ship  // Sealed class
{
    public Corvette(string name, int speed) : base(name, speed) { }

    public override string GetInfo()
    {
        return $"{Name} — это корвет, быстрое военное судно, движущееся со скоростью {Speed} км/ч.";
    }

 
    public override string ToString()
    {
        return $"Корвет: {Name}, Скорость: {Speed} км/ч";
    }
}

public class Captain : IVehicle
{
    public string Name { get; set; }
    public int Experience { get; set; }

    public Captain(string name, int experience)
    {
        Name = name;
        Experience = experience;
    }

    public void Move()
    {
        Console.WriteLine($"{Name} управляет судном.");
    }

    public void Stop()
    {
        Console.WriteLine($"{Name} остановил судно.");
    }

    public string GetInfo()
    {
        return $"{Name} — капитан с {Experience} летним опытом.";
    }

    public override string ToString()
    {
        return $"Капитан: {Name}, Опыт: {Experience} лет.";
    }
}

public class Boat : Vehicle
{
    public Boat(string name, int speed) : base(name, speed) { }

    public override string GetInfo()
    {
        return $"{Name} — это лодка, движущаяся со скоростью {Speed} км/ч.";
    }


    public override string ToString()
    {
        return $"Лодка: {Name}, Скорость: {Speed} км/ч";
    }
}

public class Printer
{
    public void Print(IVehicle vehicle)
    {
        Console.WriteLine(vehicle.ToString());
        Console.WriteLine(vehicle.GetInfo());
    }
}

public class Program
{
    public static void Main(string[] args)
    {
   
        Vehicle ship = new Ship("Корабль", 40);
        Vehicle steamboat = new Steamboat("Пароход", 29);
        Vehicle sailboat = new Sailboat("Парусник", 17);
        Vehicle corvette = new Corvette("Корвет", 50);
        IVehicle captain = new Captain("Капитан Иван", 5);
        Vehicle boat = new Boat("Лодка", 8);

        IVehicle[] vehicles = new IVehicle[] { ship, steamboat, sailboat, corvette, captain, boat };

        Printer printer = new Printer();

        foreach (var vehicle in vehicles)
        {
            printer.Print(vehicle);
        }
    }
}
