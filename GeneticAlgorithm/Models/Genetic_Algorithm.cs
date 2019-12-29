using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    public delegate double GAFunction(double[] values);
    public delegate void Message(string message);
    
    /// <summary>
	/// Главный класс генетического алгоритма
	/// </summary>
    public class Genetic_Algorithm
    {
        public event Message Notify;

        /// <summary>
        /// Конструктор для инициализации
        /// </summary>
        public Genetic_Algorithm(double crossoverRate, double mutationRate, int populationSize, int generationSize)
        {
            InitialValues();
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            PopulationSize = populationSize;
            GenerationSize = generationSize;
            GenomeSize = 2;
            FitnessFile = string.Empty;
        }

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
        /// Частота скрещиваний
        /// </summary>
        public double CrossoverRate { get; set; }

        /// <summary>
        /// Частота мутации
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
        public void InitialValues()
        {
            Elitism = false;
        }

        /// <summary>
        /// Настоящая приспособленность
        /// </summary>
        private double TotalFitness { get; set; }

        /// <summary>
		/// Cписок текущих геномов
		/// </summary>
        private List<Genome> ThisGeneration { get; set; }

        /// <summary>
		/// Cписок следующего поколения геномов
		/// </summary>
        private List<Genome> NextGeneration { get; set; }

        /// <summary>
		/// Cписок приспособленностей
		/// </summary>
        private List<double> FitnessTable { get; set; }

        /// <summary>
		/// 
		/// </summary>
        public GAFunction FitnessFunction { get; set; }

        /// <summary>
        /// Временный (надеюсь) стандарный рандомайзер
        /// </summary>
        static Random Rand = new Random();

        /// <summary>
        /// Величина поколения
        /// </summary>
        private int GenerationSize { get; set; }

        /// <summary>
		/// Создаем геномы и заполняем их генами
		/// </summary>
        private void CreateGenomes()
        {
            for (var i = 0; i < PopulationSize; i++)
            {
                Genome genome = new Genome(GenomeSize);
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

        /// <summary>
		/// Колесо рулетки (чем выше приспособленность, тем большая вероятность выбора)
		/// </summary>
        private int RouletteSelection()
        {
            double randomFitness = Rand.NextDouble() * TotalFitness;
            int check = -1;
            int first = 0;
            int last = PopulationSize - 1;
            int mid = (last - first) / 2;

            while (check == -1 && first <= last)
            {
                if (randomFitness < FitnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > FitnessTable[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;

                if ((last - first) == 1)
                {
                    check = last;
                }
            }
            return check;
        }

        /// <summary>
		/// Создание следующего поколения
		/// </summary> 
        private void CreateNextGeneration()
        {
            NextGeneration.Clear();
            Genome genome = null;

            if(Elitism)
            {
                genome = ThisGeneration[PopulationSize - 1];
            }

            for (var p = 0; p < PopulationSize; p+=2)
            {
                int parentIndexFirst = RouletteSelection();
                int parentIndexSecond = RouletteSelection();
                Genome parentFirst, parentSecond, childFirst, childSecond;

                parentFirst = ThisGeneration[parentIndexFirst];
                parentSecond = ThisGeneration[parentIndexSecond];

                if (Rand.NextDouble() < CrossoverRate)
                {
                    parentFirst.Crossover(ref parentSecond, out childFirst, out childSecond);
                }
                else
                {
                    childFirst = parentFirst;
                    childSecond = parentSecond;
                }
                childFirst.Mutate();
                childSecond.Mutate();

                NextGeneration.Add(childFirst);
                NextGeneration.Add(childSecond);
            }
            if (Elitism && genome != null)
            {
                NextGeneration[0] = genome;
            }

            ThisGeneration.Clear();
            for (var p = 0; p < PopulationSize; p++)
            {
                ThisGeneration.Add(NextGeneration[p]);
            }
        }

        /// <summary>
		/// Метод работы алгоритма
		/// </summary> 
        public void WorkGeneticAlgorithm()
        {
            FitnessTable = new List<double>();
            ThisGeneration = new List<Genome>(GenerationSize);
            NextGeneration = new List<Genome>(GenerationSize);
            Genome.MutationRate = MutationRate;

            CreateGenomes();
            RankPopulation();

            //будет редачиться
            for (var g = 0; g < GenerationSize; g++)
            {
                CreateNextGeneration();
                RankPopulation();
                Notify?.Invoke($"{(ThisGeneration[PopulationSize - 1].Fitness)}, {g}");
            }
        }

        /// <summary>
        /// Получить лучшее значение (для вывода)
        /// </summary> 
        public void GetBest(out double[] values, out double fitness)
        {
            Genome genome = ThisGeneration[PopulationSize - 1];
            values = new double[genome.Length];
            genome.GetValues(ref values);
            fitness = genome.Fitness;
        }
    }
}
