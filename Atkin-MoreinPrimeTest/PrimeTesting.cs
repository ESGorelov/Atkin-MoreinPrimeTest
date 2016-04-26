using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
        public BigInteger Number
        {
            get;
            private set;
        }
        #endregion

        #region Конструкторы
        private PrimeTesting()
        {

        }
        public PrimeTesting(BigInteger _number)
        {
            Number = _number;
        }
        #endregion

        #region Методы
        public int StartTesting()
        {
            //-----------Пытаемся-найти-представление-4p=u^2+|D|v^2-----------------------
            int currentD = 0;
            List<BigInteger> uv = new List<BigInteger>();
            while (currentD < D.Length)
            {
                 uv = Extension.KornakiSmit(Number, D[currentD]);
                if(uv.Count==0)
                    currentD++;
                else       
                    break;              
            }
            if (currentD == D.Length) return -1;
            //-----------------------------------------------------------------------
            //-------------------Получаем возможные порядки------------------------------
            List<BigInteger> ordersCurve = new List<BigInteger>();
            if (D[currentD] == -3) // 6 порядков
            {
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
                ordersCurve.Add(Number + 1 + ((uv[0] + 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 - ((uv[0] + 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 + ((uv[0] - 3 * uv[1]) >> 1));
                ordersCurve.Add(Number + 1 - ((uv[0] - 3 * uv[1]) >> 1));
            }
            else if(D[currentD] == -4 )// 4 порядка
            {
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
                ordersCurve.Add(Number + 1 + 2 * uv[1]);
                ordersCurve.Add(Number + 1 - 2 * uv[1]);
            }
            else
            {
                ordersCurve.Add(Number + 1 + uv[0]);
                ordersCurve.Add(Number + 1 - uv[0]);
            }
            //-----------------------------------------------------------------------
            //-----------------Раскладываем порядки на множители---------------------


            //-----------------------------------------------------------------------



            return 0;
        }
        #endregion


    }
}
