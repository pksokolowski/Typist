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

        [Fact]
        public void AveragesOutMiddleNumbers()
        {
            var data = new double[] { 1, 2, 3, 4 };
            var median = MedianCalculator.median(data);
            Assert.Equal(2.5, median);
        }

        [Fact]
        public void ReturnsTheOnlyElementAsMedian()
        {
            var data = new double[] { 5.5};
            var median = MedianCalculator.median(data);
            Assert.Equal(5.5, median);
        }

        [Fact]
        public void DoesFindMedianInUnsortedArray()
        {
            var data = new double[] { 3, 1, 2 };
            var median = MedianCalculator.median(data);
            Assert.Equal(2, median);
        }

        [Fact]
        public void DoesFindMedianInTwoElementArray()
        {
            var data = new double[] { 10, 20};
            var median = MedianCalculator.median(data);
            Assert.Equal(15, median);
        }
    }
}
