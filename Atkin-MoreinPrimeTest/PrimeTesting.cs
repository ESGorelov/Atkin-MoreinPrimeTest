using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using EllipticCurve;
using System.IO;

namespace Atkin_MoreinPrimeTest
{
    class PrimeTesting
    {
        #region Константы
        /// <summary>
        /// Дискриминанты с числом классов 1 и 2 по возрастанию
        /// </summary>
        private int[] D = new int [] {-3,-4,-7,-8,-11,-19,-43,-67,-163,-15,-20,-24,-35,-40,-51,-52,-88,-91,-115,-123,-148,-187,-232,-235,-267,-403,-427};

        #endregion

        #region Свойства
        /// <summary>
        /// Число для тестрования на простоту
        /// </summary>
        public static BigInteger Number
        {
            get;
            private set;
        }
        /// <summary>
        /// Класс разложения на множители
        /// </summary>
        private FactorOrders fct;
        private StringBuilder Cert;
        /// <summary>
        /// Сертификат числа
        /// </summary>
        public string Certificate
        {
            get
            {
                return Cert.ToString();
            }
            private set
            {

            }
        }
        #endregion

        #region Конструкторы
    
        public PrimeTesting()
        {
            Cert = new StringBuilder();
            fct = new FactorOrders();
        }
        #endregion

        #region Методы
        public void StartTesting()
        {
            BigInteger res = Testing();  
            while(res!=-1 && res!=1)
            {
                if (res == -5) break;
                Number = res;
                res = Testing();
            }      
            if(res == -1)
            {
                Cert = new StringBuilder("Composite");
            } else if(res == -5)
            {
                Cert = new StringBuilder("Polynom");
            }          
        }
        public void SetNewNumber(BigInteger N)
        {
            Cert = new StringBuilder();
            if(D.Length>30)
                D = new int[] { -3, -4, -7, -8, -11, -19, -43, -67, -163, -15, -20, -24, -35, -40, -51, -52, -88, -91, -115, -123, -148, -187, -232, -235, -267, -403, -427 };
            Number = N;
        }

