using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOAlgorithmGenerator
{
    public class NumberConverter
    {
        private float _from;
        private float _to;
        private double _precision;


        private int _individualResolution;

        public int BinaryToIntConvert(string input)
        {
            var inte =  Convert.ToInt32(input, 2);
            return inte;
        }

        public string IntToBinaryConvert(int input)
        {
            var binaryValue = Convert.ToString(input, 2);
            if (binaryValue.Length < _individualResolution)
            {
                var builder = new StringBuilder();
                for (int i = 0; i < _individualResolution - binaryValue.Length; i++)
                {
                    builder.Append("0");
                }
                builder.Append(binaryValue);
                return builder.ToString();
            }
            return binaryValue;
        }

        public string RealToBin(double input)
        {
            return IntToBinaryConvert(RealToIntConvert(input));
        }

        public double BinToReal(string input)
        {
            return IntToRealConvert(BinaryToIntConvert(input));
        }

        public double IntToRealConvert(int input)
        {
            var res =  Math.Round((input * (_to - _from) / (Math.Pow(2, _individualResolution) - 1))
                + _from, GetPrecisionInt(_precision.ToString()));
            return res;
        }

        public int RealToIntConvert(double input)
        {
            return Convert.ToInt32(Math.Round((1 / (_to - _from)) *
                (input - _from) * (Math.Pow(2, _individualResolution) - 1)));
        }

        public NumberConverter(float from, float to, double precision)
        {
            _from = from;
            _to = to;
            _precision = precision;
            SetIndividualResolution();
        }

        private void SetIndividualResolution()
        {
            _individualResolution = int.Parse(Math.Ceiling(Math.Log(((_to - _from) /
                _precision) + 1, 2)).ToString());
        }

        private int GetPrecisionInt(string precision)
        {
            var s1 = precision.Substring(2);
            return s1.IndexOf('1')+1;
        }
    }
}
