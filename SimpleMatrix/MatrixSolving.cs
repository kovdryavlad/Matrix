using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMatrix
{
    class MatrixSolving
    {
        public static Vector ByMatrixMathod(Matrix matrix, Vector b)
        {
            if (!matrix.IsSquare())
                throw new NonSquareMatrixException();

            int MatrixSize = matrix.Rows;
            int VectorSize = b.Length;

            if (MatrixSize != VectorSize)
                throw new MatrixSolvingExcepton();

            double determinant = matrix.Determinant();

            if (Math.Round(determinant, 8) == 0)
                throw new DeterminantNullException();

            //обратная матрица
            Matrix inverse = matrix.Inverse();

            return inverse.MultiplyOnVectorColumn(b);
        }
    }
}