        BigInteger Testing(int _currentD = 0)
        {
            //--наименьшее псевдопростое по 4 базам
            if (Number < 3215031751)
                if (FactorOrders.MRTest(Number, new List<int> { 2, 3, 5, 7 }))
                    return 1;
                else
                    return -1;

            if(_currentD == D.Length) //Кончились дискриминанты
            {
                D = FundamentalDiscrim(9000,D);
                return Testing(_currentD);
            }
            int currentD = _currentD;
            //--------------L(D,N) = 1------------------------------------
            while (Extension.Lezhandr(D[currentD], Number) != 1)
            {
                currentD++;
                if (currentD == D.Length)
                {
                    D = FundamentalDiscrim(9000,D);
                    return Testing(_currentD);
                }// Кончились дискриминанты
            }
            //-----------Пытаемся-найти-представление-4p=u^2+|D|v^2-----------------------   
            List<BigInteger> uv = new List<BigInteger>();
            while (currentD < D.Length)
            {
                 uv = Extension.KornakiSmit(Number, D[currentD]);
                if(uv.Count==0)
                    currentD++;
                else       
                    break;              
            }
            if (currentD == D.Length)
            {
                if (currentD > 2000) return -1;
                D = FundamentalDiscrim(9000,D);
                return Testing(currentD);
            }// Кончились дискриминанты
            //-----------------------------------------------------------------------
            //-------------------Получаем возможные порядки------------------------------
            List<BigInteger> ordersCurve = new List<BigInteger>();
            if (D[currentD] == -3) // 6 порядков
            {               
                ordersCurve.Add(Number + 1 + ((uv[0] + 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 - ((uv[0] + 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 + ((uv[0] - 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 - ((uv[0] - 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
            }
            else if(D[currentD] == -4 )// 4 порядка
            {
                ordersCurve.Add(Number + 1 + 2 * uv[1]);
                ordersCurve.Add(Number + 1 - 2 * uv[1]);
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
            }
            else
            {
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
            }
            //-----------------------------------------------------------------------
            //-----------------Раскладываем порядки на множители---------------------

            fct.SetNewNumbers(ordersCurve);
            while (true)
            {
                fct.StartFact();
                if (fct.result != null)
                {
                    //---------------Если попал простой порядок-----------------------
                    if(fct.result.Count == 1)
                    {
                        ordersCurve.RemoveRange(0, fct.NumberResult + 1);
                        fct.SetNewNumbers(ordersCurve);
                        continue;
                    }
                    BigInteger orderCurve = ordersCurve[fct.NumberResult];
                    //-----------------Получаем параметры кривой---------------------
                    var curveParam = GetCurveComplexMultiply(D[currentD]);
                    if (curveParam == null) return -5;// Проблемы с нахождением корней многочлена
                    var paramCurve = GetCurveParamForOrder(orderCurve, curveParam);
                    if (paramCurve == null)
                    {
                        ordersCurve.RemoveRange(0, fct.NumberResult + 1);
                        fct.SetNewNumbers(ordersCurve);
                        continue;
                    }
                    //---------------------------------------------------------------
                    //-------------Операции с точкой---------------------------------
                    EllipticCurvePoint P = new EllipticCurvePoint(Number, paramCurve[0], paramCurve[1]);
                    BigInteger k = orderCurve / fct.result[fct.result.Count - 1];
                    var U = EllipticCurvePoint.Multiply(k, P);
                    if (U.CheckPoint() == false) return -1; // Составное
                    while (U.X == 0 && U.Z == 0)
                    {
                        P.NexPoint();
                        U = EllipticCurvePoint.Multiply(k, P);
                        if (U.CheckPoint() == false)
                            return -1; // Составное
                    }
                    var V = EllipticCurvePoint.Multiply(fct.result[fct.result.Count - 1], U);
                    if (V.X != 0 && V.Z != 0)
                        return -1; // Составное
                                   //--------------Формирование Сертификата---------------------

                    Cert.Append(string.Format("N = {0}",Number)+ "\r\n");
                    Cert.Append(string.Format("D = {0}  U = {1}  V = {2}",D[currentD],uv[0],uv[1])+ "\r\n");
                    Cert.Append("#E = ");
                    foreach(var del in fct.result)
                    {
                        Cert.Append(del + " * ");
                    }
                    Cert.Remove(Cert.Length - 3, 3);
                    Cert.Append("\r\n");
                    Cert.Append(string.Format("E({0}, {1}, {2}, {3}, {4}, {5} )", P.A, P.B, P.P, P.X, P.Y, P.Z)+ "\r\n");
                    //---------------------------------------------------------------
                    //-----------Возврат числа q-------------------------------------

                    return fct.result[fct.result.Count - 1];

                    //---------------------------------------------------------------
                }
                else // если порядки разложить не удалось, то начинаем с начала со следующего дискриминанта
                {
                    return Testing(currentD + 1);
                }
            }   
        }
        #endregion

        #region privateMethods
        /// <summary>
        /// Параметры кривых полученных комплексным умножением
        /// </summary>
        /// <param name="D">Фундаментальный Дискриминант</param>
        /// <returns></returns>
        private static List<BigInteger[]> GetCurveComplexMultiply(int D)
        {
            List<BigInteger[]> paramCurve = new List<BigInteger[]>();
            BigInteger g = GetNonQuadr();
            if(D == -3)
            {
                for(int i = 0; i < 6; i++)
                {
                    paramCurve.Add(new BigInteger[] {0,BigInteger.ModPow(-g,i,Number)});
                }
                return paramCurve;            
            }
            else if (D == -4)
            {
                for (int i = 0; i < 4; i++)
                {
                    paramCurve.Add(new BigInteger[] { BigInteger.ModPow(-g, i, Number), 0 });
                }
                return paramCurve;
            }
            else
            {
                ComplexPolynom GilbPol = Extension.GilbertPolynom(D);
                Polynom GilbPolFp = GilbPol.GetPolynomReal(Number);
                //---------------------------------------------------------------
                //-----------------Получение корня полинома----------------------   

                BigInteger root;
                if (GilbPolFp.Degree <= 2)
                {
                    var roots = GilbPolFp.GetRoots();
                    if (roots.Count != 0)
                        root = roots[0];
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var nod = Polynom.GreatCommonDivisor(GilbPolFp, Polynom.Derivative(GilbPolFp));
                    if(nod.Degree!=0 && nod.Degree<3)
                    {
                        var roots = nod.GetRoots();
                        if (roots.Count != 0)
                            root = roots[0];
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(new FileStream("polynom.txt", FileMode.Append)))
                        {
                            sw.WriteLine("//------------------------------------------------------------------------");
                            sw.WriteLine(Number);
                            sw.WriteLine("D = " + D);
                            sw.WriteLine(GilbPolFp + " mod " + Number);
                            sw.WriteLine("//------------------------------------------------------------------------");
                            sw.Close();
                        }
                        return null;
                    }                   
                }

                //---------------------------------------------------------------
                //------------------Получение параметров-------------------------
                BigInteger c = root * Extension.Inverse(root - 1728, Number);
                c = BigInteger.Remainder(c, Number);
                BigInteger r = BigInteger.Remainder(3 * BigInteger.Negate(c), Number);
                BigInteger s = BigInteger.Remainder(2 * c, Number);
                //---------------------------------------------------------------
                paramCurve.Add(new BigInteger[] { r, s });
                paramCurve.Add(new BigInteger[] { BigInteger.Remainder(r * BigInteger.ModPow(g, 2, Number), Number), BigInteger.Remainder(s * BigInteger.ModPow(g, 3, Number), Number) });
                return paramCurve;          
                //---------------------------------------------------------------
            }
        }
        /// <summary>
        /// Случайный квадратичный невычет по модулю тестируемого числа
        /// </summary>
        /// <returns>Случайный квадратичный невычет</returns>
        private static BigInteger GetNonQuadr()
        {
            if (Number % 3 == 1)
            {
                BigInteger g = Extension.Random(1, Number);
                while ((Extension.Lezhandr(g, Number) != -1) || (BigInteger.ModPow(g,(Number - 1)/3, Number) == 1))
                {
                    g = Extension.Random(1, Number);
                }
                return g;
            }
            else
            {
                BigInteger g = Extension.Random(1, Number);
                while (Extension.Lezhandr(g, Number) != -1)
                {
                    g = Extension.Random(1, Number);
                }
                return g;
            }            
        }
        /// <summary>
        /// Поиск кривой с заданным порядком
        /// </summary>
        /// <param name="orderCurve">Необходимый порядок</param>
        /// <param name="paramCurve">Параметры кривых</param>
        /// <returns></returns>
        private static BigInteger[] GetCurveParamForOrder(BigInteger orderCurve, List<BigInteger[]> paramCurve )
        {
            int count = 0;
            List<int> curveNum = new List<int>(); ;
            int iteration = 0;
            while (iteration < 5)
            {
                curveNum = new List<int>();
                for (int i = 0; i < paramCurve.Count; i++)
                {
                    EllipticCurvePoint point = new EllipticCurvePoint(Number, paramCurve[i][0], paramCurve[i][1]);
                    EllipticCurvePoint orderPoint = EllipticCurvePoint.Multiply(orderCurve, point);
                    if (orderPoint.X == 0 && orderPoint.Z == 0)
                    {
                        count++;
                        curveNum.Add(i);
                    }
                }
                if (count == 1)
                    return paramCurve[curveNum[0]];
                count = 0;
                iteration++;
            }
            if (curveNum.Count == 0) return null; // Порядки не подошли
            return paramCurve[curveNum[1]];
        }
        /// <summary>
        /// Фундаментальные Дискриминанты
        /// </summary>
        /// <param name="border">Граница</param>
        /// <returns>Массив дискриминантов</returns>
        private static int[] FundamentalDiscrim(int border, int[] _oldD)
        {
            List<int> d = _oldD.ToList<int>();
            int[] prime = new int[] { 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271 };
            for (int i = 1; i < border; i++)
            {
                if (i % 16 == 3 || i % 16 == 4 || i % 16 == 7 || i % 16 == 8 || i % 16 == 11 || i % 16 == 15)
                {
                    int t = i;
                    while ((t & 0x1) == 0) t = t >> 1;
                    for (int j = 0; j < prime.Count() && prime[j] < (int)(Math.Sqrt(i)) + 1; j++)
                    {
                        if (t % (prime[j] * prime[j]) == 0)
                        {
                            t = 0;
                            break;
                        }
                    }
                    if (t != 0)
                    {
                        if(!d.Contains(-i))
                        {
                            d.Add(-i);
                        }
                    }
                }
            }
            return d.ToArray();
        }
        #endregion

    }
}
