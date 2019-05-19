using System;
using System.Linq;

namespace Algebra
{
    public static class UpperLimitSearcher
    {
        /// <summary>
        /// Полуение верхней границы положительных корней полинома
        /// </summary>
        public static double GetUpperLimit(double[] сoefficients)
        {
            if (сoefficients.Length < 2)
                throw new ArgumentException("Неверная длинна массива", "сoefficients");

            //Если при старшей степени отрицательный коэффициент, то меняем знаки у всех коэф.
            //корни остаются теми же
            if (сoefficients[0] < 0)
                сoefficients = сoefficients.Select(x => -x).ToArray();

            //Если есть отрицательный коэффициент k!=null
            var k = ExistNegativeInCoefficients(сoefficients);

            double UpperLimit = 0;  //искомая граница
            if (k.HasValue)
                UpperLimit = MethodWithK(сoefficients, k.Value);
            else
                UpperLimit = MethodWithoutK(сoefficients);

            return UpperLimit;
        }

        //Поиск первого отрицательного коэффициента
        private static int? ExistNegativeInCoefficients(double[] сoefficients)
        {
            int? k = 1;
            var length = сoefficients.Length;

            for (; k < length; k++)
                if (сoefficients[k.Value] < 0)
                    break;

            if (k == length)    //если дошли до конца
                return null;

            return k;
        }

        //более точная грацница (При наличии k)
        private static double MethodWithK(double[] сoefficients, int k)
        {
            var B = Math.Abs(сoefficients.Min());

            return 1 + Math.Pow(B / сoefficients[0], 1d / k);
        }
        
        //широкая граница  (При отсутствии k)
        private static double MethodWithoutK(double[] сoefficients)
        {
            var A = сoefficients.Max();

            return 1 + A / сoefficients[0];
        }

    }
}
