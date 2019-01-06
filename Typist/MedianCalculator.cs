using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typist
{
    /// <summary>
    /// Calculates a median for a collection/array.
    /// For now using a naive O(n log n) approach, 
    /// might later be replaced by a linear time solution.
    /// </summary>
    class MedianCalculator
    {    
        public static double median(double[] arr)
        {
            Array.Sort(arr);
            var medianIndex = Math.Max(0, (arr.Length / 2) - 1);
            var median = arr[medianIndex];
            if (arr.Length > 2 && arr.Length % 2 != 0)
            {
                median = (median + arr[medianIndex + 1]) / 2;
            }
            return median;
        }

        public static double median(List<double> list)
        {
            return median(list.ToArray());
        }
    }
}
