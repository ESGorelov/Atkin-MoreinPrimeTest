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
            List<TimeSpan> time = new List<TimeSpan>();
            Stopwatch st = new Stopwatch();
            int countfalse = 0;
            int countprime = 0;
            int countcomposite = 0;
            BigInteger N = BigInteger.Pow(2, 254) - 1;
            //BigInteger N = BigInteger.Parse("618970019642690137449566183");
            PrimeTesting pt = new PrimeTesting();
            while (true)
            {
                while (!FactorOrders.MRTest(N, new List<int> { 2, 3, 5, 7, 11 }))
                {
                    N+=2;
                }
                pt.SetNewNumber(N);
                Console.WriteLine(N);
                st.Start();
                pt.StartTesting();
                st.Stop();
                time.Add(st.Elapsed);
                st.Reset();
                if (pt.Certificate == "Polynom")
                    countfalse++;
                else if (pt.Certificate == "Composite")
                    countcomposite++;
                else
                    countprime++;
                
                N+=2;

            }
            #endregion
        }


    }
}
