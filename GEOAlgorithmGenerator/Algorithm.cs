using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOAlgorithmGenerator
{
    public class Algorithm
    {
        private NumberConverter _calc;
        public string Precision { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Tau { get; set; }
        public int Size { get; set; }
        public Random Randomizer { get; set; }

        public double? Best { get; set; }


        public Individual Individual { get; set; }
        public List<Individual> Mutated { get; set; }

        public List<MutationModel> MutationElements { get; set; }

        Results results;

        public Algorithm(string precision, string from, string to, string tau)
        {
            results = new Results()
            {
                FXs = new List<double>(),
                Bests = new List<double>()
            };
            _calc = new NumberConverter(float.Parse(from), float.Parse(to), double.Parse(precision));
            Precision = precision;
            From = from;
            To = to;
            Tau = int.Parse(tau);
            Size = Calculations.GetL(Precision, float.Parse(From), float.Parse(To));
            Randomizer = new Random();
            Individual = new Individual()
            {
                Binary = ""
            };
            Mutated = new List<Individual>();
            MutationElements = new List<MutationModel>();
        }

        public Results Start(int iterations)
        {
            GetRandomIndividual();
            for (int i = 0; i < iterations; i++)
            {
                Process(i+1);
            }
            return results;
        }

        public void GetRandomIndividual()
        {
            for (int i = 0; i < Size; i++)
            {
                Individual.Binary += GetRandomInt(0, 2, Randomizer).ToString();
            }
        }

        public void Process(int iteration)
        {
            // Utworznie zmutowanej populacji
            for (int i = 0; i < Size; i++)
            {
                if (Individual.Binary[i] == '0')
                    Mutated.Add(new Individual() { Id = i + 1, Binary = ReplaceAtIndex(i, '1', Individual.Binary) });
                else
                    Mutated.Add(new Individual() { Id = i + 1, Binary = ReplaceAtIndex(i, '0', Individual.Binary) });
            }

            // Liczenie wartości funkcji
            Individual.Real = _calc.BinToReal(Individual.Binary);
            Individual.FunctionResult = GetFunctionResult(Individual.Real);

            // Liczenie wartości funkcji zmutowanych
            foreach (var item in Mutated)
            {
                item.Real = _calc.BinToReal(item.Binary);
                item.FunctionResult = GetFunctionResult(item.Real);
            }

            // sortowanie zmutowanych 
            Mutated = Mutated.OrderByDescending(_ => _.FunctionResult).ToList();

            // tworzenie tabeli wag i prawdopodobieństw
            for (int i = 1; i <= Size; i++)
            {
                var range = Mutated.FindIndex(_ => _.Id == i) + 1;
                var prob = 1/Math.Pow(range+0.2, Tau);

                MutationElements.Add(new MutationModel()
                {
                    Id = i,
                    Range = range,
                    Probability = prob,
                    Selected = GetRandomNumber(0, 1, Randomizer) < prob
                });
            }

            // mutacja
            var ids = MutationElements.Where(_ => _.Selected).Select(_ => _.Id).ToList();
            StringBuilder sb2 = new StringBuilder(Individual.Binary);
            foreach (var elem in ids)
            {
                if (sb2[elem - 1] == '1')
                    sb2[elem - 1] = '0';
                else
                    sb2[elem - 1] = '1';
            }
            var result = sb2.ToString();


            var resReal = _calc.BinToReal(result);
            var fx = GetFunctionResult(resReal);
            if (Best == null)
            {
                results.Iteration = iteration;
                Best = fx;
                results.FXBest = Best.Value;
                results.XRealBest = resReal;
                results.XBinBest = result;
            }
            else
            {
                if (fx > Best)
                {
                    results.Iteration = iteration;
                    Best = fx;
                    results.FXBest = Best.Value;
                    results.XRealBest = resReal;
                    results.XBinBest = result;
                }
                    
            }
            Individual = new Individual()
            {
                Binary = result
            };
            results.FXs.Add(fx);
            results.Bests.Add(Best.Value);
        }

        private int GetRandomInt(int from, int to, Random random)
        {
            return random.Next(from, to);
        }

        private double GetRandomNumber(float from, float to, Random random)
        {
            double number;
            number = random.NextDouble() * (to - from) + from;
            return number;
        }

        private double GetFunctionResult(double input)
        {
            return (input % 1) * (Math.Cos(20 * Math.PI * input) - Math.Sin(input));
        }

        static string ReplaceAtIndex(int i, char value, string word)
        {
            char[] letters = word.ToCharArray();
            letters[i] = value;
            return string.Join("", letters);
        }
    }
}
