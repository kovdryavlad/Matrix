using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMatrix;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleMatrix.Matrix m = new SimpleMatrix.Matrix(3, 3, new[] {

                 2,  -1,  1.0,
                -1,   2, -1,
                 0,   0,  1

            });

            var mClone = (SimpleMatrix.Matrix)m.Clone();
            ;

            double[][] arr = new[] { new[] { 0.0, 1 }, new[] { 2.0, 3 } };
            var arrClone = (double[][])arr.Clone();
            //SimpleMatrix.Matrix a = new SimpleMatrix.Matrix(3, 3, new[] {2d,3,4,1,3,3,0,4,4});
            //Vector v = new Vector(new double[] { 20, 16, 20d });
            //var r = a.Solve(v);
            ;
            /*
            var v1 =new[] { 1d, 2, 3 };
            var v2 =new[] { 5d, 6, 7 };

            Vector aa = new Vector(v1);
            Vector ab = new Vector(v2);
            var T = 12*aa;
            var T2 = ab * aa;
            ;
            //var joined = ArrayMatrix.Join(new[] { v1, v2 }.ToList());
            //SimpleMatrix.Matrix matrix = new SimpleMatrix.Matrix(joined);
            ;
            //SimpleMatrix.Matrix ghj = new SimpleMatrix.Matrix(3,2, new []{1,2,3,4,5,6.0}) ;

            
            SimpleMatrix.Matrix t = SimpleMatrix.Matrix.Create.New(2);
            t[0, 0] = 3;
            t[0, 1] = 4;
            t[1, 0] = 5;
            t[1, 1] = 6;
            
           
            */

            /*
            SimpleMatrix.Matrix t = new SimpleMatrix.Matrix(7,7, new[]{
              1.6,       7,     10,    330,    -9,     25,    1.3, 
                1,       9,      7,     -7,     0,     11,    457,
                2,       6,    -10,     33,    12,      7,     23,
              2.5,       0,     56,     89,    45,     66,     -1,
                3,     0.5,    567,     12,   -90,     34,     12,
                3,       0,     67,     -8,     0,      0,      0,
               11,       0,      3,    0.3,     0,      0,      0
            });
            //var r = t.GetRow(0);
            //var u = t[0, 3];
            var det = t.Determinant();
            ;
            */

            SimpleMatrix.Matrix t = new SimpleMatrix.Matrix(3,3, new[]{
              1.0, 2, 3,
                3, 4, 5,
                1, 3, 3
            });

            var det = t.Determinant();
            var det2 = t.Determinant();
            ;

            /*
            t[0, 0] = 1;
            t[0, 1] = 2;
            t[0, 2] = 3;

            t[1, 0] = 0;
            t[1, 1] = 5;
            t[1, 2] = 6;

            t[2, 0] = 0;
            t[2, 1] = 8;
            t[2, 2] = 9;  
            */

            /*
            var t1 = DateTime.Now;
            var d = t.Determinant();
            var t2 = DateTime.Now;
            Console.WriteLine(d);
            Console.WriteLine("Time:{0}", (t2 - t1).TotalSeconds);
            // SimpleMatrix.Matrix B = SimpleMatrix.Matrix.Create.New(3, 4);
            */

            /*
            SimpleMatrix.Matrix A = SimpleMatrix.Matrix.Create.New(4,4);
            A[0, 0] = 2.2;
            A[0,1]= 1;
            A[0,2]= 0.5;
            A[0, 3] = 2;

            A[1, 0] = 1;
            A[1, 1] = 1.3;
            A[1, 2] = 2;
            A[1, 3] = 1;

            A[2,0]=0.5;
            A[2,1]=2;
            A[2,2]=0.5;
            A[2, 3] = 1.6;
            
            A[3, 0] = 2;
            A[3, 1] = 1;
            A[3, 2] = 1.6;
            A[3, 3] = 2;
            

            var res = LaverierFadeevaMethod.Solve(A, LaverierFadeevaSolvingOptions.FullSolving);
            var a = -11;
            */

            /*
            int s = 1000;
            
            SimpleMatrix.Matrix A = SimpleMatrix.Matrix.Create.New(s);

            SimpleMatrix.Matrix B = SimpleMatrix.Matrix.Create.New(s);

            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    B[i,j] = A[i, j] = j;
                }
            }
            
            
            
            
            var c = A;
            var d = B;
            DateTime z = DateTime.Now;
            var v = ArrayMatrix.MultiplyN3Transpose(c, d);
            DateTime z2 = DateTime.Now;
            Console.WriteLine((z2 - z).TotalSeconds);
            
            /*
            //var c = new[] { -10.0, -10, -10, -10 };
            var c = new[] { 10.0, 10, 10, 10 };
            
            var a = new[] { 1.0, 2, 4, 4 };
            var b = new[] { 2.0, 6, 2, 2 };
           // var f = new[] { -18.0, -40,-46, -54 };
           var f = new[] { 18.0, 40, 46, 54 };

            var solve = TridiagonalMatrixSolving.CyclycWithoutMinuses(a, c, b, f);
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine(solve[i]);
            }
            */
            //Console.ReadLine();
        }

        private static void DOSMTH(SimpleMatrix.Matrix t)
        {
            t = t.SwapRows(0, 1);
        }
    }
}
