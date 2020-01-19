using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GeneticAlgorithm.ConsoleVersion
{
    class Program
    {
        static void Main()
        {
            Genetic_Algorithm ga = new Genetic_Algorithm(0.8, 0.05, 300, 1000);
            ga.Notify += ((a) => Console.WriteLine(a));
            ga.FitnessFunction = new GAFunction(theActualFunction);
            ga.Elitism = true;
            ga.WorkGeneticAlgorithm();
            double[] values;
            double fitness;

            ga.GetBest(out values, out fitness);

            Console.WriteLine(fitness);
            for (var i = 0; i < values.Length; i++)
            {
                Console.WriteLine(values[i]);
            }

            Console.Read();
        }
        public static double theActualFunction(double[] values, int n)
        {

            double x = values[0];
            double y = values[1];

            //double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(9 * Math.PI * x) * Math.Sin(9 * Math.PI * y), 2);
            double f1 = -(Math.Pow(x, 2) + Math.Pow(y, 2)-4);
            return f1;
        }
    }
}
