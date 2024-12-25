using System;

 public static class TaskNum1
{


    public static void Point1A()
    {

        Console.WriteLine("__Task 1.a__");
        Console.WriteLine("sbyte: ");
        sbyte sbyteType = Convert.ToSByte(Console.ReadLine());

        Console.WriteLine("byte: ");
        byte byteType = Convert.ToByte(Console.ReadLine());

        Console.WriteLine("short: ");
        short shortType = Convert.ToInt16(Console.ReadLine());

        Console.WriteLine("ushort: ");
        ushort ushortType = Convert.ToUInt16(Console.ReadLine());

        Console.WriteLine("int: ");
        int intType = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("uint: ");
        uint uintType = Convert.ToUInt32(Console.ReadLine());

        Console.WriteLine("long: ");
        long longType = Convert.ToInt64(Console.ReadLine());

        Console.WriteLine("ulong: ");
        ulong ulongType = Convert.ToUInt64(Console.ReadLine());

        Console.WriteLine("float: ");
        float floatType = Convert.ToSingle(Console.ReadLine());

        Console.WriteLine("double: ");
        double doubleType = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("decimal: ");
        decimal decimalType = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("bool: ");
        bool boolType = Convert.ToBoolean(Console.ReadLine());

        Console.WriteLine("char: ");
        char charType = Convert.ToChar(Console.ReadLine() ?? "");


        Console.WriteLine($"sbyte: {sbyteType}");
        Console.WriteLine($"byte: {byteType}");
        Console.WriteLine($"short: {shortType}");
        Console.WriteLine($"ushort: {ushortType}");
        Console.WriteLine($"int: {intType}");
        Console.WriteLine($"uint: {uintType}");
        Console.WriteLine($"long: {longType}");
        Console.WriteLine($"ulong: {ulongType}");
        Console.WriteLine($"float: {floatType}");
        Console.WriteLine($"double: {doubleType}");
        Console.WriteLine($"decimal: {decimalType}");
        Console.WriteLine($"bool: {boolType}");
        Console.WriteLine($"char: {charType}");
    }

    //------------------------------------------------------------------------------------------------


    public static void Point1B()
    {
        Console.WriteLine("\n\n__Task 1.b__");
        Console.WriteLine("\n явное приведение:\n");

        int intB = 10;
        double doubleB = (double)intB;
        Console.WriteLine($"{intB} --> {doubleB}");

        float floatB = 3.14f;
        int intB2 = (int)floatB;
        Console.WriteLine($"{floatB} --> {intB2}");

        long longB = 1000;
        short shortB = (short)longB;
        Console.WriteLine($"{longB} --> {shortB}");

        byte byteB = 65;
        char charB = (char)byteB;
        Console.WriteLine($"{byteB} --> {charB}");

        double doubleB2 = 3.99;
        int intB3 = (int)doubleB2;
        Console.WriteLine($"{doubleB2} --> {intB3}");



        Console.WriteLine("Неявное приведение:\n");
        int intB4 = 5;
        double doubleB3 = intB4;
        Console.WriteLine($"{intB4} --> {doubleB3}");

        short shortB2 = 100;
        int intB5 = shortB2;
        Console.WriteLine($"{shortB2} --> {intB5}");

        float floatB2 = 2.5f;
        double doubleB4 = floatB2;
        Console.WriteLine($"{floatB2} --> {doubleB4}");

        byte byteB2 = 50;
        int intB6 = byteB2;
        Console.WriteLine($"{byteB2} --> {intB6}");

        long longB2 = 1000;
        float floatB3 = longB2;
        Console.WriteLine($"{longB2} --> {floatB3}");
    }

    //------------------------------------------------------------------------------------------------
    public static void Point1C()
    {
        Console.WriteLine("\n\n__Task 1.c__");
        int intC = 42;

        object boxedC = intC;

        int unpackedC = (int)boxedC;

        Console.WriteLine($"intC: {intC}");
        Console.WriteLine($"boxedC: {boxedC}");
        Console.WriteLine($"unpackedC: {unpackedC}");
    }


    //------------------------------------------------------------------------------------------------

    public static void Point1D()
    {
        Console.WriteLine("\n\n__Task 1.d__");
        var message = "message";
        var number = 5;

        Console.WriteLine($"Тип переменной message: {message.GetType()}");
        Console.WriteLine($"Значение переменной message: {message}");
        Console.WriteLine($"\nТип переменной number: {number.GetType()}");
        Console.WriteLine($"Значение переменной number: {number}");
    }
    //------------------------------------------------------------------------------------------------



    public static void Point1E()
    {
        Console.WriteLine("\n\n__Task 1.e__");
        int? nullable = null;
        Console.WriteLine($"nullable: {nullable}");

        nullable = 10;
        Console.WriteLine($"New nullable: {nullable}");
    }


    //------------------------------------------------------------------------------------------------

    public static void Point1F()
    {
        Console.WriteLine("\n\n__Task 1.f__");
        var varF = 5;
        Console.WriteLine($"varF: {varF}");

    }
}