using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


/*!
 \brief  Работа с матрицами 
 \author Vlad Kovdrya
 */
namespace SimpleMatrix
{

    //вся работа с матрицами
    /*!
   \brief  Матрицы
   \author Vlad Kovdrya
   \warning Только операции над матрицами и создание матриц

   Класс операций с матрицами. Хранит в себе транспонирование, вычитание, сложение, произведение, преобразование деление и нахождение детерминанта.
   */
    public class Matrix : ICloneable
    {
        /// <summary>
        /// Сложение матриц
        /// </summary>
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
        /// <summary>
        /// Разность матриц
        /// </summary>
        /// <returns></returns>
        static Matrix Substruct(Matrix A, Matrix B)
        {
            if (A.CanAdd(B))
            {
                double[][] SumMatrix = ArrayMatrix.Difference(A.data, B.data);

                return Create.New(SumMatrix);
            }
            else
                throw new MatrixSubstructException();


        }
        //произведение
        /// <summary>
        /// Произведение матриц
        /// </summary>
        /// <returns>Новая матрица</returns>
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

        /// <summary>
        /// умножение текущей матрицы на вектор-столбец
        /// </summary>
        public Vector MultiplyOnVectorColumn(Vector vector)
        {
            int length = vector.Length;
            var data = vector.data;
            Matrix MatrixforVector = new Matrix(length, 1, data);

            Matrix ResultMatrix = this * MatrixforVector;
            return ResultMatrix.GetColumn(0);
        }

        /// <summary>
        /// умножение текущей матрицы на вектор-строку
        /// </summary>
        public Matrix MultiplyOnVectorRow(Vector vector)
        {
            int length = vector.Length;
            var data = vector.data;
            Matrix MatrixforVector = new Matrix(1, length, data);

            return this * MatrixforVector;
        }


