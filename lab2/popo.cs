using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class Popo
    {
        private int возраст;

        public int Возраст
        {
            get { return возраст; }
            set { возраст = value; }
        }
        public Popo(int возраст)
        {
            Возраст = возраст;
        }
        public void LookInfo()
        {
            Console.WriteLine($"Возраст:{Возраст}");
        }


        public string Строка(string input)
        {
            return $"{input} Возраст: {возраст} ";
        }
    }

    class Program
    {
        static void Main()
        {
            Popo popo = new Popo(24);
            popo.LookInfo();
            string result = popo.Строка("opopopo");
            Console.WriteLine(result);
        }

    }
}