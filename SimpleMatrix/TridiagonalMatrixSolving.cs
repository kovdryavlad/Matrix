using System;
using System.Linq;

namespace SimpleMatrix
{
    //решение тридиагональных матриц 
    /*!
 \brief ThreeDiagonalMatrix
 \author Vlad Kovdrya
 \warning Работа только с трехдиагональными матрицами

 Класс для работы с трехдиагональными матрицами. */
    public static class TridiagonalMatrixSolving
    {
        /// <summary>
        /// Делит тридиагональную матрицу (квадратный массив) на три одмомерных массива
        /// </summary>
        /// <param name="inputMatrix">Входная матрица в виде квадратного массива</param>
        /// <returns>Кортеж (диагональ, лежащая под главной; ГЛАВНАЯ ДИАГОНАЛЬ; диагональ, лежащая над главной)</returns>
        static Tuple<double[], double[], double[]> GetDiagonalsFromMatrix(double[][] inputMatrix)
        {
            int N = inputMatrix.Length;

            double[] A = new double[N];
            double[] C = new double[N];
            double[] B = new double[N];

            for (int i = 0; i < N; i++)
            {
                if (i != 0)
                    A[i] = inputMatrix[i][i - 1];
                else
                    A[i] = 0;

                C[i] = inputMatrix[i][i];

                if (i != N - 1)
                    B[i] = inputMatrix[i][i + 1];
                else
                    B[i] = 0;
            }

            return new Tuple<double[], double[], double[]> (A, C, B);
        }

        /// <summary>
        /// Решение тридиагональной матрицы методом прогонки
        /// </summary>
        /// <param name="inputMatrix">Матрица</param>
        /// <param name="f">Правая часть(стоблец)</param>
        /// <returns>Вектор иксов</returns>
        public static double[] Normal(double[][] inputMatrix, double[] f)
        {
            int N = inputMatrix.Length;

            var tuple = GetDiagonalsFromMatrix(inputMatrix);
            var a = tuple.Item1;
            var c = tuple.Item2;
            var b = tuple.Item3;

            return TridiagonalMatrixAlgorithm(N, a, c, b, f);
        }

        /// <summary>
        /// Решение тридианональной матрицы методом прогонки
        /// </summary>
        /// <param name="a">Диагонвль, лежащая под главной</param>
        /// <param name="c">Главная Диагональ</param>
        /// <param name="b">Диагонвль, лежащая Над главной</param>
        /// <param name="f">Правая часть (столбец)</param>
        /// <returns>Вектор иксов</returns>
        public static double[] Normal(double[] a, double[] c, double[] b, double[] f)
        {
            int N = c.Length;

            return TridiagonalMatrixAlgorithm(N, a, c, b, f);
        }

        //сам метод решения (Прогонка обычная)
        private static double[] TridiagonalMatrixAlgorithm(int n, double[] a, double[] c, double[] b, double[] f)
        {
              /*
               * b - диагональ, лежащая над главной (нумеруется: [0;n-2])  //то есть b[n-1](последнее) равняется 0. Так должно быть
               * c - главная диагональ матрицы A (нумеруется: [0;n-1])		
               * a - диагональ, лежащая под главной (нумеруется: [1;n-1])  //то есть a[0] равняется 0. Так должно быть
               * f - правая часть (столбец)
               * x - решение, массив x будет содержать ответ
              */

            double[] x = new double[n];
            
            double m;
            for (int i = 1; i < n; i++)
            {
                m = a[i] / c[i - 1];
                c[i] = c[i] - m * b[i - 1];
                f[i] = f[i] - m * f[i - 1];
            }

            x[n - 1] = f[n - 1] / c[n - 1];

            for (int i = n - 2; i >= 0; i--)
                x[i] = (f[i] - b[i] * x[i + 1]) / c[i];

            return x;
        }

