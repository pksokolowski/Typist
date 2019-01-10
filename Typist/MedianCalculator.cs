using System;
using System.Collections.Generic;

namespace Typist
{
    /// <summary>
    /// Calculates a median for a collection/array.
    /// For now using a naive O(n log n) approach, 
    /// might later be replaced by a linear time solution.
    /// </summary>
    public class MedianCalculator
    {    
        public static double median(double[] arr)
        {
            if (arr.Length == 1) return arr[0];            
            Array.Sort(arr);
            var medianIndex = (arr.Length / 2);
            var median = arr[medianIndex];
            if (arr.Length >= 2 && arr.Length % 2 == 0)
            {
                median = (median + arr[medianIndex - 1]) / 2;
            }
            return median;
        }

        public static double median(List<double> list)
        {
            return median(list.ToArray());
        }
    }
}
