using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOAlgorithmGenerator
{
    public class Calculations
    {
        public static int GetL(string precision, float from, float to)
        {
            return int.Parse(Math.Ceiling(Math.Log(((to - from) /
                double.Parse(precision)) + 1, 2)).ToString());
        }
    }
}
