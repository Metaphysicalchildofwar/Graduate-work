using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.WorkModels
{
    /// <summary>
    /// Класс для выбора функции из ComboBox
    /// </summary> 
    internal class ChoiseFunction
    {
        double f;

        /// <summary>
		/// Метод выбора функции из ComboBox
		/// </summary> 
        public double TheActualFunction(double[] values)
        {
            double x = values[0];
            double y = values[1];

            switch (ViewModel.ViewModelGeneticAlgorithm.numberFunct)
            {
                case 0: f = -(Math.Pow(x, 2.0) + Math.Pow(y, 2.0) - 4.0); break;
                case 1: f = -(Math.Pow((x-3.0), 2.0) + Math.Pow((y - 1.0), 2.0) - 6.0); break;
                case 2: f = Math.Pow(Math.E, -(Math.Pow(x, 2.0) + Math.Pow(y, 2.0))) + 2 * Math.Pow(Math.E, -(Math.Pow((x - 2.0), 2.0) + Math.Pow((y - 2.0), 2.0))); break;
                case 3: f = Math.Pow(Math.E, -(Math.Pow(x, 2.0) + Math.Pow(y, 2.0))) + 2 * Math.Pow(Math.E, -(Math.Pow((x - 2.0), 2.0) + Math.Pow((y - 2.0), 2.0))) + 6 * Math.Pow(Math.E, -(Math.Pow((x + 2), 2.0) + Math.Pow((y - 2), 2.0))); break;
            }
            return f;
        }
    }
}
