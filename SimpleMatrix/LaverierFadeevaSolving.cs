using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Algebra;

namespace SimpleMatrix
{
    /*!
 \brief LaverierFadeevaMethod
 \author Vlad Kovdrya

 Метод Лверье Фадеева
 */
    public static class LaverierFadeevaMethod
    {
        /// <summary>
        /// Нахождение решения с помощью метода Лверье Фадеева
        /// </summary>
        /// <returns></returns>
        public static LaverrierFadeevaMethodResult Solve(Matrix A, LaverierFadeevaSolvingOptions options)
        {
            int size = ValidateInputMatrix(A);  //валидация входной матрицы + в случае успеха получение размера матрицы

            var lstP = new List<double>();      //коефициенты характеристического уравнения
            var lstB = new List<Matrix>();      //список матриц B  - еще пригодяться при подсчете векторов

            var E = Matrix.Create.Identity(size);   //единичная матрица

            Matrix An = (Matrix)A.Clone();

            for (int i = 0; i < size; i++)
            {
                var p = An.Trace()/(i+1.0);
                lstP.Add(p);

                var Bn = An - p * E;
                lstB.Add(Bn);

                An = A * Bn;
            }

            var result = new LaverrierFadeevaMethodResult();
            result.InverseMatrix = lstB[size - 2] / lstP[size - 1];     //обратная матрица

            if (options == LaverierFadeevaSolvingOptions.OnlyInverseOfMatrix)
                return result;

            double[] eigenValues = GetEigenValues(lstP.ToArray(), size);    //получение собственных значений матрицы

            var eigenVectors = GetEigenVectors(E, lstB.ToArray(),eigenValues, size);

            result.EigenValues = eigenValues;
            //result.EigenVectors = eigenVectors.Select(vct => vct.Normilize()).ToArray();
            result.EigenVectors = eigenVectors.ToArray();

            return result;
        }

        
        //валидация входной матрицы
        private static int ValidateInputMatrix(Matrix a)
        {
            if (!a.IsSquare())
                throw new NonSquareMatrixException();

            else
                return a.Rows;
        }

        //получение собственных значений матрицы  
        private static double[] GetEigenValues(double[] p, int numberOfRoots)
        {
            //по формуле, при построении полинома знаки коэфициентов около всех степеней, кроме старшей, меняются
            var SignChangedCoefs = p.Select(v => -v).ToList();

            //добавляем 1 для старшей степени
            SignChangedCoefs.Insert(0, 1);

            //return EquationSolver.GetRoots(SignChangedCoefs.ToArray(), 0.00000001, 0.1);            
            return EquationSolver.GetRoots(SignChangedCoefs.ToArray(),numberOfRoots);            
        }
        
        //получене собственных векторов
        private static Vector[] GetEigenVectors(Matrix e, Matrix[] B, double[] eigenValues, int size)
        {
            //Берем первые колонки из y и B
            Vector y0 = e.GetColumn(0);

            Vector[] b = new Vector[size - 1];
            Parallel.For(0, size - 1, (i) =>
            {
                b[i] = B[i].GetColumn(0);
            });

            var EigenValLength = eigenValues.Length;
            Vector[] EigenVectors = new Vector[EigenValLength];


            Parallel.For(0, EigenValLength, (i) =>
            {
                var length = size - 1;
                var y = (Vector)y0.Clone();
                for (int j = 0; j < length; j++)
                    y = eigenValues[i] * y + b[j];

                EigenVectors[i] = y;
            });

            return EigenVectors;
        }


    }

    ///Опции использования метода Лаверрье-Фадеева
    public enum LaverierFadeevaSolvingOptions
    {
        FullSolving,///<Полное использование метода
        OnlyInverseOfMatrix///< Частичное использование метода. Только обратная матрица
    }
    /*!
  \brief  Результат  метода Лаверье Фадеева

  Класс для работы с метдом Лаверье Фадеева */
    public class LaverrierFadeevaMethodResult
    {
        public Matrix InverseMatrix { get; internal set; }       ///<Обратная матрица
        public double[] EigenValues { get; internal set; }       ///<Собственные числа
        public  Vector[] EigenVectors { get; internal set; }     ///<Собственные векторы
    }
}
