using System;

namespace project01
{

    public static class Main1
    {

        static void Main()
        {
            (int a, int b, int c) kor = (1, 4, 5);
            Console.WriteLine($"{kor.b}");



            {
                int num1 = 10;
                int num2 = 20;

                string ConvertToString(int a, int b)
                {
                    // Явное преобразование
                    string strA = a.ToString();
                    string strB = b.ToString();

                    return strA + ", " + strB;  // Неявное преобразование
                }

                string result = ConvertToString(num1, num2);
                Console.WriteLine(result);
            }
        }
    }
}




















//        TaskNum1.Point1A();
//            TaskNum1.Point1B();
//            TaskNum1.Point1C();
//            TaskNum1.Point1D();
//            TaskNum1.Point1E();
//            TaskNum1.Point1F(); 
//            Console.ReadLine();

//            TaskNum2.Point2A();
//            TaskNum2.Point2B();
//            TaskNum2.Point2C();
//            TaskNum2.Point2D();
//            Console.ReadLine();

//            TaskNum3.Point3A();
//            TaskNum3.Point3B();
//            TaskNum3.Point3C();
//            Console.ReadLine();

//            TaskNum4.Point4();
//            TaskNum5.Point5();
//            TaskNum6.Point6();

//        }
//    }
//}