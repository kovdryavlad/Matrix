using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SimpleMatrix
{
    //TODO
    //SetRow
    //SetColumn
    //И справить в связи с этим поиск детерминанта

    //вся работа с матрицами
    public class Matrix:ICloneable
    {
        //сложение матриц
        static Matrix Sum(Matrix A, Matrix B)
        {
            if (A.CanAdd(B))
            {
                double[][] SumMatrix = ArrayMatrix.Sum(A.data, B.data);
        
                return Create.New(SumMatrix);
            }
            else
                throw new MatrixAddException();
        }

        static Matrix Difference(Matrix A, Matrix B)
        {
            if (A.CanAdd(B))
            {
                double[][] SumMatrix = ArrayMatrix.Difference(A.data, B.data);

                return Create.New(SumMatrix);
            }
            else
                throw new MatrixDifferenceException();
        }

        //произведение матриц
        static Matrix Multiply(Matrix A, Matrix B)
        {
            if (A.CanMultiply(B))
            {   
                return MultiplyN3(A, B);
                
                //return MultiplyN3Transpose(A, B);
            }
            else
                throw new MatrixMultiplyException();
        }

        //умножение матриц (в виде массивов)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Matrix MultiplyN3(Matrix A, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyN3(A.data, B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }
        
        //самое лучшее умножение
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Matrix MultiplyN3Transpose(Matrix A, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyN3Transpose(A.data, B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        //произведение матрицы на число
        static Matrix Multiply(double k, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyOnk(k,B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        //деление на число
        static Matrix Divide(Matrix A, double k)
        {
            double[][] CArr = ArrayMatrix.DivideOnk(A,k);
            var Cmatrix = new Matrix(CArr);
            return Cmatrix;
        }

        //переопределение сложения
        public static Matrix operator +(Matrix A, Matrix B)
        {
            return Sum(A, B);
        }

        //переопределение сложения
        public static Matrix operator -(Matrix A, Matrix B)
        {
            return Difference(A, B);
        }

        //переопределение умножения
        public static Matrix operator *(Matrix A, Matrix B)
        {
            return Multiply(A, B);
        }

        public static Matrix operator *(double k, Matrix B)
        {
            return Multiply(k, B);
        }

        //переопределение деления
        public static Matrix operator /(Matrix A, double k)
        {
            return Divide(A, k);
        }

        //оператор преобразования
        public static implicit operator double[][] (Matrix x)
        {
            return x.data;
        }
        
        /// <summary>
        /// Возвращает транспонированую матрицу
        /// </summary>
        public Matrix Transpose()
        {
            double[][] CArr = ArrayMatrix.TransposeArr(data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }
        
        /// <summary>
        /// Метод проверяет можно ли слаживать матрицу с входной. 
        /// </summary>
        public bool CanAdd(Matrix B)
        {
            if (Rows == B.Rows && Columns == B.Columns)
                return true;

            return false;
        }

        /// <summary>
        /// Метод проверяет можно ли умножать матицу на входную справа. 
        /// </summary>
        public bool CanMultiply(Matrix B)
        {
            if (Columns == B.Rows)
                return true;

            return false;
        }

        //квадратна ли матрица
        public bool IsSquare()
        {
            if (this.Rows == this.Columns)
                return true;
            else
                return false;
        }

        //симмтерична ли матрица
        public bool IsSymmetric()
        {
            if (!this.IsSquare())
                throw new MatrixOperationException("Прямоугольную матрицу нельзя проверить на симетричность");

            var Tmatrix = Transpose();

            int ARows = this.Rows;
            int ACols = this.Columns;
            for (int i = 0; i <ARows ; i++)
                for (int j = 0; j < ACols; j++)
                    if (data[i][j] != data[j][i])
                        return false;

            return true;
        }

        /// <summary>
        /// меняет местами строки матрицы
        /// </summary>
        public Matrix SwapRows(int rowA, int rowB)
        {
            //ошибки
            if (rowA < 0)
                throw new ArgumentException("Индекс строки не может быть меньше 0", "rowA");
            if (rowB < 0)
                throw new ArgumentException("Индекс строки не может быть меньше 0", "rowB");
            if (rowA > Rows)
                throw new ArgumentException("Индекс строки не может быть больше максимаольного индекса в матрице", "rowA");
            if (rowB > Rows)
                throw new ArgumentException("Индекс строки не может быть больше максимаольного индекса в матрице", "rowB");


            //метод
            var swappedArray = ArrayMatrix.SwapRows(data, rowA, rowB);

            return Create.New(swappedArray);
        }
        
        //меняет местами столбцы матрицы
        public Matrix SwapCols(Matrix A, int ColumnA, int ColumnB)
        {
            //ошибки
            if (ColumnA < 0)
                throw new ArgumentException("Индекс столбца не может быть меньше 0", "ColumnA");
            if (ColumnB < 0)
                throw new ArgumentException("Индекс столбца не может быть меньше 0", "ColumnB");
            if (ColumnA > Columns)
                throw new ArgumentException("Индекс столбца не может быть больше максимаольного индекса столбца в матрице", "ColumnA");
            if (ColumnB > Columns)
                throw new ArgumentException("Индекс столбца не может быть больше максимаольного индекса столбца в матрице", "ColumnB");

            //метод
            var transp = Transpose();

            var swappedArray = ArrayMatrix.SwapRows(transp, ColumnA, ColumnB);

            return Create.New(swappedArray).Transpose();
        }

        public Vector GetRow(int numberOfRow)
        {
            int rows = Rows;    //строк всего
            if (numberOfRow < 0 || numberOfRow >= rows)
                throw new NonExtistRowException();
            
            return new Vector(ArrayMatrix.GetRow(data, numberOfRow));
        }

        public Vector GetColumn(int numberOfColumn)
        {
            int cols = Columns;    //колонок всего
            if (numberOfColumn < 0 || numberOfColumn >= cols)
                throw new NonExtistColumnException();

            return new Vector(ArrayMatrix.GetColumn(data, numberOfColumn));
        }

        /// <summary>
        /// Получает след матрицы(сумму диагональных элементов)
        /// </summary>
        public double Trace()
        {
            if (!IsSquare())
                throw new NonSquareMatrixException();

            var diagonal = ArrayMatrix.GetDiagonal(data);
            return diagonal.Sum();
        }

        public object Clone()
        {
            return new Matrix((double[][])data.Clone());
        }

        //сама матрица
        double[][] data;

        //вренет массив-матрицу по ссылке, а не его клон
        public double[][] GetDataByReference()
        {
            return data;
        }

        //кнструктор
        public Matrix(double[][] arr)
        {
            data = (double[][])arr.Clone();
        }

        public Matrix(int rows, int Columns, double[] matrix)
        {
            if (rows * Columns != matrix.Length)
                throw new MatrixInitializationException();

            var arr = ArrayMatrix.GetJaggedArray(rows, Columns);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < Columns; j++)
                    arr[i][j] = matrix[i * Columns + j];

            data = arr;
            
        }
        /// <summary>
        /// Возвращает количество строк матрицы
        /// </summary>
        public int Rows
        { get { return data.Length; } }
        
        /// <summary>
        /// Возвращает количество столбцов матрицы
        /// </summary>
        public int Columns
        { get { return data[0].Length; } }

        /// <summary>
        /// Возвращает или устанавливает элемент матрицы
        /// </summary>
        /// <param name="i">Строка</param>
        /// <param name="j">Столбец</param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get { return data[i][j]; }

            set { data[i][j] = value; } 
        }

        /// <summary>
        /// Поиск детерминанта для квадратной матрицы
        /// </summary>
        /// <returns></returns>
        public double Determinant()
        {
            return DeterminantSercher.GetDeterminant(this);
        }
        
        //вложенный класс, отвечающий за создание матриц
        /// <summary>
        /// Фабрики для создания матриц
        /// </summary>
        public static class Create
        {
            /// <summary>
            /// Создает квадратную нулевую матрицу n*n
            /// </summary>
            public static Matrix New(int n)
            {
                return New(n, n);
            }
            
            /// <summary>
            /// Создает нулевую прямоугольную матрицу 
            /// </summary>
            public static Matrix New(int rows, int columns)
            {
                double[][] arr = ArrayMatrix.GetJaggedArray(rows, columns);
                return new Matrix(arr);
            }

            /// <summary>
            /// Если уже есть двумерный массив с коэффициентами
            /// </summary>
            /// <returns>Вернет матрицу, полученую из массива</returns>
            public static Matrix New(double[][] MatrixArray)
            {
                return new Matrix(MatrixArray);
            }

            /// <summary>
            /// Если уже есть зубчастый массив с коэффициентами
            /// </summary>
            /// <returns>Вернет матрицу, полученую из массива</returns>
            public static Matrix New(double[,] MatrixArray)
            { 
                var Rows = MatrixArray.GetLength(0);
                var Cols = MatrixArray.GetLength(1);

                var result = ArrayMatrix.GetJaggedArray(Rows, Cols);

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        result[i][j] = MatrixArray[i, j];

                return new Matrix(result); 
            }

            /// <summary>
            /// Создает единичную матрицу размера n*n
            /// </summary>
            public static Matrix Identity(int n)
            {
                double[][] CArr = ArrayMatrix.GetIdentity(n);
                var Cmatrix = Matrix.Create.New(CArr);
                return Cmatrix;
        
            }

            /// <summary>
            /// Получение матрицы из одномерного массива
            /// </summary>
            /// <param name="rows">Строки</param>
            /// <param name="columns">Столбцы</param>
            /// <param name="MatrixAsOneDimArray">Одномерный массив</param>
            /// <returns></returns>
            public static Matrix Identity(int rows, int columns, double[] MatrixAsOneDimArray)
            {
                return new Matrix(rows, columns, MatrixAsOneDimArray);
            }

            /// <summary>
            /// Создает диагональную матрицу
            /// </summary>
            /// <param name="arr">Массив диагональных элементов</param>
            public static Matrix Diagonal(double[] arr)
            {
                double[][] CArr = ArrayMatrix.GetDiagonal(arr);
                var Cmatrix = Matrix.Create.New(CArr);
                return Cmatrix;
            }

            /// <summary>
            /// Создает диагональную матрицу
            /// </summary>
            /// <param name="v">Вектор диагонильных элементов</param>
            public static Matrix Diagonal(Vector v)
            {
                return Diagonal(v.data);    
            }
        }

        public override string ToString()
        {
            int r = Rows;
            int c = Columns;

            string s = "";

            Action<double[]> add2s = v =>
            {
                s += "(";

                var length = v.Length;

                for (int i = 0; i < length; i++)
                    s += String.Format("{0:0.0000}{1}", v[i], i<length-1? "; ":"");

                    s += "); "+Environment.NewLine;
            };

            for (int i = 0; i < r; i++)
                add2s(data[i]);
            
            return s;
        }
    }
}
