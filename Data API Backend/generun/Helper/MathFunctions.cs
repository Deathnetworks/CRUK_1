using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace generun.Helper
{
    public static class MathFunctions
    {
        public static decimal Cv(decimal Mean, decimal SD)
        {
            decimal rsCv = (SD / Mean) * 100;
            return rsCv;
        }

        public static decimal Mean(List<decimal> Mean)
        {
            int numItems = 0;
            decimal rsMean = 0;
            decimal total = 0;
            foreach (decimal item in Mean)
            {
                total += item;
                numItems++;
            }
            if (numItems == 0 | total == 0)
                return 0;
            rsMean = total / numItems;
            return rsMean;
        }

        public static decimal Sd(List<decimal> results)
        {
            decimal mean = 0;
            decimal variance = 0;
            decimal rsSd = 0;
            mean = Mean(results);

            List<decimal> differences = new List<decimal>();

            foreach (decimal item in results)
            {
                decimal temp = item - mean;
                temp = temp * temp;
                differences.Add(temp);
            }

            variance = Mean(differences);
            rsSd = (decimal)Math.Sqrt((double)variance);
            if (rsSd == 0)
                return 0;

            return rsSd;
        }

        public static decimal Square(List<decimal> Square)
        {
            decimal rsSquare = 0;
            decimal total = 0;
            foreach (decimal item in Square)
            {
                total += item;
            }
            rsSquare = total * total;
            return rsSquare;
        }
    }
}

