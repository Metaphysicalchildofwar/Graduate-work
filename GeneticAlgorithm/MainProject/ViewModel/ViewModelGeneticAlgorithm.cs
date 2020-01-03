using GeneticAlgorithm.Models;
using MainProject.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProject.ViewModel
{
    class ViewModelGeneticAlgorithm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        Command _work;
        public static PropertyModel.Properties Properties { get; set; } = new PropertyModel.Properties();

        Genetic_Algorithm ga;

        //временно тут
        public static double theActualFunction(double[] values)
        {
            double x = values[0];
            double y = values[1];

            //double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(9 * Math.PI * x) * Math.Sin(9 * Math.PI * y), 2);
            double f1 = -(Math.Pow(x, 2) + Math.Pow(y, 2) - 4);
            return f1;
        }

        //пока что синхронная работа
        public Command WorkGeneticAlgorithm => _work ??
           (_work = new Command(async obj =>
           {
               double doubleRes;
               int intRes;

               if (!(double.TryParse(Properties.CrossoverRate, out doubleRes)) || !(double.TryParse(Properties.MutationRate, out doubleRes)) ||
                   !(int.TryParse(Properties.PopulationSize, out intRes)) || !(int.TryParse(Properties.GenerationSize, out intRes)))
               {
                   MessageBox.Show("Данные введены некорректно.\nПроверьте правильность заполнения");
               }
               else 
               {
                   ga = new Genetic_Algorithm(Double.Parse(Properties.CrossoverRate), Double.Parse(Properties.MutationRate),
                                              Int32.Parse(Properties.PopulationSize), Int32.Parse(Properties.GenerationSize));

                   //отчистка полей для вывода результатов
                   Properties.BestFitness = string.Empty;
                   Properties.BestX = string.Empty;
                   Properties.BestY = string.Empty;
                   Properties.IntermediateValues = string.Empty;

                   ga.Notify += ((a) => Properties.IntermediateValues += a + '\n');
                   ga.FitnessFunction = new GAFunction(theActualFunction);
                   ga.Elitism = true;
                   await Task.Run(() => ga.WorkGeneticAlgorithm());
                   double[] values;
                   double fitness;

                   ga.GetBest(out values, out fitness);

                   Properties.BestFitness += fitness.ToString();
                   Properties.BestX += values[0].ToString();
                   Properties.BestY += values[1].ToString();
               }
           }
           ));

    }
}
