using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOAlgorithmGenerator
{
    public class MutationModel
    {
        public int Id { get; set; }

        public int Range { get; set; }

        public double Probability { get; set; }

        public bool Selected { get; set; }
    }
}
