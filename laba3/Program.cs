using System;
using System.Numerics;
using System.Runtime.CompilerServices;
namespace Vector;

public class Vector
{
    public int Value { get; set; }
    public Vector(int value)
    {
        Value = value;
    }
    public static Vector operator ++(Vector v)
    { return new Vector(v.Value+1); }




    private int[] elements;

    public int Length => elements.Length;


    public Vector() => elements = new int[0];

    public Vector(int[] elements) => this.elements = elements ?? throw new ArgumentNullException(nameof(elements));

    public int this[int index]
    {
        get => elements[index];
        set => elements[index] = value;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        int length = Math.Min(a.Length, b.Length);
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = a.elements[i] + b.elements[i];
        }
        return new Vector(result);
    }

    public static bool operator >(Vector a, Vector b) => a.Length > b.Length;

    public static bool operator <(Vector a, Vector b) => a.Length < b.Length;

    public static bool operator ==(Vector a, Vector b) => ReferenceEquals(a, b);

    public static bool operator !=(Vector a, Vector b) => !ReferenceEquals(a, b);

    public static bool operator true(Vector v) => v.Length == 0;

    public static bool operator false(Vector v) => v.Length > 0;

    public void RemovePositiveElements()
    {
        int[] filtered = new int[elements.Length];
        int index = 0;
        foreach (var element in elements)
        {
            if (element <= 0) filtered[index++] = element;
        }
        Array.Resize(ref filtered, index);
        elements = filtered;
    }

    public override string ToString() => $"Vector: {{ {string.Join(", ", elements)} }}";

    public override bool Equals(object obj)
    {
        if (obj is Vector other && other.Length == Length)
        {
            for (int i = 0; i < Length; i++)
            {
                if (elements[i] != other.elements[i]) return false;
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 5;
        foreach (var element in elements)
        {
            hash = hash * 4 + element;
        }
        return hash;
    }

    public class Production
    {
        public int Id { get; }
        public string OrganizationName { get; }

        public Production(int id, string organizationName)
        {
            Id = id;
            OrganizationName = organizationName;
        }

        public override string ToString() => $"Production ID: {Id}, Name: {OrganizationName}";
    }

    public Production ProductionInfo { get; set; } = new Production(21, "EPAM");

    public class Developer
    {
        public string Name { get; }
        public int Id { get; }
        public string Department { get; }

        public Developer(string name, int id, string department)
        {
            Name = name;
            Id = id;
            Department = department;
        }

        public override string ToString() => $"Developer: {Name}, ID: {Id}, Department: {Department}";
    }

    public Developer DeveloperInfo { get; set; } = new Developer("Shmanai Vika", 13, "UI Design");
}

public static class StatisticOperation
{
    public static int Sum(Vector vector)
    {
        int sum = 0;
        for (int i = 0; i < vector.Length; i++)
        {
            sum += vector[i];
        }
        return sum;
    }


    public static int Difference(Vector vector)
    {
        if (vector.Length == 0) return 0;
        int max = vector[0], min = vector[0];
        for (int i = 0; i < vector.Length; i++)
        {
            if (vector[i] > max) max = vector[i];
            if (vector[i] < min) min = vector[i];
        }
        return max - min;
    }


    public static int Count(Vector vector) => vector.Length;
}


public static class ExtensionMethods
{

    public static string TrimStart(this string str, int count)
    {
        if (string.IsNullOrEmpty(str) || count < 0) return str;
        return str.Length > count ? str.Substring(count) : string.Empty;
    }
}

public static class VectorEx

{
    public static Vector Increment(this  Vector vector)

    {
        if(vector == null)
        {
            throw new ArgumentNullException(nameof(vector));
        }
        return new Vector(vector.Value + 1);
    }
}

    





