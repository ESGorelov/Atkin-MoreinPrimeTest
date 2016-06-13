using System;
using System.Diagnostics;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atkin_MoreinPrimeTest
{
    class Program
    {
        static void Main(string[] args)
        {

            int exp = 164;
            TimeSpan Hours = new TimeSpan(4, 0, 0);
            PrimeTesting pt = new PrimeTesting();

            

            while (exp < 214)
            {
                List<TimeSpan> time = new List<TimeSpan>(); // Список Времени работы
                Stopwatch timeTesting = new Stopwatch();
                Stopwatch WorkTime = new Stopwatch();


                int countfalse = 0;
                int countprime = 0;
                int countcomposite = 0;
                StringBuilder numberfalse = new StringBuilder();
                StringBuilder numbercomposite = new StringBuilder();

                BigInteger N = BigInteger.Pow(2, exp) - 1;

                

                WorkTime.Start();
                Console.WriteLine("Work on exp = " + exp + " start in: " + DateTime.Now + " Length: "+ Hours.ToString());
                while (WorkTime.Elapsed < Hours)
                {
                    while (!MRTest(N, new List<int> { 2, 3, 5, 7 }))
                    {
                        N += 2;
                    }
                    pt.SetNewNumber(N);
                    timeTesting.Start();
                    pt.StartTesting();
                    timeTesting.Stop();

                    if (pt.Certificate == "Error")
                    {
                        countfalse++;
                        numberfalse.Append(N.ToString() + "\r\n");
                    }
                    else if (pt.Certificate == "Composite")
                    {
                        countcomposite++;
                        numbercomposite.Append(N.ToString() + "\r\n");
                    }
                    else
                    {
                        time.Add(timeTesting.Elapsed);
                        countprime++;
                    }
                    timeTesting.Reset();
                    N += 2;
                }
                WorkTime.Stop();
                using (StreamWriter sw = new StreamWriter(new FileStream(string.Format("time_all{0}.txt", exp), FileMode.Append)))
                {
                    foreach (var t in time)
                    {
                        sw.WriteLine(t.TotalMilliseconds);
                    }
                }
                    using (StreamWriter sw = new StreamWriter(new FileStream(string.Format("time{0}.txt",exp), FileMode.Append)))
                {
                    TimeSpan min = time[0];
                    TimeSpan max = time[0];
                    long milisecondTime = 0;

                    foreach (var t in time)
                    {
                        sw.WriteLine(t.TotalMilliseconds);
                        if (t < min)
                            min = t;
                        if (t > max)
                            max = t;
                        milisecondTime += t.Minutes * 6000 + t.Seconds * 1000 + t.Milliseconds;
                    }
                    milisecondTime = milisecondTime / time.Count;
                    long secondTime = milisecondTime / 1000; // seconds
                    TimeSpan taverage = new TimeSpan(0,0, (int)(secondTime / 60), (int)(secondTime % 60),(int)(milisecondTime % 1000));

                    sw.WriteLine("//------------------------------------------------------------------------------//");
                    sw.WriteLine();


                    sw.WriteLine("Exp =     " + exp);
                    sw.WriteLine("Average Time =    " + taverage.ToString());
                    sw.WriteLine("Min Time =    " + min.ToString());
                    sw.WriteLine("Max Time =    " + max.ToString());
                    sw.WriteLine("Пройденный диапазон:  " + BigInteger.Subtract(N, BigInteger.Pow(2, exp) - 1));
                    sw.WriteLine("Кол-во Проверенных чисел:     " + (countcomposite + countfalse + countprime).ToString());
                    sw.WriteLine("Кол-во Составных:     " + countcomposite);
                    sw.WriteLine("Кол-во Простых:     " + countprime);
                    sw.WriteLine("Кол-во Ошибок:     " + countfalse);


                    sw.WriteLine();
                    sw.WriteLine("//------------------------------------------------------------------------------//");
                    sw.WriteLine();
                }
                using (StreamWriter sw = new StreamWriter(new FileStream("failnumber_" + exp + ".txt", FileMode.Append)))
                {
                    sw.Write(numberfalse.ToString());
                }
                using (StreamWriter sw = new StreamWriter(new FileStream("compositenumber_" + exp + ".txt", FileMode.Append)))
                {
                    sw.Write(numbercomposite.ToString());
                }
                Console.WriteLine("{0}  done", exp);
                exp += 10;
                Hours += new TimeSpan(0, 20, 0);
            }
        }
        public static bool MRTest(BigInteger n, List<int> a)
        {
            if (n == 1 || n == 2 || n == 3 || n == 5 || n == 7 || n == 11) return true; // для корректной проверки чисел 2 3 5 7 по всем базам 2 3 5 7
            if ((n & 0x1) == 0) return false;
            BigInteger n1 = BigInteger.Subtract(n, BigInteger.One); // n-1
            BigInteger s = 0, t = n1;
            while ((t & 0x01) == 0)
            {
                t = t >> 1;
                s = BigInteger.Add(s, BigInteger.One);
            } // разложение 2^s * t
            foreach (int basemr in a)
            {

                BigInteger b = BigInteger.ModPow(basemr, t, n);


                if (b.CompareTo(BigInteger.One) == 0 || b.CompareTo(n1) == 0)
                {
                    continue;
                }
                for (int i = 0; i < s; i++)
                {
                    b = BigInteger.ModPow(b, 2, n);
                    if (b.CompareTo(BigInteger.One) == 0)
                    {
                        return false;
                    }
                    if (b.CompareTo(n1) == 0)
                    {
                        break;
                    }
                }
                if (b.CompareTo(n1) != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

}

