using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOAlgorithmGenerator
{
    public class Results
    {
        public int Iteration { get; set; } = 0;

        public double XRealBest { get; set; }

        public string XBinBest { get; set; }

        public double FXBest { get; set; }

        public List<double> FXs { get; set; }

        public List<double> Bests { get; set; }
    }
}
