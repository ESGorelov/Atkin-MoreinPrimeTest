using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atkin_MoreinPrimeTest
{
    class Polynom
    {
        #region свойства
        public BigInteger Degree
        {
            get;
            private set;
        }
        public BigInteger Fp // Поле Галуа (Если Fp = 0 то поле R)
        {
            get;
            private set;
        }
        List<Monom> coefficients; // номер элемента = степень x
        #endregion
        #region Конструкторы
        Polynom(Polynom a)
        {
            Degree = a.Degree;
            Fp = a.Fp;
            coefficients = a.coefficients;
        }
        public Polynom(List<Monom> coeff, BigInteger _Fp)
        {
            coefficients = coeff;
            Fp = _Fp;
            CheckPoly();
        }

        #endregion

        #region Операции с Полиномами
        /// <summary>
        /// Разность полиномов
        /// </summary>
        /// <param name="a">Уменьшаемое</param>
        /// <param name="b">Вычитаемое</param>
        /// <returns></returns>
        public static Polynom operator -(Polynom a, Polynom b)
        {
            a.NormPolynom();
            b.NormPolynom();
            
            List<Monom> c = new List<Monom>();
            int curnta = 0; // Текущий элемент в полиноме а
            int curntb = 0; // Текущий элемент в полиноме b

            while(curnta < a.coefficients.Count && curntb < b.coefficients.Count)
            {
                if (a.coefficients[curnta].Degree < b.coefficients[curntb].Degree)
                {
                    c.Add(Monom.Negate(b.coefficients[curntb]));
                    curntb++;
                } else if (a.coefficients[curnta].Degree > b.coefficients[curntb].Degree)
                {
                    c.Add(a.coefficients[curnta]);
                    curnta++;
                } else
                {
                    c.Add(a.coefficients[curnta].SubNumber(b.coefficients[curntb].Coefficient));
                    curnta++;
                    curntb++;
                }
            }

            if (curnta == a.coefficients.Count)
            {
                while (curntb < b.coefficients.Count)
                {
                    c.Add(Monom.Negate(b.coefficients[curntb]));
                    curntb++;
                }
            }
            else if (curntb == b.coefficients.Count)
            {
                while (curnta < a.coefficients.Count)
                {
                    c.Add(a.coefficients[curnta]);
                    curnta++;
                }
            }
            return new Polynom(c, a.Fp);
        } 
        /// <summary>
        /// Сумма полиномов
        /// </summary>
        /// <param name="a">Слагаемое</param>
        /// <param name="b">Слагаемое</param>
        /// <returns></returns>
        public static Polynom operator +(Polynom a, Polynom b)
        {
            a.NormPolynom();
            b.NormPolynom();

            List<Monom> c = new List<Monom>();
            int curnta = 0; // Текущий элемент в полиноме а
            int curntb = 0; // Текущий элемент в полиноме b

            while (curnta < a.coefficients.Count && curntb < b.coefficients.Count)
            {
                if (a.coefficients[curnta].Degree < b.coefficients[curntb].Degree)
                {
                    c.Add(b.coefficients[curntb]);
                    curntb++;
                }
                else if (a.coefficients[curnta].Degree > b.coefficients[curntb].Degree)
                {
                    c.Add(a.coefficients[curnta]);
                    curnta++;
                }
                else
                {
                    c.Add(a.coefficients[curnta].AddNumber(b.coefficients[curntb].Coefficient));
                    curnta++;
                    curntb++;
                }
            }

            if (curnta == a.coefficients.Count)
            {
                while (curntb < b.coefficients.Count)
                {
                    c.Add(b.coefficients[curntb]);
                    curntb++;
                }
            }
            else if (curntb == b.coefficients.Count)
            {
                while (curnta < a.coefficients.Count)
                {
                    c.Add(a.coefficients[curnta]);
                    curnta++;
                }
            }
            return new Polynom(c, a.Fp);
        }
        /// <summary>
        /// Умножение полинома на число
        /// </summary>
        /// <param name="number">Множитель</param>
        public void MultiplyInt(BigInteger number)
        {
            for (int i = 0; i <coefficients.Count; i++)
            {
                coefficients[i] = coefficients[i].MultiplyInt(number);
            }
            NormPolynom();
        }
        /// <summary>
        /// Умножение полиномов
        /// </summary>
        /// <param name="A">Множитель</param>
        /// <param name="B">Множитель</param>
        /// <returns></returns>
        public static Polynom operator *(Polynom A, Polynom B)
        {
            List<Monom> c = new List<Monom>();
            for(int i = 0; i < A.coefficients.Count; i++)
            {
                for(int j = 0; j < B.coefficients.Count; j++)
                {
                    c.Add(A.coefficients[i] * B.coefficients[j]);
                }
            }
            return new Polynom(c, A.Fp);
        }
        public static Polynom Remainder(Polynom a, Polynom b)
        {
            if (a.Degree < b.Degree) return a;
            List<Monom> div = new List<Monom>(); // целая часть
            while (a.Degree >= b.Degree)
            {
                BigInteger current_deg = a.Degree - b.Degree;
                Monom c = new Monom(current_deg, 1); // целая часть от деления

                BigInteger currentInt = a.coefficients[0].Coefficient * Extension.Inverse(b.coefficients[0].Coefficient, b.Fp);
                c.MultiplyInt(currentInt);
                div.Add(c);
                Polynom temp = new Polynom(new List<Monom> { c }, a.Fp);
                a -= temp * b;
                a.NormPolynom();
            }
            return a;
        }
        #endregion

        /// <summary>
        /// Нормировка полинома в заданном поле.
        /// </summary>
        private void NormPolynom()
        {
            if (Fp != 0) // Если Fp = 0 то поле R
            {
                for (int i = 0; i <coefficients.Count; i++)
                {
                    if (coefficients[i].Coefficient < 0)
                    {
                        BigInteger k = BigInteger.Abs(coefficients[i].Coefficient) / Fp + 1;
                        coefficients[i] = coefficients[i].AddNumber(Fp * k);
                    }
                    if (coefficients[i].Coefficient >= Fp)
                    {
                        coefficients[i] = coefficients[i].Remainder(Fp);
                    }
                }
            }
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            CheckPoly();

            if (Degree == 0)// Если нулевая степень полинома
            {
                sb.Append(coefficients[0].Coefficient);
            }
            else
            {
                sb.Append(string.Format("{0}x^{1}", coefficients[0].Coefficient, coefficients[0].Degree)); // первый моном

                for (int i = 1; i < coefficients.Count - 1; i++) // от второго до предпоследнего монома
                {
                    if (coefficients[i].Coefficient > 0)
                    {
                        sb.Append(string.Format(" + {0}x^{1}", coefficients[i].Coefficient, coefficients[i].Degree));
                    }
                    else
                    {
                        sb.Append(string.Format(" - {0}x^{1}", BigInteger.Abs(coefficients[i].Coefficient), coefficients[i].Degree));
                    }
                }

                if (coefficients[coefficients.Count - 1].Degree == 0) // Если степень последнего нулевая (Свободный член)
                {
                    if (coefficients[coefficients.Count - 1].Coefficient > 0)
                    {
                        sb.Append(string.Format(" + {0}", coefficients[coefficients.Count - 1].Coefficient));
                    }
                    else
                    {
                        sb.Append(string.Format(" - {0}", BigInteger.Abs(coefficients[coefficients.Count - 1].Coefficient)));
                    }
                }
                else
                {
                    if (coefficients[coefficients.Count - 1].Coefficient > 0)
                    {
                        sb.Append(string.Format(" + {0}x^{1}", coefficients[coefficients.Count - 1].Coefficient, coefficients[coefficients.Count - 1].Degree));
                    }
                    else
                    {
                        sb.Append(string.Format(" - {0}x^{1}", BigInteger.Abs(coefficients[coefficients.Count - 1].Coefficient), coefficients[coefficients.Count - 1].Degree));
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Создает объект типа Polynom из переданной строки
        /// </summary>
        /// <remarks>Перед первым членом обязательно указывается знак. Например: Polynom.Parse("+x^3+2x^2+3x^1+4,19) </remarks>
        /// <param name="_poly">Строка с полиномом</param>
        /// <param name="_Fp">Модуль(Поле Галуа)</param>
        public static Polynom Parse(string _poly, int _Fp)
        {
            if (_poly == "") throw new Exception("string is Empty"); // пустая строка

            StringBuilder poly = new StringBuilder(_poly);
            List<Monom> polycoeff = new List<Monom>(); // Коэфф. полинома в разреженном виде

            while(poly.Length!=0)
            {
                BigInteger coef = 1;
                BigInteger deg = 1;
                #region sign
                bool sign = true;
                if(poly[0]!= '-' && poly[0]!='+') throw new Exception("Input string was malformed");
                if (poly[0] =='-')
                {
                    sign = false;
                }
                #endregion

                if (poly[1] == 'x')
                {
                    if(poly.Length >2 && poly[2] =='^')
                    {
                        int countdeg = ParseNumber(poly, 3);
                        deg = BigInteger.Parse(poly.ToString(3, countdeg));
                        poly = poly.Remove(0, countdeg + 3);
                        if (sign)
                        {
                            polycoeff.Add(new Monom(deg, coef));
                        }
                        else
                        {
                            polycoeff.Add(new Monom(deg, -coef));
                        }
                    }
                    else
                    {
                        poly = poly.Remove(0, 2);
                        if (sign)
                        {
                            polycoeff.Add(new Monom(deg, coef));
                        }
                        else
                        {
                            polycoeff.Add(new Monom(deg, -coef));
                        }
                    }
                }
                else if (poly[1] >= '1' && poly[1] <= '9')
                {
                    int count = ParseNumber(poly, 1);
                    if (count + 1 < poly.Length) 
                    {
                        string c = poly.ToString(1, count);
                        if(sign)
                        {
                            coef = BigInteger.Parse(c);
                        }
                        else
                        {
                            coef = BigInteger.Negate(BigInteger.Parse(c));
                        }
                
                        if (poly[count+1] != 'x') throw new Exception("Input string was malformed");

                        if(count+2 < poly.Length)
                        {
                            if (poly[count + 2] == '^')
                            {
                                int countdeg = ParseNumber(poly, count + 3);
                                string d = poly.ToString(count + 3, countdeg);
                                deg = BigInteger.Parse(d);

                                polycoeff.Add(new Monom(deg, coef));
                                poly = poly.Remove(0, count + countdeg + 3);
                            }
                            else if (poly[count + 2] == '+' || poly[count + 2] == '-')
                            {
                                polycoeff.Add(new Monom(deg, coef));
                                poly = poly.Remove(0, count + 2);
                            }
                            else throw new Exception("Input string was malformed");

                        }
                        else
                        {
                            polycoeff.Add(new Monom(deg, coef));
                            poly = poly.Remove(0, count + 2);
                        }                                                
                    }
                    else // свободный член
                    {
                        string c = poly.ToString(1, count);
                        coef = BigInteger.Parse(c);
                        if(sign)
                         polycoeff.Add(new Monom(0, coef));
                        else
                            polycoeff.Add(new Monom(0, -coef));
                        poly = poly.Remove(0, count+1);
                    }                   
                }
                else throw new Exception("Input string was malformed");    
                          
            }
            return new Polynom(polycoeff,_Fp);
        }
        /// <summary>
        /// Выделяет целое число из строки с полиномом
        /// </summary>
        /// <param name="_poly">Полином в строковой записи</param>
        /// <param name="startindex">Начальный индекс числа</param>
        /// <returns></returns>
        static int ParseNumber(StringBuilder _poly, int startindex)
        {
            if (_poly[startindex] < '1' || _poly[startindex] > '9') return 0;
            int count = 1;
            while (count+ startindex < _poly.Length && (_poly[startindex + count] >= '0' && _poly[startindex + count] <= '9'))
            {
                count++;
            }
            return count;
        }
        /// <summary>
        /// Удаление нулевых коэффициентов
        /// </summary>
        void DeleteZeroCoef()
        {
            coefficients.RemoveAll(delegate (Monom a)
            {
                return a.Coefficient.CompareTo(0) == 0;
            });
        } 
        /// <summary>
        /// Сортировка по убыванию степеней
        /// </summary>
        void SortOfDegree()
        {
            coefficients.Sort(delegate (Monom a1, Monom a2)
            {
                return a2.Degree.CompareTo(a1.Degree);
            });
        }
        /// <summary>
        /// Суммирует мономы с одинаковыми степенями в пределах одного полинома
        /// </summary>
        void SummEqualsDegry()
        {
            SortOfDegree();
            for(int i = 0; i < coefficients.Count - 1;)
            {
                if (coefficients[i].Degree == coefficients[i + 1].Degree)
                {
                    coefficients[i] =  coefficients[i].AddNumber(coefficients[i + 1].Coefficient);
                    coefficients.RemoveAt(i + 1);
                }
                else
                    i++;
            }
        }
        /// <summary>
        /// Удаление нулевых коэффициентов, Сортировка,сложение одинаковых степеней, установка степени полинома
        /// </summary>
        void CheckPoly()
        {
            DeleteZeroCoef();
            SummEqualsDegry();
            NormPolynom();
            Degree = coefficients[0].Degree;
        }  

    }
    struct Monom
    {
        public BigInteger Degree
        {
            get;
            private set;
        }
        public BigInteger Coefficient
        {
            get;
            private set;
        }
        public Monom(BigInteger _degre, BigInteger _coeff)
        {
            Degree = _degre;
            Coefficient = _coeff;
        }
        /// <summary>
        /// Прибавляет к коэффициенту число
        /// </summary>
        /// <param name="number">Число</param>
        public Monom AddNumber(BigInteger number)
        {
            Coefficient += number;
            return this;
        }
        /// <summary>
        /// Отнимает от коэффициента число
        /// </summary>
        /// <param name="number">Число</param>
        public Monom SubNumber(BigInteger number)
        {
            Coefficient -= number;
            return this;
        }
        /// <summary>
        /// Остаток по модулю
        /// </summary>
        /// <param name="number">Модуль</param>
        public Monom Remainder(BigInteger number)
        {
            Coefficient = BigInteger.Remainder(Coefficient,number);
            return this;
        }
        /// <summary>
        /// Изменяет знак коэффициента
        /// </summary>
        /// <returns></returns>
        static public Monom Negate(Monom a)
        {
            return new Monom(a.Degree, BigInteger.Negate(a.Coefficient));
        }
        /// <summary>
        /// Умножение монома на число
        /// </summary>
        /// <param name="number">Множитель</param>
        /// <returns></returns>
        public Monom MultiplyInt(BigInteger number)
        {
            Coefficient = BigInteger.Multiply(Coefficient, number);
            return this;
        }
        /// <summary>
        /// Умножение монома на моном
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Monom operator *(Monom a,Monom b)
        {
            Monom c = new Monom();
            c.Coefficient = a.Coefficient * b.Coefficient;
            c.Degree = a.Degree + b.Degree;
            return c;
        }
    }
}
