using System;
using System.Diagnostics;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Atkin_MoreinPrimeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1
            BigInteger N = BigInteger.Pow(2, 89) - 2;

            while (true)
            {
                while (!FactorOrders.MRTest(N, new List<int> { 2, 3, 5, 7, 11 }))
                {
                    N++;
                }
                Console.WriteLine(N);
                PrimeTesting pt = new PrimeTesting(N);
                pt.StartTesting();
                Console.WriteLine(pt.Certificate);
                N++;
                Thread.Sleep(500);
            }
            #endregion

        }


    }
}
