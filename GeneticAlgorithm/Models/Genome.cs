using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    /// <summary>
    /// Класс для генома - набора генов
    /// </summary>
    public class Genome
    {
        public Genome() { }
        public Genome(int length, bool createGenes)
        {
            Length = length;
            Genes = new double[length];
            
            if (createGenes)
            {
                CreateGenes();
            }
        }
        public Genome(int length)
        {
            Length = length;
            Genes = new double[Length];
            CreateGenes();
        }

        /// <summary>
        /// Приспособленность
        /// </summary>
        public double Fitness { get; set; }

        /// <summary>
        /// Процент мутации
        /// </summary>
        public static double MutationRate { get; set; }

        /// <summary>
        /// Величина генома
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Гены 
        /// </summary>
        public double[] Genes { get; }

        /// <summary>
        /// Получить значения генов
        /// </summary>
        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < Length; i++)
                values[i] = Genes[i];
        }

        /// <summary>
        /// Временный (надеюсь) стандарный рандомайзер
        /// </summary>
        static Random Rand = new Random();

        /// <summary>
        /// Рандомное создание генов
        /// </summary>
        public void CreateGenes()
        {
            for (var l = 0; l < Length; l++)
            {
                Genes[l] = Rand.NextDouble();
            }
        }

        /// <summary>
        /// Перегородка в геноме, для рандомности разделения генов в дочерние геномы
        /// </summary>
        private int Partition()
        {
            return (int)(Rand.NextDouble() * Length);
        }

        /// <summary>
        /// Cкрещивание геномов
        /// </summary>
        public void Crossover(ref Genome familyGenome, out Genome firstChild, out Genome secondChild)
        {          
            int partition = Partition();

            firstChild = new Genome(Length, false);
            secondChild = new Genome(Length, false);

            for (var l = 0; l < Length; l++)
            {
                if (l < partition)
                {
                    firstChild.Genes[l] = Genes[l];
                    secondChild.Genes[l] = familyGenome.Genes[l];
                }
                else
                {
                    firstChild.Genes[l] = familyGenome.Genes[l];
                    secondChild.Genes[l] = Genes[l];
                }
            }           
        }

        /// <summary>
        /// Мутация генов
        /// </summary>
        public void Mutate()
        {
            for (var l= 0; l < Length; l++)
            {
                if (Rand.NextDouble() < MutationRate)
                {
                    Genes[l] = (Genes[l] + Rand.NextDouble()) / 2.0;
                }
            }
        }
    }
}
