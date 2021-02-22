using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Statistics
{
    public class Trendline
    {
        private readonly IList<float> xAxisValues;
        private readonly IList<float> yAxisValues;
        //private int count;
        //private float xAxisValuesSum;
        //private float xxSum;
        //private float xySum;
        //private float yAxisValuesSum;

        public Trendline(IList<float> yAxisValues, IList<float> xAxisValues)
        {
            this.yAxisValues = yAxisValues;
            this.xAxisValues = xAxisValues;

            //this.Initialize();
        }

        public float Slope()
        {
            int count = yAxisValues.Count;
            float yAxisValuesSum = yAxisValues.Sum();
            float xAxisValuesSum = xAxisValues.Sum();
            float xxSum = 0;
            float xySum = 0;

            for (int i = 0; i < count; i++)
            {
                xySum += (this.xAxisValues[i] * this.yAxisValues[i]);
                xxSum += (this.xAxisValues[i] * this.xAxisValues[i]);
            }

            return ((count * xySum) - (xAxisValuesSum * yAxisValuesSum)) / ((count * xxSum) - (xAxisValuesSum * xAxisValuesSum));
        }

        //public float Slope { get; private set; }
        //public float Intercept { get; private set; }
        //public float Start { get; private set; }
        //public float End { get; private set; }

        //private void Initialize()
        //{
        //    this.count = this.yAxisValues.Count;
        //    this.yAxisValuesSum = this.yAxisValues.Sum();
        //    this.xAxisValuesSum = this.xAxisValues.Sum();
        //    this.xxSum = 0;
        //    this.xySum = 0;

        //    for (int i = 0; i < this.count; i++)
        //    {
        //        this.xySum += (this.xAxisValues[i] * this.yAxisValues[i]);
        //        this.xxSum += (this.xAxisValues[i] * this.xAxisValues[i]);
        //    }

        //    this.Slope = this.CalculateSlope();
        //    //this.Intercept = this.CalculateIntercept();
        //    //this.Start = this.CalculateStart();
        //    //this.End = this.CalculateEnd();
        //}

        //public float CalculateSlope()
        //{
        //    try
        //    {
        //        return ((this.count * this.xySum) - (this.xAxisValuesSum * this.yAxisValuesSum)) / ((this.count * this.xxSum) - (this.xAxisValuesSum * this.xAxisValuesSum));
        //    }
        //    catch (DivideByZeroException)
        //    {
        //        return 0;
        //    }
        //}

        //private float CalculateIntercept()
        //{
        //    return (this.yAxisValuesSum - (this.Slope * this.xAxisValuesSum)) / this.count;
        //}

        //private float CalculateStart()
        //{
        //    return (this.Slope * this.xAxisValues.First()) + this.Intercept;
        //}

        //private float CalculateEnd()
        //{
        //    return (this.Slope * this.xAxisValues.Last()) + this.Intercept;
        //}
    }
}
