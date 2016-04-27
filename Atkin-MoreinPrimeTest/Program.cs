using System;
using System.Diagnostics;
using System.Numerics;
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
            PrimeTesting pt = new PrimeTesting(BigInteger.Pow(2, 89) - 1);
            pt.StartTesting();
            #region Корни
            //BigInteger p = BigInteger.Pow(2, 89) - 1;
            //BigInteger p = 131;


            //Polynom h = Polynom.Parse("+x^4-9x^2+7", 131);
            //Polynom h = Polynom.Parse("+x^3+1662705765583389101921015x^2+356560280230433613294194825x+489476008241378181249146744", p);

            //Console.WriteLine(h);

            //Console.WriteLine("-----------------Корни:------------------- ");
            //for (int i = 0; i < h.Fp; i++)
            //{
            //    if (h.GetValue(i) == 0)
            //        Console.WriteLine(i);
            //}
            //Console.WriteLine("--------------------------End--------------------");
            //Polynom u = Polynom.Parse("+x", p);
            //Console.WriteLine(u);

            //Polynom n = Polynom.ModPow(Polynom.Parse("+x", p), p, h);

            //Console.WriteLine("-----------------Корни:------------------- ");
            //var t = h.GetRoots();
            //if (t != null)
            //{
            //    foreach (var r in t)
            //    {
            //        Console.WriteLine(r);
            //    }
            //}
            //Console.WriteLine("--------------------------End--------------------");
            #endregion


            Console.ReadKey();

        }


    }
}
