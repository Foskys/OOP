using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Plant
{
    public string Name { get; set; }
    public string Species { get; set; }
    public double Height { get; set; }

    public Plant(string name, string species, double height)
    {
        Name = name;
        Species = species;
        Height = height;
    }

    public override bool Equals(object obj)
    {
        if (obj is Plant other)
        {
            return Name == other.Name && Species == other.Species && Height == other.Height;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Species, Height);
    }

    public override string ToString()
    {
        return $"{Name} ({Species}), Высота: {Height}м";
    }
}

public class PlantCollection : IList<Plant>
{
    private HashSet<Plant> plants = new HashSet<Plant>();

    public Plant this[int index]
    {
        get => plants.ElementAt(index);
        set
        {
            var item = plants.ElementAt(index);
            plants.Remove(item);
            plants.Add(value);
        }
    }

    public int Count => plants.Count;

    public bool IsReadOnly => false;

    public void Add(Plant item) => plants.Add(item);

    public void Clear() => plants.Clear();

    public bool Contains(Plant item) => plants.Contains(item);

    public void CopyTo(Plant[] array, int arrayIndex) => plants.ToArray().CopyTo(array, arrayIndex);

    public IEnumerator<Plant> GetEnumerator() => plants.GetEnumerator();

    public int IndexOf(Plant item)
    {
        return plants.ToList().IndexOf(item);
    }

    public void Insert(int index, Plant item)
    {
        if (!plants.Contains(item))
        {
            var tempList = plants.ToList();
            tempList.Insert(index, item);
            plants = new HashSet<Plant>(tempList);
        }
    }

    public bool Remove(Plant item) => plants.Remove(item);

    public void RemoveAt(int index)
    {
        var item = plants.ElementAt(index);
        plants.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator() => plants.GetEnumerator();
}

class Program
{
    static void Main()
    {
        // Задание 1
        var plantCollection = new PlantCollection();

        plantCollection.Add(new Plant("Роза", "Rosa", 0.5));
        plantCollection.Add(new Plant("Подсолнух", "Helianthus", 1.8));
        plantCollection.Add(new Plant("Дуб", "Quercus", 20.0));

        Console.WriteLine("1. Растения в коллекции:");
        foreach (var plant in plantCollection)
        {
            Console.WriteLine(plant);
        }

        var sunflower = new Plant("Подсолнух", "Helianthus", 1.8);
        Console.WriteLine($"\nСодержит Подсолнух: {plantCollection.Contains(sunflower)}");

        plantCollection.Remove(sunflower);
        Console.WriteLine("\nПосле удаления Подсолнуха:");
        foreach (var plant in plantCollection)
        {
            Console.WriteLine(plant);
        }

        // Задание 2
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        Console.WriteLine("\n2. Начальный список:");
        Console.WriteLine(string.Join(", ", numbers));

        numbers.RemoveRange(2, 3);
        Console.WriteLine("\nПосле удаления 3 элементов:");
        Console.WriteLine(string.Join(", ", numbers));

        numbers.Add(99);
        numbers.AddRange(new[] { 100, 101 });
        Console.WriteLine("\nПосле добавления элементов:");
        Console.WriteLine(string.Join(", ", numbers));

        LinkedList<int> linkedList = new LinkedList<int>(numbers);
        Console.WriteLine("\nСвязанный список:");
        foreach (var number in linkedList)
        {
            Console.Write(number + " ");
        }
        Console.WriteLine();

        int searchValue = 99;
        bool found = linkedList.Contains(searchValue);
        Console.WriteLine($"\nСодержит {searchValue}: {found}");

        // Задание 3
        ObservableCollection<Plant> observablePlants = new ObservableCollection<Plant>();
        observablePlants.CollectionChanged += Plants_CollectionChanged;

        observablePlants.Add(new Plant("Лилия", "Lilium", 0.6));
        observablePlants.Add(new Plant("Клён", "Acer", 5.0));
        observablePlants.RemoveAt(0);
    }

    private static void Plants_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (Plant newItem in e.NewItems)
            {
                Console.WriteLine($"\n3. Добавлено: {newItem}");
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (Plant oldItem in e.OldItems)
            {
                Console.WriteLine($"\n3. Удалено: {oldItem}");
            }
        }
    }
}
