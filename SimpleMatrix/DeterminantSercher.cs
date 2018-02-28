using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMatrix
{
    /*!
 \brief  Детерминант

 Класс для нахождения детерминанта
 */
    public static class DeterminantSercher
    {
        /// <summary>
        /// Детерминант матрицы
        /// </summary>
        /// <param name="A">Матрица</param>
        /// <returns>Детерминант</returns>
        public static double GetDeterminant(Matrix A)
        {
            if (!A.IsSquare())
                throw new NonSquareMatrixException();

            var length = A.Rows;
            double determinant = 1;

            for (int i = 0; i < length-1; i++)
            {
                //поиск максимума в столбце, свап срок и деление строки на макс. элемент 
                Tuple<double, int, Matrix> temp = RowChanging(i, A, length);
                A = temp.Item3;
                
                //Сделать нули под указанным элементом
                DoNullsUnderCurrentlement(A, i, length);

                //Накапливаем детерминант
                determinant *= temp.Item1 * temp.Item2;
            }

            //захватываем последний элемент
            determinant *= A[length - 1, length - 1];

            return determinant;    
        }
        
        //кортеж (множитель для детерминанта, и int: -1 Если строки переставлялись, 1 Если не переставлялись)
        static Tuple<double, int, Matrix> RowChanging(int n, Matrix A, int size)
        {
            var Column = A.GetColumn(n);
            //находим максимальный элемент по модулю
            var indexWithMaxEl = RowWithAbsMaxFromTo(Column.data, n, size);

            //не переставляли строки местами
            int wasSwapped = 1;

            if (indexWithMaxEl != n)
            {
                A = A.SwapRows(n, indexWithMaxEl);
                wasSwapped = -1; //переставляли строки местами
            }

            var row = A.GetRow(n);
            var coef = row[n];

            if (coef == 0.0)
                throw new DeterminantException();

            row = row /coef;

            //меняем строку в исходной матрице
            A.GetDataByReference()[n] = row.data;

            return new Tuple<double, int, Matrix>(coef, wasSwapped, A);
        }

        //строка с самым большим по модулю элементом
        static int RowWithAbsMaxFromTo(double[] arr, int FromIndex, int TOIndex)
        {
            int indx = FromIndex;
            var max = Math.Abs(arr[indx]);
            
            for (int i = FromIndex; i < TOIndex; i++)
            {
                var curr = Math.Abs(arr[i]);
                if (curr > max)
                {
                    max = curr;
                    indx = i; 
                }
            }

            return indx;           
        }

        //
        private static void DoNullsUnderCurrentlement(Matrix a, int n, int length)
        {
            double[] currRow = a.GetRow(n).data;
            double[] nextRow;
            double[] temp;

            for (int i = n+1; i < length; i++)
            {
                //текущая строка умножная на первый коэфициент под ним
                temp =MultiplyRowFromTo(currRow,a[i, n], n,length);
                
                //следующая строка
                nextRow = a.GetRow(i).data;

                SubstractFromTo(nextRow, temp, n, length);

                a.GetDataByReference()[i] = nextRow;
            }            
        }

        //умножить массив от указаного и до указаного элемента
        static double[] MultiplyRowFromTo(double[] row, double k, int fromIndex, int ToIndex)
        {
            double[] clone = (double[])row.Clone();
            for (int i = fromIndex; i < ToIndex; i++)
                clone[i] =row[i]*k;

            return clone;
        }

        //Разница указанных массивов от и до
        /// <param name="a">Тот массив, от которого отнимают</param>
        /// <param name="b">Тот массив, который отнимают</param>
        static void SubstractFromTo(double[] a, double[] b, int FromIndex, int ToIndex)
        {
            for (int i = FromIndex; i < ToIndex; i++)
                a[i] = a[i] - b[i];
        }
    }
}
