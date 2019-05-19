using System;
using System.Linq;

namespace SimpleMatrix
{
    //TODO
    //создать общую ошибку для векторов
    //понаследовать от нее ошибки сложения и вычитания векторов или ошибку разници длин вектора

    //вся работа с векторами
    /*!
 \brief Vector
 \author Vlad Kovdrya
 \warning Работа только с векторами

 Класс для работы с векторами. */
    [Serializable]
    public class Vector:ICloneable
    {
        //единичный ли?
        /// <summary>
        /// Проверка вектора на единичность
        /// </summary>
        /// <param name="A">Vector</param>
        /// <returns>Bool answer</returns>
        internal static bool IsUnit(Vector A)
        {
            double Sum = A.data.Sum();

            if (Sum == 1)
                return true;

            return false;
        }
        
        //норма вектора
        private double GetNormOfVector()
        {
            //подкорневое выражение
            double S = data.Aggregate(0d, (sum, element) => sum + Math.Pow(element, 2));

            return Math.Sqrt(S);
        }

        /// <summary>
        /// Нахождение нормированного вектора
        /// </summary>
        /// <returns>Нормированный вектор</returns>
        public Vector Normilize()
        {
            //норма вектора
            var norm = GetNormOfVector();

            //нормированный вектор
            var normilized = data.Select(v => v / norm).ToArray();

            return Vector.Create.New(normilized);
        }
        /// <summary>
        /// Клонирование вектора
        /// </summary>
        /// <returns>Вектор</returns>
        public object Clone()
        {
            return new Vector((double[])data.Clone());
        }

        public double[] GetCloneOfData()
        {
            return (double[])data.Clone();
        }

        //переопределение умножения
        /// <summary>
        /// Умножение вектора на число
        /// </summary>
        /// <param name="k">Число</param>
        /// <param name="V">Вектор</param>
        /// <returns>Новый вектор</returns>
        public static Vector operator *(double k, Vector V)
        {
            return new Vector(ArrayVector.MultiplyOnK(k, V.data));
        }

        //переопределение деления
        /// <summary>
        /// Деление вектора на число
        /// </summary>
        /// <param name="k">Число</param>
        /// <param name="V">Вектор</param>
        /// <returns>Новый вектор</returns>
        public static Vector operator /(Vector V, double k)
        {
            return new Vector(ArrayVector.DivideOnK(V.data, k));
        }
        /// <summary>
        /// Сумирование векторов
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Новый вектор</returns>
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.CanAdd(b))
                return new Vector(ArrayVector.Sum(a.data,b.data));
            else
                throw new VectorsSizeException();
        }

        public static Vector operator +(Vector a, double b)
        {
            var aclone = (Vector)a.Clone();

            var array = aclone.data;
            for (int i = 0; i < array.Length; i++)
                array[i] += b;

            return aclone;
        }

        /// <summary>
        /// Разность векторов
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Новый вектор</returns>
        public static Vector operator -(Vector a, Vector b)
        {
            if (a.CanAdd(b))
                return new Vector(ArrayVector.Substruct(a.data, b.data));
            else
                throw new VectorsSizeException();
        }

        public static Vector operator -(Vector a, double b)
        {
            var aclone = (Vector)a.Clone();

            var array = aclone.data;
            for (int i = 0; i < array.Length; i++)
                array[i] -= b;

            return aclone;
        }

        /// <summary>
        /// Умножение двух векторов(Подразумевается умножение вектора-столбца на вектор-строку, но приводить их к такому виду не нужно)
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Новый вектор</returns>
        public static double operator *(Vector a, Vector b)
        {
            if (a.CanAdd(b))
                return ArrayVector.Multiply(a.data, b.data);
            else
                throw new VectorsSizeException();
        }

        public static Vector operator *(Vector a, Matrix b)
        {
            var aMatrix = new Matrix(new double[1][]{ a.data });
            if (aMatrix.CanMultiply(b)) {

                var resultMatrix = aMatrix * b;

                return new Vector(resultMatrix.data[0]);
            }
                
            else
                throw new MultiplyingVectorOnmatrixException();
        }



        //можно ли слаживать
        /// <summary>
        /// Проверка вектора перед сложнием
        /// </summary>
        /// <param name="b">Вектор</param>
        /// <returns>Bool</returns>
        public bool CanAdd(Vector b)
        {
            if (data.Length == b.Length)
                return true;

            return false;
        }
        
        //сам вектор
        internal double[] data;

        //конструктор
        /// <summary>
        /// Констректор вектора
        /// </summary>
        /// <param name="array">Массив</param>
        public Vector(double[] array)
        {
            data = (double[])array.Clone();
        }

        /// <summary>
        /// Возвращает длинну вектора
        /// </summary>
        public int Length { get { return data.Length; } }

        //индексатор
        /// <summary>
        /// Возвращает или устанавливает элемент матрицы (индексатор)
        /// </summary>
        /// <param name="i">Индекс</param>
        public double this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }

        //фабрики - вложенный класс
        /// <summary>
        /// Фабрики для создания векторов
        /// </summary>
          /*!
 \brief VectorCreat
 \author Vlad Kovdrya
 \warning Работа только с векторами

 Класс для создания векторов. */
        public static class Create
        {
            /// <summary>
            /// Создание нулевого вектора
            /// </summary>
            /// <param name="n">Длинна</param>
            /// <returns>Вектор</returns>
            public static Vector New(int n)
            {
                var arr = new double[n];

                return new Vector(arr);
            }

            /// <summary>
            /// Создание ненулевого вектора из готовых коеффициентов
            /// </summary>
            /// /// <param name="array">Массив</param>
            /// <returns>Вектор</returns>
            public static Vector New(double[] array)
            {
                return new Vector(array);
            }

            /// <summary>
            /// Преобразование. Возвращает единичный вектор (i элемент равен 1, остальные - 0)
            /// </summary>
            /// <param name="n">Длинна вектора</param>
            /// <param name="i">Элемент, который установится в 1</param>
            /// <returns>Вектор</returns>
            public static Vector Unit(int n, int i)
            {
                Vector A = New(n);
                A[i] = 1;

                return A;
            }
        }
        /// <summary>
        /// Запись вектора в строку
        /// </summary>
        /// <returns>Строка</returns>
        public override string ToString()
        {
            string s = "(";

            var length = data.Length;

            for (int i = 0; i < length; i++)
              s += String.Format("{0:0.0000}{1}", data[i], i < length - 1 ? "; " : ")");
            
            return s;
        }
    }
}
