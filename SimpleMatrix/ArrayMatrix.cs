using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleMatrix
{
    /*!
\brief  Массив матриц


*/
    //операции над матрицами в виде зубчастых массивов
    public static class ArrayMatrix
    {
        //возвращает нужный зубчастыЙ массив
        /// <summary>
        /// Нахождение зубчастого масссива
        /// </summary>
        /// <param name="rows">Количество строк</param>
        /// <param name="columns">Количество колонок</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] GetJaggedArray(int rows, int columns)
        {
            double[][] array = new double[rows][];

            for (int i = 0; i < rows; i++)
                array[i] = new double[columns];

            return array;
        }
        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="A">Массив массивов 1</param>
        /// <param name="B">Массив массивов 2</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] Sum(double[][] A, double[][] B)
        {
            int ARows = A.Length;
            int ACols = A[0].Length;

            double[][] SumMatrix = GetJaggedArray(ARows, ACols);

            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < ACols; j++)
                    SumMatrix[i][j] = A[i][j] + B[i][j];

            return SumMatrix;
        }
        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="A">Массив массивов 1</param>
        /// <param name="B">Массив массивов 2</param> 
        /// <returns>Новый массив</returns>
        //разница матриц
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] Difference(double[][] A, double[][] B)
        {
            int ARows = A.Length;
            int ACols = A[0].Length;

            double[][] DifferenceMatrix = GetJaggedArray(ARows, ACols);

            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < ACols; j++)
                    DifferenceMatrix[i][j] = A[i][j] - B[i][j];

            return DifferenceMatrix;
        }

        //Умножение обычное N^3
        /// <summary>
        /// Умножение
        /// </summary>
        /// <param name="A">Массив массивов 1</param>
        /// <param name="B">Массив массивов 2</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] MultiplyN3(double[][] A, double[][] B)
        {
            int ARows = A.Length;
            int ACols = A[0].Length;
            int BCols = B[0].Length;

            double[][] C = GetJaggedArray(ARows, BCols);//new double[a.GetLength(0), b.GetLength(1)];

            double sum = 0;

            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < BCols; j++)
                {
                    sum = 0;

                    for (int k = 0; k < ACols; k++)
                        sum += A[i][k] * B[k][j];

                    C[i][j] = sum;
                }

            return C;
        }

        //используется в быстром умножении - умножает одну строку на один столбец
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Mult1(double[][] AA, int ii, double[][] BB, int jj)
        {
            double Sum = 0;
           
            int ACols = AA[0].Length;
            for (int i = 0; i < ACols; i++)
                Sum += AA[ii][i] * BB[jj][i];

            return Sum;
        }

        //быстрое умножение с использованием транспонирования
        /// <summary>
        /// Быстрое умножение (с использованием транспонирования)
        /// </summary>
        /// <param name="A">Массив массивов 1</param>
        /// <param name="B">Массив массивов 2</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] MultiplyN3Transpose(double[][] A, double[][] B)
        {
            int CRows = A.Length;
            int CCols = B[0].Length;

            double[][] C = GetJaggedArray(CRows, CCols);
            
            double[][] Bb = TransposeArr(B);


            Parallel.For(0, CRows, (i) =>
            {
                for (int j = 0; j < CCols; j++)
                    C[i][j] = Mult1(A, i, Bb, j);

            });

            return C;
        }

        /// <summary>
        /// Транспонирование
        /// </summary>
        /// <param name="a">Массив</param>
        /// <returns>Новый массив</returns>
        //транспонирование
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] TransposeArr(double[][] a)
        {

            int ACols = a[0].Length;
            int ARows = a.Length;
            double[][] Transpose = new double[ACols][];//, A.Rows];

            for (int i = 0; i < ACols; i++)
                Transpose[i] = new double[ARows];


            Parallel.For(0, ARows, (i) =>
            {
                for (int j = 0; j < ACols; j++)
                    Transpose[j][i] = a[i][j];
            });
            return Transpose;
        }

        //умножение на число
        /// <summary>
        /// Умножение на число
        /// </summary>
        /// <param name="A">Массив массивов</param>
        /// <param name="k">Число</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] MultiplyOnk(double k, double[][] A)
        {
            int ARows = A.Length;
            int ACols = A[0].Length;

            double[][] KMatrix = GetJaggedArray(ARows, ACols);

            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < ACols; j++)
                    KMatrix[i][j] = A[i][j]*k;

            return KMatrix;
        }

        //деление на число
        /// <summary>
        /// Деление на число
        /// </summary>
        /// <param name="A">Массив массивов</param>
        /// <param name="k">Число</param> 
        /// <returns>Новый массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] DivideOnk(double[][] A, double k)
        {
            int ARows = A.Length;
            int ACols = A[0].Length;

            double[][] KMatrix = GetJaggedArray(ARows, ACols);

            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < ACols; j++)
                    KMatrix[i][j] = A[i][j] / k;

            return KMatrix;
        }
        /// <summary>
        /// Создание единичной матрицы
        /// </summary>
        /// <param name="n">Размер</param>
        /// <returns>Новый массив</returns>
        //возвращает единичную матрицу заданого размера
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] GetIdentity(int n)
        {
            double[][] arr = GetJaggedArray(n, n);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        arr[i][j] = 1;

            return arr;
        }
        /// <summary>
        /// Создание диагональной матрицы
        /// </summary>
        /// <param name="vector">Вектор</param>
        /// <returns>Новый массив</returns>
        //создать дианональную матрицу 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] GetDiagonal(double[] vector)
        {
            int n = vector.Length;

            double[][] arr = GetJaggedArray(n, n);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        arr[i][j] = vector[i];

            return arr;
        }
        /// <summary>
        /// Изменения порядка строк
        /// </summary>
        /// <param name="rowA">Номер строки первого массива </param>
        ///  <param name="data">Массив</param>
        ///  <param name="rowB">Номер строки второго массива </param>
        /// <returns>Новый массив</returns>
        //поменять местами строки
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[][] SwapRows(double[][]data, int rowA, int rowB)
        {
            var dataClone = (double[][])data.Clone();

            var c = dataClone[rowA];

            dataClone[rowA] = dataClone[rowB];
            dataClone[rowB] = c;

            return dataClone;
        }
        /// <summary>
        /// Нахождение диагонали
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <returns>Новый массив</returns>
        //получить диагональ
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] GetDiagonal(double[][] matrix)
        {
            List<double> lst = new List<double>();

            var size = matrix.GetLength(0);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (i == j)
                        lst.Add(matrix[i][j]);

            return lst.ToArray();
        }
        /// <summary>
        /// Нахождение строки
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="numberOfROw">номер строки</param>
        /// <returns>Строка</returns>
        //получить строку
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] GetRow(double[][] matrix, int numberOfROw)
        {
            var row = matrix[numberOfROw];

            return row;
        }
        /// <summary>
        /// Нахождение столбца
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="numberOfColumn">Номер колонки</param>
        /// <returns>Столбец</returns>
        //получить столбец
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] GetColumn(double[][] matrix, int numberOfColumn)
        {
            var transposed = TransposeArr(matrix);

            return GetRow(transposed, numberOfColumn);
        }
    }
}
