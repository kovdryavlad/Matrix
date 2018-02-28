namespace SimpleMatrix
{
    /*!
\brief  Массив векторов


*/
    internal class ArrayVector
    {
        /// <summary>
        /// Умножение на число
        /// </summary>
        /// <param name="arr">Массив</param>
        /// <param name="k">Число</param> 
        /// <returns>Новый массив</returns>
        public static double[] MultiplyOnK(double k, double[] arr)
        {
            var length = arr.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr[i] * k;

            return newArr;
        }
        /// <summary>
        /// Деление на число
        /// </summary>
        /// <param name="arr">Массив</param>
        /// <param name="k">Число</param> 
        /// <returns>Новый массив</returns>
        public static double[] DivideOnK(double[] arr,double k)
        {
            var length = arr.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr[i] / k;

            return newArr;
        }
        /// <summary>
        /// Сумма 
        /// </summary>
        /// <param name="arr1">Первый массив</param>
        /// <param name="arr2">Второй массив</param> 
        /// <returns>Новый массив</returns>
        public static double[] Sum(double[] arr1, double[] arr2)
        {
            var length = arr1.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr1[i] + arr2[i];

            return newArr;
        }
        /// <summary>
        /// Вычитание
        /// </summary>
        /// <param name="arr1">Первый массив</param>
        /// <param name="arr2">Второй массив</param> 
        /// <returns>Новый массив</returns>
        public static double[] Substruct(double[] arr1, double[] arr2)
        {
            var length = arr1.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr1[i] - arr2[i];

            return newArr;
        }
    }
}
