using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{
   public static class Pop
    {
        public static string ToTitleCase (this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToUpper(str[0])+str.Substring(1);  
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string example = "hiho";
            Console.WriteLine(example.ToTitleCase());
        }
    }
}
