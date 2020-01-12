using GeneticAlgorithm.Models;
using MainProject.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProject.ViewModel
{
    class ViewModelGeneticAlgorithm : INotifyPropertyChanged
    {

        public ViewModelGeneticAlgorithm()
        {
            Properties.PopulationSize = SaveState.Default.PopulationSize;
            Properties.MutationRate = SaveState.Default.MutationRate;
            Properties.CrossoverRate = SaveState.Default.CrossoverRate;
            Properties.GenerationSize = SaveState.Default.GenerationSize;

        }
        ~ViewModelGeneticAlgorithm()
        {
            SaveState.Default.PopulationSize = Properties.PopulationSize;
            SaveState.Default.MutationRate = Properties.MutationRate;
            SaveState.Default.CrossoverRate = Properties.CrossoverRate;
            SaveState.Default.GenerationSize = Properties.GenerationSize;
            SaveState.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        Command _work;
        public static PropertyModel.Properties Properties { get; set; } = new PropertyModel.Properties();
        Genetic_Algorithm ga;
        double f;
        int numberFunct;
        public Command WorkGeneticAlgorithm => _work ??
            (_work = new Command(async obj =>
            {
                double doubleRes;
                int intRes;
                if (!(double.TryParse(Properties.CrossoverRate.Replace(
                                    (CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ",") ? "," : ".",
                                    (CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ",") ? "." : ",")
                                    , out doubleRes)) || 
                   !(double.TryParse(Properties.MutationRate.Replace(
                                    (CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ",") ? "," : ".",
                                    (CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ",") ? "." : ","), out doubleRes)) ||
                   !(int.TryParse(Properties.PopulationSize, out intRes)) || !(int.TryParse(Properties.GenerationSize, out intRes)))
                {
                    MessageBox.Show("Данные введены некорректно.\nПроверьте правильность заполнения");
                }
                else
                {
                    ga = new Genetic_Algorithm(Double.Parse(Properties.CrossoverRate, CultureInfo.InvariantCulture), Double.Parse(Properties.MutationRate, CultureInfo.InvariantCulture),
                                                  Int32.Parse(Properties.PopulationSize), Int32.Parse(Properties.GenerationSize));

                    //отчистка полей для вывода результатов
                    Properties.BestFitness = string.Empty;
                    Properties.BestX = string.Empty;
                    Properties.BestY = string.Empty;
                    Properties.IntermediateValues = string.Empty;

                    var Item = Properties.TheActualFunction;

                    ga.Notify += ((a) => Properties.IntermediateValues += a + '\n');

                    switch (Item)
                    {
                        case 0:
                            numberFunct = 0;
                            ga.FitnessFunction = new GAFunction(TheActualFunction); 
                            break;
                        case 1:
                            numberFunct = 1;
                            ga.FitnessFunction = new GAFunction(TheActualFunction);   
                            break;
                    }

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
        public double TheActualFunction(double[] values)
        {
            double x = values[0];
            double y = values[1];
            
            switch (numberFunct)
            {
                case 0: f = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(9 * Math.PI * x) * Math.Sin(9 * Math.PI * y), 2); break;
                case 1: f = -(Math.Pow(x, 2) + Math.Pow(y, 2) - 4); break;
            }
            return f;
        }
    }
}
