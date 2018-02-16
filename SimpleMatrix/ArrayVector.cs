namespace SimpleMatrix
{
    internal class ArrayVector
    {
        public static double[] MultiplyOnK(double k, double[] arr)
        {
            var length = arr.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr[i] * k;

            return newArr;
        }

        public static double[] DivideOnK(double[] arr,double k)
        {
            var length = arr.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr[i] / k;

            return newArr;
        }

        public static double[] Sum(double[] arr1, double[] arr2)
        {
            var length = arr1.Length;
            var newArr = new double[length];

            for (int i = 0; i < length; i++)
                newArr[i] = arr1[i] + arr2[i];

            return newArr;
        }

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
