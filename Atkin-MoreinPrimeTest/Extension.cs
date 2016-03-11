using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Atkin_MoreinPrimeTest
{
    class Extension
    {
        public static int Lezhandr(BigInteger a, BigInteger m) // символ Якоби. Если m - простое то Якоби = Лежандр
        {
            a = BigInteger.Remainder(a, m);
            int t = 1;
            while (a.CompareTo(0)!=0)
            {
                while(BigInteger.Remainder(a,2) == 0)
                {
                    a = BigInteger.Divide(a, 2);
                    if (BigInteger.Remainder(m, 8) == 3 || BigInteger.Remainder(m, 8) == 5)
                        t = -t;
                }
                BigInteger temp = a;
                a = m;
                m = temp;
                if (BigInteger.Remainder(a, 4) == 3 && BigInteger.Remainder(m, 4) == 3)
                    t = -t;
                a = BigInteger.Remainder(a, m);
            }
            if (m.CompareTo(1) == 0)
                return t;

            return 0;
        }
        public static BigInteger SquareRootModPrime(BigInteger a,BigInteger p) // решение сравнения x^2 mod p = a. если -1 то решения нет
        {
            if (Lezhandr(a, p) != 1) return -1;

            a = BigInteger.Remainder(a, p);

            if(BigInteger.Remainder(p, 8)==3 || BigInteger.Remainder(p, 8)==7)
            {
                BigInteger x = BigInteger.ModPow(a, (p + 1) / 4, p);
                return x+x >p ? BigInteger.Abs(x-p):x;
            }

            if (BigInteger.Remainder(p, 8) == 5)
            {
                BigInteger x = BigInteger.ModPow(a, (p + 3) / 8, p);
                BigInteger y = BigInteger.ModPow(x, 2, p);
                if (y != a) x = BigInteger.Remainder(x * BigInteger.ModPow(2, (p - 1) / 4, p), p);
                return x + x > p ? BigInteger.Abs(x - p) : x;
            }

            BigInteger b = 1;
            while (Lezhandr(b, p) != -1) b++;

            int s = 0; BigInteger t = p - 1;
            while (BigInteger.Remainder(t, 2) == 0)
            {
                t = BigInteger.Divide(t, 2);
                s++;
            } // разложение 2^s * t

            BigInteger A = Inverse(a, p);
            BigInteger c = BigInteger.ModPow(b, t, p);
            BigInteger r = BigInteger.ModPow(a,(t+1)/2,p);
            for(int i = 1; i< s;i++)
            {
                BigInteger exp = BigInteger.Pow(2, s - 1 - i);
                BigInteger d = BigInteger.ModPow(r * r * A, exp, p);
                if (d == p - 1) r = BigInteger.Remainder(r * c, p);
                c = BigInteger.ModPow(c, 2, p);
            }
            if (r + r > p) r = BigInteger.Abs(r - p);
            return r + r > p ? BigInteger.Abs(r - p) : r;
        }

        static BigInteger Inverse(BigInteger ch, BigInteger n)
        {
            BigInteger a = ch, y = 0;
            BigInteger b = n, x = BigInteger.Zero, d = BigInteger.One;
            while (a.CompareTo(BigInteger.Zero) == 1)//a>0
            {
                BigInteger q = BigInteger.Divide(b, a);
                y = a;
                a = BigInteger.Remainder(b, a);
                b = y;
                y = d;
                d = BigInteger.Subtract(x, BigInteger.Multiply(q, d));
                x = y;
            }
            x = BigInteger.Remainder(x, n);
            if (x.CompareTo(BigInteger.Zero) == -1)//x<0
            {
                x = BigInteger.Remainder(BigInteger.Add(x, n), n);
            }
            return x;
        }
    }
}
