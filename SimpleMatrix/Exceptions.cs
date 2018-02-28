using System;

namespace SimpleMatrix
{
    //корневая ошибка для матриц
    /// <summary>
    /// Корневая ошибка для матриц
    /// </summary>
     /*!
   \brief  Ошибки
   \author Vlad Kovdrya

   Класс всевозможных ошибок */
    public class MatrixException : Exception
    {
        /// <summary>
        /// Корневая ошибка для матриц: ошибка матрицы
        /// </summary>
        public MatrixException()
            :base("Ошибка матрицы")
        {

        }
        /// <summary>
        /// Корневая ошибка для матриц: ошибка матрицы с дополнительным сообщением
        /// </summary>
        public MatrixException(string message)
            :base("Ошибка матрицы\n" + message)
        {

        }
    }
    /// <summary>
    /// Ошибка инициализации
    /// </summary>
    public class MatrixInitializationException:MatrixException
    {
        /// <summary>
        /// Ошибка инициализации: количество столбцов на количество строк должно быть равно длинне массива
        /// </summary>
        public MatrixInitializationException()
            :base("Ошибка инициализации! Row*Column должно равняться длинне массива")
        {

        }
    }
    /// <summary>
    /// Ошибка при поиске детерминанта
    /// </summary>
    public class DeterminantException:MatrixException
    {
        /// <summary>
        /// Ошибка при поиске детерминанта: на одном из шагов, был получен столбец в котором содержатся одни нули
        /// </summary>
        public DeterminantException()
            :base("При поске детерминанта, на одном из шагов, был получен столбец в котором содержатся одни нули.")
        {

        }
    }
    /// <summary>
    /// Ошибка операции над матрицей
    /// </summary>
    public class MatrixOperationException : MatrixException
    {
        /// <summary>
        /// Ошибка операции над матрицей: невозможно выполнить
        /// </summary>
        public MatrixOperationException()
            : base("Невозможно выполнить операцию")
        {

        }
        /// <summary>
        /// Ошибка операции над матрицей: невозможно выполнить. Имеет дополнительное сообщение
        /// </summary>
        public MatrixOperationException(string message)
            : base("Невозможно выполнить операцию\n" + message)
        {

        }
    }
    /// <summary>
    /// Ошибка сложения матриц
    /// </summary>
    public class MatrixAddException : MatrixOperationException
    {
        /// <summary>
        /// Ошибка сложения матриц: размеры матриц не совпадают
        /// </summary>
        public MatrixAddException()
            : base("Невозможно выполнить сложение матриц, поскольку их размеры не совпадают")
        {

        }
    }

    /// <summary>
    /// Ошибка вычитания матриц
    /// </summary>
    public class MatrixSubstructException : MatrixOperationException
    {
        /// <summary>
        /// Ошибка вычитания матриц: размеры  матриц не совпадают
        /// </summary>
        public MatrixSubstructException()
            : base("Невозможно выполнить вычитание матриц, поскольку их размеры не совпадают")
        {
            
        }
    }
    /// <summary>
    /// Ошибка умножения матриц
    /// </summary>
    public class MatrixMultiplyException : MatrixOperationException
    {
        /// <summary>
        /// Ошибка умножения матриц: Неподходящие размеры матриц
        /// </summary>
        public MatrixMultiplyException()
            : base("Неподходящие размеры для умножения")
        {

        }
    }
    /// <summary>
    /// Ошибка строки
    /// </summary>
    public class NonExtistRowException : MatrixOperationException
    {
        /// <summary>
        /// Ошибка строки: указанная строка не существует
        /// </summary>
        public NonExtistRowException()
            : base("Указанная строка не существует!")
        {

        }
    }
    /// <summary>
    /// Ошибка столбца
    /// </summary>
    public class NonExtistColumnException : MatrixOperationException
    {
        /// <summary>
        /// Ошибка столбца: казанный столбец не существует
        /// </summary>
        public NonExtistColumnException()
            : base("Указанный столбец не существует!")
        {

        }
    }
    /// <summary>
    /// Ошибка столбца: указанный столбец не существует
    /// </summary>
    public class NonSquareMatrixException : MatrixOperationException
    {
        public NonSquareMatrixException()
            :base("Для выполнения заданной операции матрица должна быть квадратной")
        {

        }
    }
}
