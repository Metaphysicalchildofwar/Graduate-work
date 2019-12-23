using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    /// <summary>
	/// Сравнивает геномы по пригодности
	/// </summary>
    public sealed class GenomeComparison : IComparer<Genome>
    {
        public int Compare(Genome x, Genome y)
        {
            if (x.Fitness > y.Fitness)
            {
                return 1;
            }
            else if (x.Fitness < y.Fitness)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
