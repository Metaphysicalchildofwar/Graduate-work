using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.PropertyModel
{
    class Properties : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void InvokePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        
        private string _crossoverRate;
        private string _mutationRate;
        private string _populationSize;
        private string _generationSize;
        private string _genomeSize;

        private string _bestFitness;
        private string _bestX;
        private string _bestY;

        private string _intermediateValues;

        /// <summary>
        /// Вывод в TextBox промежуточный фитнесс значений
        /// </summary>
        public string IntermediateValues
        {
            get => _intermediateValues;
            set
            {
                if (_intermediateValues != value)
                {
                    _intermediateValues = value;
                    InvokePropertyChanged(nameof(IntermediateValues));
                }
            }
        }

        /// <summary>
        /// Вывод в TextBox лучшего x
        /// </summary>
        public string BestX
        {
            get => _bestX;
            set
            {
                if (_bestX != value)
                {
                    _bestX = value;
                    InvokePropertyChanged(nameof(BestX));
                }
            }
        }

        /// <summary>
        /// Вывод в TextBox лучшего y
        /// </summary>
        public string BestY
        {
            get => _bestY;
            set
            {
                if (_bestY != value)
                {
                    _bestY = value;
                    InvokePropertyChanged(nameof(BestY));
                }
            }
        }

        /// <summary>
        /// Вывод в TextBox лучшего приближения
        /// </summary>
        public string BestFitness
        {
            get => _bestFitness;
            set
            {
                if (_bestFitness != value)
                {
                    _bestFitness = value;
                    InvokePropertyChanged(nameof(BestFitness));
                }
            }
        }

        /// <summary>
        /// Свойство для изменения TextBox'а частоты скрещиваний
        /// </summary>
        public string CrossoverRate
        {
            get => _crossoverRate;
            set
            {
                if(_crossoverRate != value)
                {
                    _crossoverRate = value;
                    InvokePropertyChanged(nameof(CrossoverRate));
                }
            }
        }

        /// <summary>
        /// Свойство для изменения TextBox'а частоты мутаций
        /// </summary>
        public string MutationRate
        {
            get => _mutationRate;
            set
            {
                if (_mutationRate != value)
                {
                    _mutationRate = value;
                    InvokePropertyChanged(nameof(MutationRate));
                }
            }
        }

        /// <summary>
        /// Свойство для изменения TextBox'а размера популяции
        /// </summary>
        public string PopulationSize
        {
            get => _populationSize;
            set
            {
                if (_populationSize != value)
                {
                    _populationSize = value;
                    InvokePropertyChanged(nameof(PopulationSize));
                }
            }
        }

        /// <summary>
        /// Свойство для изменения TextBox'а величины поколения
        /// </summary>
        public string GenerationSize
        {
            get => _generationSize;
            set
            {
                if (_generationSize != value)
                {
                    _generationSize = value;
                    InvokePropertyChanged(nameof(GenerationSize));
                }
            }
        }

        /// <summary>
        /// Свойство для изменения TextBox'а размера генома
        /// </summary>
        public string GenomeSize
        {
            get => _genomeSize;
            set
            {
                if (_genomeSize != value)
                {
                    _genomeSize = value;
                    InvokePropertyChanged(nameof(GenomeSize));
                }
            }
        }
    }
}
