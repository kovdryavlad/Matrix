using System;

namespace SimpleMatrix
{
    //корневая ошибка для матриц
    public class MatrixException : Exception
    {
        public MatrixException()
            :base("Ошибка матрицы")
        {

        }

        public MatrixException(string message)
            :base("Ошибка матрицы\n" + message)
        {

        }
    }

    public class MatrixInitializationException:MatrixException
    {
        public MatrixInitializationException()
            :base("Ошибка инициализации! Row*Column должно равняться длинне массива")
        {

        }
    }

    public class DeterminantException:MatrixException
    {
        public DeterminantException()
            :base("При поске детерминанта, на одном из шагов, был получен столбец в котором содержатся одни нули.")
        {

        }
    }

    public class MatrixOperationException : MatrixException
    {
        public MatrixOperationException()
            : base("Невозможно выполнить операцию")
        {

        }

        public MatrixOperationException(string message)
            : base("Невозможно выполнить операцию\n" + message)
        {

        }
    }

    public class MatrixAddException : MatrixOperationException
    {
        public MatrixAddException()
            : base("Невозможно выполнить сложение матриц, поскольку их размеры не совпадают")
        {

        }
    }

    public class MatrixDifferenceException : MatrixOperationException
    {
        public MatrixDifferenceException()
            : base("Невозможно выполнить вычитание матриц, поскольку их размеры не совпадают")
        {

        }
    }

    public class MatrixMultiplyException : MatrixOperationException
    {
        public MatrixMultiplyException()
            : base("Неподходящие размеры для умножения")
        {

        }
    }

    public class NonExtistRowException : MatrixOperationException
    {
        public NonExtistRowException()
            : base("Указанная строка не существует!")
        {

        }
    }

    public class NonExtistColumnException : MatrixOperationException
    {
        public NonExtistColumnException()
            : base("Указанный столбец не существует!")
        {

        }
    }

    public class NonSquareMatrixException : MatrixOperationException
    {
        public NonSquareMatrixException()
            :base("Для выполнения заданной операции матрица должна быть квадратной")
        {

        }
    }
}