        //умножение матриц
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Matrix MultiplyN3(Matrix A, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyN3(A.data, B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        //самое лучшее умножение
        /// <summary>
        /// Произведение матриц (Этот метод работает быстрее на матрицах больших размеров. Размер матриц, при котором лучше перейти на это умножение, зависит от вычислительной мощности машины, на которой выполняется код)
        /// </summary>
        /// <returns>Новая матрица</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix MultiplyN3Transpose(Matrix A, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyN3Transpose(A.data, B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        //произведение матрицы на число
        /// <summary>
        /// Произведение матрицы на число
        /// </summary>
        /// <returns>Новая матрица</returns>
        static Matrix Multiply(double k, Matrix B)
        {
            double[][] CArr = ArrayMatrix.MultiplyOnk(k, B.data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        //деление на число
        /// <summary>
        /// Деление матрицы на число
        /// </summary>
        /// <returns>Новая матрица</returns>
        static Matrix Divide(Matrix A, double k)
        {
            double[][] CArr = ArrayMatrix.DivideOnk(A, k);
            var Cmatrix = new Matrix(CArr);
            return Cmatrix;
        }

        //переопределение сложения
        /// <summary>
        /// Суммирование матриц
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static Matrix operator +(Matrix A, Matrix B)
        {
            return Sum(A, B);
        }

        //переопределение 
        /// <summary>
        /// Вычитание матриц
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static Matrix operator -(Matrix A, Matrix B)
        {
            return Substruct(A, B);
        }

        //переопределение умножения
        /// <summary>
        /// Умножение матриц
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static Matrix operator *(Matrix A, Matrix B)
        {
            return Multiply(A, B);
        }
        /// <summary>
        /// Умножение матрицы на число
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static Matrix operator *(double k, Matrix B)
        {
            return Multiply(k, B);
        }

        //переопределение деления
        /// <summary>
        /// Деление матрицы на число
        /// </summary>
        /// <returns>Новая матрица</returns>
        public static Matrix operator /(Matrix A, double k)
        {
            return Divide(A, k);
        }

        //оператор преобразования
        /// <summary>
        /// Преобразование матрицы в зубчастый массив. Рекомендуется искользовать только для передачи в методы, которые пользователь писал для работы с матрицами в виде массивов
        /// </summary>
        /// <returns>Зубчастый массив</returns>
        public static implicit operator double[][] (Matrix x)
        {
            return (double[][])x.data.Clone();
        }

        /// <summary>
        /// Нахождение транспонированной матрицы (возвращает транспонированую матрицу)
        /// </summary>
        /// <returns>Транспонированная матрица</returns>
        public Matrix Transpose()
        {
            double[][] CArr = ArrayMatrix.TransposeArr(data);
            var Cmatrix = Matrix.Create.New(CArr);
            return Cmatrix;
        }

        public Matrix SetMainDiagonal(double[] diagonalElements)
        {
            double[][] arr = ArrayMatrix.SetMainDiagonal(data, diagonalElements);

            return new Matrix(arr);
        }

        /// <summary>
        /// Метод проверяет можно ли суммировать матрицу с входной. 
        /// </summary>
        /// <returns>true or false</returns> 
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

        //квадратная ли матрица
        /// <summary>
        /// Проверка квадратности матрицы
        /// </summary>
        ///  <returns>true or false</returns>
        public bool IsSquare()
        {
            if (this.Rows == this.Columns)
                return true;
            else
                return false;
        }

        //симмтерична ли матрица
        /// <summary>
        /// Проверка симметричности матрицы
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsSymmetric()
        {
            if (!this.IsSquare())
                throw new MatrixOperationException("Прямоугольную матрицу нельзя проверить на симетричность");

            var Tmatrix = Transpose();

            int ARows = this.Rows;
            int ACols = this.Columns;
            for (int i = 0; i < ARows; i++)
                for (int j = 0; j < ACols; j++)
                    if (data[i][j] != data[j][i])
                        return false;

            return true;
        }

        /// <summary>
        /// Изменение порядка строк матрицы
        /// </summary>
        /// <param name="rowA">Первая строка матрицы</param>
        /// <param name="rowB">Вторая строка матрицы</param>
        /// <returns>Новая матрица</returns>
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
        /// <summary>
        /// Изменение пордка столбцов матрицы
        /// </summary>
        /// <param name="A">Марица</param>
        /// <param name="ColumnA">Первый столбец</param>
        /// <param name="ColumnB">Второй столбец</param>
        /// <returns>Новая матрица</returns>
        public Matrix SwapCols(Matrix A, int ColumnA, int ColumnB)
        {
            //ошибки
            if (ColumnA < 0)
                throw new ArgumentException("Индекс столбца не может быть меньше 0", "ColumnA");
            if (ColumnB < 0)
                throw new ArgumentException("Индекс столбца не может быть меньше 0", "ColumnB");
            if (ColumnA > Columns)
                throw new ArgumentException("Индекс столбца не может быть больше максимального индекса столбца в матрице", "ColumnA");
            if (ColumnB > Columns)
                throw new ArgumentException("Индекс столбца не может быть больше максимального индекса столбца в матрице", "ColumnB");

            //метод
            var transp = Transpose();

            var swappedArray = ArrayMatrix.SwapRows(transp, ColumnA, ColumnB);

            return Create.New(swappedArray).Transpose();
        }

        /// <summary>
        /// Получение указанной строки матрицы
        /// </summary>
        /// <param name="numberOfRow">Номер строки</param>
        /// <returns>Возвращает экзэмпляр класса Vector, образованный с указаной строки</returns>
        public Vector GetRow(int numberOfRow)
        {
            int rows = Rows;    //строк всего
            if (numberOfRow < 0 || numberOfRow >= rows)
                throw new NonExtistRowException();

            return new Vector(ArrayMatrix.GetRow(data, numberOfRow));
        }
        /// <summary>
        /// Получить указанный столбец матрицы
        /// </summary>
        /// <param name="numberOfColumn">Номер столбца</param>
        /// <returns>Возвращает экзэмпляр класса Vector, образованный с указанного столюбца</returns>
        public Vector GetColumn(int numberOfColumn)
        {
            int cols = Columns;    //колонок всего
            if (numberOfColumn < 0 || numberOfColumn >= cols)
                throw new NonExtistColumnException();

            //так же обраюотать ошибки выхода за границы существующих столбцов и строк

            return new Vector(ArrayMatrix.GetColumn(data, numberOfColumn));
        }

        public Matrix RemoveRow(int rowIndex) {
            //ошибки
            if (rowIndex < 0)
                throw new ArgumentException("Индекс строки не может быть меньше 0", "rowIndex");
            if (rowIndex > Rows)
                throw new ArgumentException("Индекс строки не может быть больше максимального индекса строки в матрице", "rowIndex");


            return new Matrix(ArrayMatrix.RemoveRow(data, rowIndex));
        }

        public Matrix RemoveColumn(int columnIndex)
        {
            //ошибки
            if (columnIndex < 0)
                throw new ArgumentException("Индекс столбца не может быть меньше 0", "columnIndex");
            if (columnIndex > Rows)
                throw new ArgumentException("Индекс столбца не может быть больше максимального индекса строки в матрице", "columnIndex");

            Matrix transpose = this.Transpose();
            
            return transpose.RemoveRow(columnIndex).Transpose();
        }

        /// <summary>
        /// Получает минор(алгебраическое дополнение)
        /// </summary>
        /// <param name="row">Строка для вычеркивания</param>
        /// <param name="column">Столбец для вычеркивания</param>
        /// <returns>Новая матрица</returns>
        public Matrix GetMinor(int row, int column)
        {
            if (!IsSquare())
                throw new NonSquareMatrixException();

            var size = Rows;

            if (size < 3)
                throw new MatrixOperationException("Для поиска алгебраического дополнения нужна квадратная матрица размерностью больше 3");

            if (row < 0 || row >= size)
                throw new ArgumentException("Попытка доступа к несуществующей строке", "row");

            if (column < 0 || column >= size)
                throw new ArgumentException("Попытка доступа к несуществующему столбцу", "Column");

            return new Matrix(ArrayMatrix.GetMinor(data, row, column));
        }

        /// <summary>
        /// Получает след матрицы(сумму диагональных элементов)
        /// </summary>
        /// <returns>Значение суммы</returns>
        public double Trace()
        {
            if (!IsSquare())
                throw new NonSquareMatrixException();

            var diagonal = ArrayMatrix.GetDiagonal(data);
            return diagonal.Sum();
        }

        /// <summary>
        /// Клонирует указанную матрицу
        /// </summary>
        /// <returns>Клонированая матрица</returns>
        public object Clone()
        {
            return new Matrix(ArrayMatrix.Clone(data));
        }

        //сама матрица
        public double[][] data;

        //вренет массив-матрицу по ссылке, а не его клон
        /// <summary>
        /// Для получения ссылки на массив, который, по сути, является матрицей
        /// Метод нужен в то время, когда пользователь осознанно хочет получить ссылку. Также это может сэкономить время, когда пользователь не хочет получать клон матрицы.
        /// </summary>
        /// <returns>Массив</returns>
        public double[][] GetDataByReference()
        {
            return data;
        }

        //кнструктор
        /// <summary>
        /// Задание матрицы через зубчастый массив
        /// </summary>
        /// <param name="arr">Массив, клон которого, будет использоваться в качестве матрицы</param>
        public Matrix(double[][] arr)
        {
            data = (double[][])arr.Clone();
        }

        /// <summary>
        /// Задание матрицы через одномерный массив
        /// </summary>
        /// <param name="rows">Количеств строк</param>
        /// <param name="Columns">Количество столбцов</param>
        /// <param name="matrix">ОДномерный массив, в котором построчно записана матрица</param>
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
        /// Определение количества строк матрицы
        /// </summary>
        /// <returns>Количество строк</returns>
        public int Rows
        { get { return data.Length; } }

        /// <summary>
        /// Определение количества столбцов матрицы
        /// </summary>
        /// <returns>Количество столбцов</returns>
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
        /// <returns>Детерминант матрицы</returns>
        public double Determinant()
        {
            return DeterminantSercher.GetDeterminant(this);
        }

        /// <summary>
        /// Получение обратной матрицы
        /// </summary>
        /// <returns>Обратная матрица</returns>
        public Matrix Inverse()
        {
            var LFResult = LaverierFadeevaMethod.Solve(this, LaverierFadeevaSolvingOptions.OnlyInverseOfMatrix);

           return LFResult.InverseMatrix;
        }

        /// <summary>
        /// Решение системы матричным методом
        /// </summary>
        /// <param name="b">вектор после знака =</param>
        /// <returns></returns>
        public Vector Solve(Vector b)
        {
            return MatrixSolving.ByMatrixMathod(this, b);
        }

        //вложенный класс, отвечающий за создание матриц
        /*!
   \brief CreatMatrix
   \author Vlad Kovdrya
   \warning Только создание матриц

   Класс для создания матриц. С его помощью можно создать квадратную или прямоугольную матрицу на основе разнообразных входных параметров.
   */
        /// <summary>
        /// Фабрики для создания матриц
        /// </summary>
        public static class Create
        {
            /// <summary>
            /// Создает квадратную нулевую матрицу n*n
            /// </summary>
            /// <param name="n">Размер матрицы</param>
            /// <returns>Новая матрица</returns>
            public static Matrix New(int n)
            {
                return New(n, n);
            }

            /// <summary>
            /// Создает нулевую прямоугольную матрицу 
            /// </summary>
            /// <param name="columns">Количество колонок</param>
            /// <param name="rows">Количество рядков</param>
            /// <returns>Новая матрица</returns>

            public static Matrix New(int rows, int columns)
            {
                double[][] arr = ArrayMatrix.GetJaggedArray(rows, columns);
                return new Matrix(arr);
            }

            /// <summary>
            /// Создание матрицы на основе двумерного массива с коэффициентами
            /// </summary>
            /// <param name="MatrixArray">Двумерный массив</param>
            /// <returns>Матриц полученая из массива</returns>
            public static Matrix New(double[][] MatrixArray)
            {
                return new Matrix(MatrixArray);
            }

            /// <summary>
            /// На основе зубчастого массива с коэффициентами
            /// </summary>
            /// <param name="MatrixArray">Массив</param>
            /// <returns>Матрица полученая из массива</returns>
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
            /// <param name="n">Размер</param>
            /// <returns>Новая матрица</returns>
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
            /// <returns>Новая матрица</returns>
            public static Matrix Identity(int rows, int columns, double[] MatrixAsOneDimArray)
            {
                return new Matrix(rows, columns, MatrixAsOneDimArray);
            }

            /// <summary>
            /// Создает диагональную матрицу
            /// </summary>
            /// <param name="arr">Массив диагональных элементов</param>
            /// <returns>Новая матрица</returns>
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
            /// <returns>Новая матрица</returns>
            public static Matrix Diagonal(Vector v)
            {
                return Diagonal(v.data);    
            }

            /// <summary>
            /// Соединяет вектора в матрицу
            /// </summary>
            /// <param name="vectors">Список векторов</param>
            /// <returns>Новая матрица</returns>
            public static Matrix JoinVectors(List<double[]> vectors)
            {
                var ListLen = vectors.Count;

                if (ListLen < 1)
                    throw new ArgumentException("количество вектором должно быть больше 0", "vectors");

                else if (ListLen == 1)
                    return new Matrix(vectors[0].Length, 1, vectors[0]);

                int VLen = vectors[0].Length;
                for (int i = 1; i < ListLen; i++)
                    if (vectors[i].Length != VLen)
                        throw new VectorsJoinException();

                return new Matrix(ArrayMatrix.Join(vectors));
            }

            /// <summary>
            /// Соединяет вектора в матрицу
            /// </summary>
            /// <param name="vectors">Список векторов</param>
            /// <returns>Новая матрица</returns>
            public static Matrix JoinVectors(IEnumerable<Vector> vectors)
            {
                List<Vector> inputlst = vectors.ToList();
                int n = inputlst.Count;

                List<double[]> outLst = new List<double[]>();

                for (int i = 0; i < n; i++)
                    outLst.Add(inputlst[i].data);

                return JoinVectors(outLst);
            }
        }
        /// <summary>
        /// Создает диагональную матрицу
        /// </summary>
        /// <param name="v">Вектор диагонильных элементов</param>
        /// <returns>Новая матрица</returns>
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