        //циклическая прогонка
        /// <summary>
        /// Цклическая прогонка 
        /// </summary>
        /// <param name="a">vector a</param>
        /// <param name="b">vector b</param>
        /// <param name="c">vector c</param>
        /// <param name="f">vector f</param>
        /// <returns>Y</returns>
        public static double[] Cyclyc(double[] a, double[] c, double[] b, double[] f)
        {
            //для математических формул где нумерация начинается с единицы
            int N = f.Length + 1;
            
            //сместить массив ена единицу вправо(нулеой элемент равен 0)
            Func<double[], double[]> Offset = inputArray =>
            {

                var lst = inputArray.ToList();
                lst.Insert(0, 0);

                return lst.ToArray();

            };

            a = Offset(a);
            c = Offset(c);
            b = Offset(b);
            f = Offset(f);

            double[] alpha = new double[N];
            double[] beta = new double[N];
            double[] gama = new double[N];
            double[] p = new double[N];
            double[] q = new double[N];
            double[] y = new double[N];

            N = N - 1;

            alpha[2] = b[1] / c[1];
            beta[2] = f[1] / c[1];
            gama[2] = a[1] / c[1];

            for (int i = 2; i < N; i++)
            {
                double z = c[i] - a[i] * alpha[i];
                
                alpha[i+1] = b[i]/z;
                beta[i + 1] = (f[i] + a[i] * beta[i]) / z;
                gama[i + 1] = (a[i] * gama[i]) / z;
            }


            p[N - 1] = beta[N];
            q[N - 1] = alpha[N] + gama[N];

            for (int i = N-2; i >= 1; i--)
            {
                p[i] = alpha[i + 1] * p[i + 1] + beta[i + 1];
                q[i] = alpha[i + 1] * q[i + 1] + gama[i + 1];
            }

            y[N] = (f[N] + b[N] * p[1] + a[N] * p[N - 1]) / (c[N] - b[N] * q[1] - a[N] * q[N - 1]);

            for (int i = 1; i < N; i++)
                y[i] = p[i] + y[N] * q[i];

            return y;
        }
        /// <summary>
        /// Цикл без отрицательных значений для циклической прогонки 
        /// </summary>
        /// <param name="a">vector a</param>
        /// <param name="b">vector b</param>
        /// <param name="c">vector c</param>
        /// <param name="f">vector f</param>
        /// <returns>Y</returns>
        public static double[] CyclycWithoutMinuses(double[] a, double[] c, double[] b, double[] f)
        {
            int N = f.Length + 1;

            Func<double[], double[]> Offset = inputArray =>
            {

                var lst = inputArray.ToList();
                lst.Insert(0, 0);

                return lst.ToArray();

            };

            a = Offset(a);
            c = Offset(c);
            b = Offset(b);
            f = Offset(f);

            double[] alpha = new double[N];
            double[] beta = new double[N];
            double[] gama = new double[N];
            double[] p = new double[N];
            double[] q = new double[N];
            double[] y = new double[N];

            N = N - 1;

            alpha[2] = -b[1] / c[1];
            beta[2] = f[1] / c[1];
            gama[2] = -a[1] / c[1];

            for (int i = 2; i < N; i++)
            {
                double z = -c[i] - a[i] * alpha[i];

                alpha[i + 1] = b[i] / z;
                beta[i + 1] = (-f[i] + a[i] * beta[i]) / z;
                gama[i + 1] = (a[i] * gama[i]) / z;
            }


            p[N - 1] = beta[N];
            q[N - 1] = alpha[N] + gama[N];

            for (int i = N - 2; i >= 1; i--)
            {
                p[i] = alpha[i + 1] * p[i + 1] + beta[i + 1];
                q[i] = alpha[i + 1] * q[i + 1] + gama[i + 1];
            }

            y[N] = (-f[N] + b[N] * p[1] + a[N] * p[N - 1]) / (-c[N] - b[N] * q[1] - a[N] * q[N - 1]);

            for (int i = 1; i < N; i++)
                y[i] = p[i] + y[N] * q[i];



            var lsty = y.ToList();
            lsty.RemoveAt(0);
            y = lsty.ToArray();

            return y;
        }
    }
}