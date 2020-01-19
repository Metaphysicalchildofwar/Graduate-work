using GeneticAlgorithm.Models;
using MainProject.Commands;
using MainProject.WorkModels;
using Models;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MainProject.ViewModel
{
    class ViewModelGeneticAlgorithm : INotifyPropertyChanged
    {

        public ViewModelGeneticAlgorithm()
        {
            Properties.PopulationSize = SaveState.Default.PopulationSize;
            Properties.GenerationSize = SaveState.Default.GenerationSize;
            Properties.Availability = true;

        }
        ~ViewModelGeneticAlgorithm()
        {
            SaveState.Default.PopulationSize = Properties.PopulationSize;
            SaveState.Default.GenerationSize = Properties.GenerationSize;
            SaveState.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        Command _work;
        
        ChoiseFunction function;
        public static PropertyModel.Properties Properties { get; set; } = new PropertyModel.Properties();

        Genetic_Algorithm ga;
        internal static int numberFunct;
        public Command WorkGeneticAlgorithm => _work ??
            (_work = new Command(async obj =>
            {
                if (Properties.Availability == true)
                {
                    Properties.Availability = false;
                }

                int intRes;
                function = new ChoiseFunction();
                if (!(int.TryParse(Properties.PopulationSize, out intRes)) || !(int.TryParse(Properties.GenerationSize, out intRes)))
                {
                    MessageBox.Show("Данные введены некорректно.\nПроверьте правильность заполнения");
                    Properties.Availability = true;
                }
                else
                {
                    ga = new Genetic_Algorithm(Int32.Parse(Properties.PopulationSize), Int32.Parse(Properties.GenerationSize));

                    //отчистка полей для вывода результатов
                    Properties.BestFitness = string.Empty;
                    Properties.BestX = string.Empty;
                    Properties.BestY = string.Empty;
                    Properties.IntermediateValues = string.Empty;

                    var Item = Properties.TheActualFunction;
                    Properties.Points = new ObservableCollection<DataPoint>();
                    ga.Notify += ((a) => Properties.IntermediateValues += a + '\n');

                    //выбор функции из ComboBox
                    switch (Item)
                    {
                        case 0:
                            numberFunct = 0;
                            break;
                        case 1:
                            numberFunct = 1;                        
                            break;
                        case 2:
                            numberFunct = 2;
                            break;
                        case 3:
                            numberFunct = 3;
                            break;
                    }

                    //работа алгоритма
                    ga.FitnessFunction = new GAFunction(function.TheActualFunction);
                    ga.Elitism = true;
                    await Task.Run(() => ga.WorkGeneticAlgorithm());

                    //получить первое и последнее значение для отображения промежутка на оси ОУ
                    Properties.FirstValue = ga.Datas.First().Y;
                    Properties.LastValue = Math.Round(ga.Datas.Last().Y);

                    //получить точки для чертежа
                    Properties.Points = ga.Datas;

                    double[] values;
                    double fitness;

                    ga.GetBest(out values, out fitness);

                    Properties.BestFitness += fitness.ToString();
                    Properties.BestX += values[0].ToString();
                    Properties.BestY += values[1].ToString();

                    Properties.Availability = true;
                }
            }));
            

    }
}
