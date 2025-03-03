using System;
using System.Collections.Generic;

















public partial class Set
{
    private static int instanceCount = 0;
    private static readonly Random random = new Random();
    private readonly int id;
    private HashSet<int> elements;
    public const string ClassName = "Класс Множество";
    private int pole1;
    private int pole2;

    public int Pole1 { get => pole1; set => pole1 = value; }
    public int Pole2 { get => pole2; set => pole2 = value; }

    static Set()
    {
        instanceCount = 0;
    }

    public Set()
    {
        elements = new HashSet<int>();
        id = CreateUniqueId();
        instanceCount++;
    }
    public Set(IEnumerable<int> elements)
    {
        this.elements = new HashSet<int>(elements ?? throw new ArgumentNullException(nameof(elements)));
        id = CreateUniqueId();
        instanceCount++;
    }

    public Set(int initialElement = 0)
    {
        elements = new HashSet<int> { initialElement };
        id = CreateUniqueId();
        instanceCount++;
    }

    private Set(int id, HashSet<int> elements)
    {
        this.id = id;
        this.elements = new HashSet<int>(elements);
    }

    public int Id => id;
    public int ElementCount { get => elements.Count; private set { } }

    public void AddElement(int element)
    {
        elements.Add(element);
    }

    public void RemoveElement(int element)
    {
        elements.Remove(element);
    }

    public int GetSum()
    {
        int sum = 0;
        foreach (int element in elements)
        {
            sum += element;
        }
        return sum;
    }

    public bool ContainsNegative()
    {
        foreach (int element in elements)
        {
            if (element < 0) return true;
        }
        return false;
    }

    public void ProcessSet(ref int multiplier, out int elementCount)
    {
        elementCount = elements.Count;
        HashSet<int> updatedElements = new HashSet<int>();
        foreach (int element in elements)
        {
            updatedElements.Add(element * multiplier);
        }
        elements = updatedElements;
    }

    public Set CreateCopy()
    {
        return new Set(id, elements);
    }

    public static void PrintClassInfo()
    {
        Console.WriteLine($"Имя класса: {ClassName}, Количество объектов: {instanceCount}");
    }

    public override bool Equals(object obj)
    {
        if (obj is Set otherSet)
        {
            return id == otherSet.id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 3 + id.GetHashCode();
        foreach (int element in elements)
        {
            hash = hash * 3 + element.GetHashCode();
        }
        return hash;
    }

    public override string ToString()
    {
        return $"Множество ID: {id}, Элементы: {{ {string.Join(", ", elements)} }}, Количество элементов: {ElementCount}";
    }

    private int CreateUniqueId()
    {
        return random.Next(1, 99);
    }
}


