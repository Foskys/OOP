using System;

public static class TaskNum4
{
    public static void Point4()
    {

        Console.WriteLine("\n\n__Task 4__");

        (int, string, char, string, ulong) tuple = (10, "Hello", 'A', "World", 456);

        Console.WriteLine($"Кортеж полностью: {tuple}");
        Console.WriteLine($"Кортеж выборочно: {tuple.Item1} {tuple.Item2} {tuple.Item3}");

        (int int1, string string1, char char1, string string2, ulong ulong1) = (10, "Hello", 'A', "World", 456);
        Console.WriteLine("\nРаспаковка кортежа в переменные:");
        Console.WriteLine($"int1: {int1}");
        Console.WriteLine($"string1: {string1}");
        Console.WriteLine($"char1: {char1}");
        Console.WriteLine($"string2: {string2}");
        Console.WriteLine($"ulong1: {ulong1}");

        var tupleVar = (10, "hi", 'D');
        Console.WriteLine("\nРаспаковка кортежа без указания типов: ");

        Console.WriteLine(tupleVar.Item1);
        Console.WriteLine(tupleVar.Item2);
        Console.WriteLine(tupleVar.Item3);
        (_, string a, _) = tupleVar;
        Console.WriteLine(a);
    }
}


//------------------------------------------------------------------------------------------------ 


public static class TaskNum5
{
    public static void Point5()
    {
        Console.WriteLine("\n\n__Task 5__");
        int[] numbers = { 8, 8, 0, 0, 5, 5, 5, 3, 5, 3, 5 };
        string myString = "Hello";

        (int max, int min, int sum, char firstChar) tuple5(int[] arr, string str)
        {
            int maxNum = arr[0];
            int minNum = arr[0];
            int sumNum = 0;

            foreach (int num in arr)
            {
                if (num > maxNum)
                {
                    maxNum = num;
                }

                if (num < minNum)
                {
                    minNum = num;
                }

                sumNum += num;
            }

            char firstLetter = str[0];
            return (maxNum, minNum, sumNum, firstLetter);
        }

        var result = tuple5(numbers, myString);

        Console.WriteLine("Результаты:");
        Console.WriteLine($"Максимальный элемент: {result.max}");
        Console.WriteLine($"Минимальный элемент: {result.min}");
        Console.WriteLine($"Сумма элементов: {result.sum}");
        Console.WriteLine($"Первая буква строки: {result.firstChar}\n");

        var tuple5b = (5, "hello", 3.14);
        var tuple5c = (1, "world", 3.16);
        Console.WriteLine($"Сравнение кортежей {tuple5b} и {tuple5c} ");

        if (Tuple.Equals(tuple5b, tuple5c))
        {
            Console.WriteLine("Кортежи равны");
        }
        else
        {
            Console.WriteLine("Кортежи не равны");
        }
    }
}


//------------------------------------------------------------------------------------------------ 


public static class TaskNum6
{
    public static void Point6()
    {
        Console.WriteLine("\n\n__Task 6__");
        CheckedOp();
        UncheckedOp();
    }

    static void CheckedOp()
    {
        void CheckedOperation()
        {
            checked
            {
                int maxValue = int.MaxValue;
                maxValue++;
                Console.WriteLine($"Максимальное значение с checked: {maxValue}");
            }
        }
        try
        {
            CheckedOperation();
        } catch(Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }

    static void UncheckedOp()
    {
        void UncheckedOperation()
        {
            unchecked
            {
                int maxValue = int.MaxValue + 1;
                Console.WriteLine($"Максимальное значение с unchecked: {maxValue}");
            }
        }

        try
        {
            UncheckedOperation();
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}