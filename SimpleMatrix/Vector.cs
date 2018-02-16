using System;
using System.Linq;

namespace SimpleMatrix
{
    //TODO
    //создать общую ошибку для векторов
    //понаследовать от нее ошибки сложения и вычитания векторов или ошибку разници длин вектора

    //вся работа с векторами
    public class Vector:ICloneable
    {
        //единичный ли?
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
        /// Возвращает нормированный вектор
        /// </summary>
        public Vector Normilize()
        {
            //норма вектора
            var norm = GetNormOfVector();

            //нормированный вектор
            var normilized = data.Select(v => v / norm).ToArray();

            return Vector.Create.New(normilized);
        }

        public object Clone()
        {
            return new Vector((double[])data.Clone());
        }

        //переопределение умножения
        public static Vector operator *(double k, Vector V)
        {
            return new Vector(ArrayVector.MultiplyOnK(k, V.data));
        }
        
        //переопределение деления
        public static Vector operator /(Vector V, double k)
        {
            return new Vector(ArrayVector.DivideOnK(V.data, k));
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.CanAdd(b))
                return new Vector(ArrayVector.Sum(a.data,b.data));
            else
                throw new Exception();
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.CanAdd(b))
                return new Vector(ArrayVector.Substruct(a.data, b.data));
            else
                throw new Exception();
        }

        //можно ли слаживать
        public bool CanAdd(Vector b)
        {
            if (data.Length == b.Length)
                return true;

            return false;
        }
        
        //сам вектор
        internal double[] data;

        //конструктор
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
        /// Возвращает или устанавливает элемент матрицы
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
        public static class Create
        {
            /// <summary>
            /// Создает нулевой вектор длинной  n
            /// </summary>
            public static Vector New(int n)
            {
                var arr = new double[n];

                return new Vector(arr);
            }

            /// <summary>
            /// Создает ненулевой вектор из готовых коэффициентов 
            /// </summary>
            public static Vector New(double[] array)
            {
                return new Vector(array);
            }

            /// <summary>
            /// Вернет единичный вектор (i элемент равен 1, остальные - 0)
            /// </summary>
            /// <param name="n">Длинна вектора</param>
            /// <param name="i">Элемент, который установится в 1</param>
            /// <returns></returns>
            public static Vector Unit(int n, int i)
            {
                Vector A = New(n);
                A[i] = 1;

                return A;
            }
        }

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
