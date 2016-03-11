using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atkin_MoreinPrimeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int m = 17;
            for (int i = 1; i < m; i++)
            {
                Console.Write(i + " ");
                Console.Write("i^2 = " + i * i % m);
                Console.WriteLine();
            }
            for (int i = 1; i < m; i++)
            {
                Console.WriteLine(Extension.Lezhandr(i, m));
            }

            Console.WriteLine(Extension.SquareRootModPrime(4,m));
            Console.ReadKey();

        }
    }
}
