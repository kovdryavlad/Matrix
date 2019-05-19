using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Algebra
{
    public class EquationSolver
    {
        public static double[] GetRoots(double[] сoefficients, double precision = 0.00000000000001, double InitialStep = 1, double MinStep = 0.25, SolverOptions options = SolverOptions.StopIfNumberOfRootsCeasedIncrease)
        {
            RootsLimits Limits = GetRootsLimits(сoefficients);

            //отделение корнеЙ
            double h = InitialStep;
            Interval[] IntervalsWithNormalStep;
            Interval[] IntervalsWithHalfOfStep;

            //лямбда полинома с коефициентами
            var lambda = FormLambdaForPolinom(сoefficients.ToArray());

            do
            {
                IntervalsWithNormalStep = GetIntervalsWithRoots(lambda, h, Limits.NLower, Limits.NUpper, Limits.PLower, Limits.PUpper);
                IntervalsWithHalfOfStep = GetIntervalsWithRoots(lambda, h / 2, Limits.NLower, Limits.NUpper, Limits.PLower, Limits.PUpper);
                h /= 2;
            }
            while (IntervalsWithNormalStep.Length != IntervalsWithHalfOfStep.Length);

            //уточнение корней
            double[] roots = ClarifyRoots(lambda, IntervalsWithHalfOfStep, precision);

            return roots;
        }

        public static double[] GetRoots(double[] сoefficients, int numberOfroots, int startIntervalsCount = 64, double precision = 0.00000001)
        {
            RootsLimits Limits = GetRootsLimits(сoefficients);
            List<Interval> intervals = new List<Interval>();
            
            //лямбда полинома с коефициентами
            var lambda1 = FormLambdaForPolinom(сoefficients.ToArray());
            var lambda2 = FormLambdaForPolinom(сoefficients.ToArray());

            Func<double, double, Func<double, double>, List<Interval>> getIntervals = (a, b, polinom) =>
             {
                 double h = (b - a) / startIntervalsCount;

                 double lBrd = 0;
                 double rBrd = 0;

                 List<Interval> result = new List<Interval>();

                //для отрицательной части
                for (double i = a; i < b; i = rBrd)
                 {
                     lBrd = i;
                     rBrd = i + h;
                     if (polinom(lBrd) * polinom(rBrd) < 0)
                         result.Add(new Interval(lBrd, rBrd));
                 }

                 return result;
             };

            do
            {
                intervals.Clear();

                Task<List<Interval>> t1 = Task<List<Interval>>.Run(() => getIntervals(Limits.NLower, Limits.NUpper, lambda1));
                Task<List<Interval>> t2 = Task<List<Interval>>.Run(() => getIntervals(Limits.PLower, Limits.PUpper, lambda2));
                
                Task.WaitAll((new[] { t1, t2 }));
                
                intervals.AddRange(t1.Result);
                intervals.AddRange(t2.Result);

                startIntervalsCount *= 2;
            }
            while (numberOfroots != intervals.Count);


            //уточнение корней
            double[] roots = ClarifyRoots(lambda1, intervals.ToArray(), precision);

            return roots;
        }


        private static RootsLimits GetRootsLimits(double[] сoefficients)
        {
            var Limits = new RootsLimits();

            //то же что и умножить на икс в высшей степени и подставить 1/x
            var ReverceCoefs = сoefficients.Reverse().ToArray(); ; //Для фи1 и фи3

            //поиск границ корней
            Limits.PUpper = UpperLimitSearcher.GetUpperLimit(сoefficients);
            Limits.PLower = 1d / UpperLimitSearcher.GetUpperLimit(ReverceCoefs);
            Limits.NLower = -UpperLimitSearcher.GetUpperLimit(SubstituteNegativeArgument(сoefficients)); //минус там не зря)
            Limits.NUpper = -1d / UpperLimitSearcher.GetUpperLimit(SubstituteNegativeArgument(ReverceCoefs));
            return Limits;
        }

        //подставить -x
        private static double[] SubstituteNegativeArgument(double[] coefs)
        {
            var clone = (double[])coefs.Clone();

            int n = clone.Length;

            if (n % 2 == 0) //старшая степень непарная
                for (int i = 0; i < n - 1; i = i + 2)
                    clone[i] = -clone[i];
            else
                 for (int i = 1; i < n - 1; i = i + 2)
                    clone[i] = -clone[i];

            return clone;
        }

        //Поиск интервалов на которых есть корни
        private static Interval[] GetIntervalsWithRoots(Func<double, double>polinom,double h, double nLower, double nUpper, double pLower, double pUpper)
        {
            var IntervalLst = new List<Interval>();

            double lBrd = 0;
            double rBrd = 0;

            //для отрицательной части
            for (double i = nLower; i < nUpper; i = rBrd)
            {
                lBrd = i;
                rBrd = i + h;
                if (polinom(lBrd) * polinom(rBrd) < 0)
                    IntervalLst.Add(new Interval(lBrd, rBrd));
            }

            //для положительной части
            for (double i = pLower; i < pUpper; i = rBrd)
            {
                lBrd = i;
                rBrd = i + h;
                if (polinom(lBrd) * polinom(rBrd) < 0)
                    IntervalLst.Add(new Interval(lBrd, rBrd));
            }

            return IntervalLst.ToArray();
        }

        //формирование уравнения для поиска собственных значений
        private static Func<double, double> FormLambdaForPolinom(double[] coefs)
        {
            var InputLength = coefs.Length;

            int n = InputLength-1;       //для степеней и an
            ParameterExpression Xparam = Expression.Parameter(typeof(double), "x");

            ConstantExpression pow = Expression.Constant((double)n);

            Expression ResultExpression = Expression.Multiply(Expression.Power(Xparam, pow), Expression.Constant(coefs[0]));

            for (int i = 1; i < InputLength; i++)
            {
                n--;
                pow = Expression.Constant((double)n);
                ResultExpression = Expression.Add(ResultExpression, Expression.Multiply(Expression.Power(Xparam, pow), Expression.Constant(coefs[i])));
            }

            LambdaExpression lambdaExpression = Expression.Lambda(ResultExpression, Xparam);
            var CompiledLambda = (Func<double, double>)lambdaExpression.Compile();

            return CompiledLambda;
        }

        //уточнение корней
        private static double[] ClarifyRoots(Func<double, double>f, Interval[] intervals, double eps)
        {
            var length = intervals.Length;

            double[] roots = new double[length];

            for (int i = 0; i < length; i++)
                roots[i] = BisectionMethod(f, intervals[i].leftBorder, intervals[i].rightBorder, eps);

            return roots;
        }


        //метод половинного деления
        private static double BisectionMethod(Func<double, double>f, double A, double B, double epsilon)
        {
            double a = A;
            double b = B;
            int IterationCounter = 0;

            double result = (a + b) / 2;
            while (Math.Abs(b - a) >= epsilon)
            {
                if (f(a) * f(result) < 0)
                    b = result;
                else
                    a = result;

                result = (a + b) / 2;

                IterationCounter++;
            }
            System.Diagnostics.Trace.WriteLine(String.Format("Количество итераций в  методе бисекции для корня {0:0.000}: {1}", result, IterationCounter));
            return result;
        }

    }

    public enum SolverOptions
    {
        ContinueToMinStep,                  //продоллжать до минимального шага
        StopIfNumberOfRootsCeasedIncrease   //остановиться если количество корней перестало увеличиваться
    }

    public class RootsLimits
    {
        public double NLower { get; internal set; } //N2
        public double NUpper { get; internal set; } //N3
        public double PLower { get; internal set; } //N1
        public double PUpper { get; internal set; } //N0
    }

    //интервалы корней
    public class Interval
    {
        internal double leftBorder;
        internal double rightBorder;

        public Interval(double leftBorder, double rightBorder)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
        }

        public override string ToString()
        {
            return String.Format("({0:0.0000}; {1:0.0000})", leftBorder, rightBorder);
        }
    }
}
