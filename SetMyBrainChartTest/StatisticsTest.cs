using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetMyBrainWPFChart.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetMyBrainChartTest
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void TrendlineTest()
        {
            float[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            float[] y = { 1, 3, 2, 4, 2, 0, 1, 3, 2, 7, 10, 3, 2, 6, 5, 4 };

            List<float> xAxisValues = x.ToList();
            List<float> yAxisValues = y.ToList();

            Trendline td = new Trendline(xAxisValues, yAxisValues);
            float slope1 = td.Slope();

            x = x.Select(e => e + 34).ToArray();
            xAxisValues = x.ToList();
            td = new Trendline(xAxisValues, yAxisValues);
            float slope2 = td.Slope();

            Assert.AreEqual(slope1, slope2);
        }

        [TestMethod]
        public void ListTest()
        {
            float[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var xList = x.ToList();
            xList.RemoveAt(0);
            xList.Add(17);
            
            x = xList.ToArray();

            xList.RemoveAt(0);
            xList.Add(10);

            xList.RemoveAt(0);
            xList.Add(777);

            x = xList.ToArray();
        }

        [TestMethod]
        public void SlopeTest()
        {
            float expected=8.175757576F;
            float[] y = {1000, 25, 34, 76, 32, 56, 78, 67, 89, 90 };
            float _y = 110;

            y=SetMyBrainWPFChart.Neurosky.Utilities.PushElement(y, _y);
            float actual=Utilities.Slope(y);

            Assert.AreEqual(expected, actual);
        }

    }
}
