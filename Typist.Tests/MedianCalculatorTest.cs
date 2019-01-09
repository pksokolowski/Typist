using System;
using Xunit;

namespace Typist.Tests
{
    public class MedianCalculatorTest
    {
        [Fact]
        public void DoesFindMedianInSortedArray()
        {
            var data = new double[] { 1, 2, 3 };
            var median = MedianCalculator.median(data);
            Assert.Equal(2, median);
        }
    }
}
