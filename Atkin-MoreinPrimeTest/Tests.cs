using System;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atkin_MoreinPrimeTest
{
    class Tests
    {
        public static void TestSquareRootModPrimeANDLezhandr(int primeNumber,int a)
        {           
            for (int i = 1; i < primeNumber; i++)
            {
                Console.Write(i + " ");
                Console.Write("i^2 = " + i * i % primeNumber);
                Console.WriteLine();
            }
            for (int i = 1; i < primeNumber; i++)
            {
                Console.WriteLine(Extension.Lezhandr(i, primeNumber));
            }

            Console.WriteLine(Extension.SquareRootModPrime(a, primeNumber));
        } // Вычисление символа Лежандра для поля Fp и решение x^2 mod primeNumber = a
        public static void TestSqrt(BigInteger number, int index = 2)
        {
            BigInteger temp = Extension.Sqrt(number,index);
            switch (index)
            {
                case 2:
                    Console.WriteLine("Число: " + number + " Корень(2): " + temp + " Корень^2: " + BigInteger.Pow(temp, 2));
                    break;
                case 3:
                    Console.WriteLine("Число: " + number + " Корень(3): " + temp + " Корень^3: " + BigInteger.Pow(temp, 3));
                    break;
            }
        }// целочисленный квадратный и кубический корень
        public static void TestKornakiSmit()
        {
            BigInteger p = BigInteger.Pow(2, 89) - 1;
            int[] D = new int[] { -3, -4, -7, -8, -11, -19, -43, -67, -163, -15, -20, -24, -35, -40, -51, -52, -88, -91, -115 };
            foreach(int d in D)
            {
                var list = Extension.KornakiSmit(p, d);
                if (list.Count == 0)
                    Console.WriteLine("Нет решения" + "  " + d);
                else
                    Console.WriteLine(list[0][0] + "  " + list[0][1] + "  " + d);
                Console.WriteLine();
            }
        }
        public static void TestPolynom()
        {
            //Polynom p = new Polynom(new List<BigInteger> { 2, 3, 4, 9, 0, 0, 1, },11);
            //Polynom p2 = new Polynom(new List<BigInteger> { 7,0,0,1 }, 11);
            
            //Console.WriteLine(p.ToString());
            //Console.WriteLine(p2.ToString());        
            //Console.WriteLine((Polynom.Remainder(p, p2)).ToString());
            //Console.WriteLine(p.ToString());
            //Console.WriteLine(p2.ToString());
        }
        public static void TestPolynomParse()
        {
            var a = Polynom.Parse("+1x^4+2x^3-3x+4",10);
            var a1 = Polynom.Parse("+x^2-x-2",10);
            Console.WriteLine(a);
            Console.WriteLine(a1);
            Console.WriteLine(a-a1);           
            Console.WriteLine(a + a1);
            Console.WriteLine(a * a1);
            Console.WriteLine(Polynom.Remainder(a,a1));
            Console.WriteLine();
            Console.WriteLine(a);
            Console.WriteLine(a1);


        }
    }
}
