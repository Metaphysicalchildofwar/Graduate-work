using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    public delegate double GAFunction(double[] values);

    /// <summary>
	/// Главный класс генетического алгоритма
	/// </summary>
    public class GeneticAlgorithm
    {
        /// <summary>
        /// Размер популяции
        /// </summary>
        public int PopulationSize { get; set; }

        /// <summary>
        /// Количество поколений
        /// </summary>
        public int Generations { get; set; }

        /// <summary>
        /// Размер генома
        /// </summary>
        public int GenomeSize { get; set; }

        /// <summary>
        /// Процент скрещиваний
        /// </summary>
        public double CrossoverRate { get; set; }

        /// <summary>
        /// Процент мутации
        /// </summary>
        public double MutationRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FitnessFile { get; set; }

        /// <summary>
        /// Удержание предыдущего лучшего генома на месте худшего в нынешнем
        /// </summary>
        public bool Elitism { get; set; }

        /// <summary>
        /// Настоящая приспособленность
        /// </summary>
        private double TotalFitness { get; set; }

        /// <summary>
		/// Cписок геномов
		/// </summary>
        private List<Genome> ThisGeneration { get; set; }

        /// <summary>
		/// Cписок приспособленностей
		/// </summary>
        private List<double> FitnessTable { get; set; }

        /// <summary>
		/// 
		/// </summary>
        public GAFunction FitnessFunction { get; set; }

        /// <summary>
		/// Создаем геномы и заполняем их генами
		/// </summary>
        private void CreateGenomes()
        {
            for (var i = 0; i < PopulationSize; i++)
            {
                Genome genome = new Genome();
                genome.CreateGenes();
                ThisGeneration.Add(genome);
            }
        }

        /// <summary>
		/// Сортируем популяции и сортируем в порядке пригодности
		/// </summary>
        private void RankPopulation()
        {
            TotalFitness = 0;
            for (var p = 0; p < PopulationSize; p++)
            {
                Genome genome = ThisGeneration[p];
                genome.Fitness = FitnessFunction(genome.Genes);
                TotalFitness += genome.Fitness;
            }
            ThisGeneration.Sort(new GenomeComparison());

            double fitness = 0.0;
            FitnessTable.Clear();
            for (var p = 0; p < PopulationSize; p++)
            {
                fitness += (ThisGeneration[p]).Fitness;
                FitnessTable.Add(fitness);
            }
        }
    }
}
