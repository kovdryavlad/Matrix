using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var coefs = new[] { 1, 0.0, 0, 3, 0, -11, -4, 2 };

            var r = Algebra.EquationSolver.GetRoots(coefs);
        }
    }
}
