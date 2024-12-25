using System;
using System.Text; //2.d

public static class TaskNum2
{
    public static void Point2A()
    {
        Console.WriteLine("\n\n__Task 2.a__");
        string str1 = "Hello";
        string str2 = "World";

        if (str1 == str2)
            Console.WriteLine($"{str1} == {str2}");
        else
            Console.WriteLine($"{str1} != {str2}");

        string str3 = "Hello";

        if (str1 == str3)
            Console.WriteLine($"{str1} == {str3}");
        else
            Console.WriteLine($"{str1} != {str3}");
    }

    //------------------------------------------------------------------------------------------------

    public static void Point2B()
    {
        Console.WriteLine("\n\n__Task 2.b__");
        string stringB1 = "String1";
        string stringB2 = "StringNum2";
        string stringB3 = "lorem ipsum,qwerty";
        string stringB4 = "lorem";

        string concString = stringB1 + stringB2;
        Console.WriteLine($"Сцепление: {concString}"); 

        string copyString = stringB2.Substring(0);
        Console.WriteLine($"Копирование: {copyString}");

        string subString = stringB3.Substring(0, 3);
        Console.WriteLine($"Выделение подстроки строки {stringB3}: {subString}\n");


        string[] stringWords = stringB3.Split(' ', ',');
        Console.WriteLine("Разделение строки на слова:");
        foreach (string word in stringWords)
        {
            Console.WriteLine(word);
        }

        string insertString = stringB3.Insert(5, subString);
        Console.WriteLine($"\nВставка подстроки {subString} в строку {stringB3}: {insertString}");

        string removeString = stringB4.Remove(3, 2);
        Console.WriteLine($"Удаление подстроки в строке {stringB4}: {removeString}");
    }

    //------------------------------------------------------------------------------------------------

    public static void Point2C()
    {
        Console.WriteLine("\n\n__Task 2.c__");
        string? stringC1 = "";
        string? stringC2 = null;
        string? stringC3 = "hello";

        if (string.IsNullOrEmpty(stringC1))
            Console.WriteLine("stringC1 is null or empty");
        else
            Console.WriteLine("stringC1 is not null or empty");

        if (string.IsNullOrEmpty(stringC2))
            Console.WriteLine("stringC2 is null or empty");
        else
            Console.WriteLine("stringC2 is not null or empty");

        if (string.IsNullOrEmpty(stringC3))
            Console.WriteLine("stringC3 is null or empty");
        else
            Console.WriteLine("stringC3 is not null or empty");


        string? stringC4 = "  ";
        string? stringC5 = " \n";

        if (string.IsNullOrWhiteSpace(stringC4))
            Console.WriteLine("\nstringC4 состоит только из пробелов или null");
        else
            Console.WriteLine("stringC4 состоит не только из пробелов и не null");

        if (string.IsNullOrWhiteSpace(stringC5))
            Console.WriteLine("stringC5 состоит только из пробелов или null");
        else
            Console.WriteLine("stringC5 состоит не только из пробелов и не null");

        if (string.IsNullOrWhiteSpace(stringC2))
            Console.WriteLine("stringC2 состоит только из пробелов или null");
        else
            Console.WriteLine("stringC2 состоит не только из пробелов и не null");
    }

    //------------------------------------------------------------------------------------------------

    public static void Point2D()
    {
        Console.WriteLine("\n\n__Task 2.d__");
        StringBuilder sb = new StringBuilder("null string");
        Console.WriteLine($"Исходная строка: {sb}");

        sb.Remove(0, 5);
        Console.WriteLine($"Удаление подстроки null: {sb}");

        sb.Insert(0, "space ");
        sb.Append("!");
        Console.WriteLine($"Добавление space в начало и ! в конец: {sb}");
    }
}