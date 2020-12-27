using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Statistics
{
    public class Utilities
    {
        public static float Median(float[] data)
        {
            data = data.OrderBy(i => i).ToArray();
            int mid = data.Length / 2;
            float median;

            if (data.Length % 2 == 0)
                median = (float)((data[mid] + data[mid - 1]) / 2.0);
            else
                median = data[mid];

            return median;
        }

        public static float Variance(float[] data)
        {
            float mean = data.Average();
            float[] dataC = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
                dataC[i] = (float)Math.Pow(data[i] - mean, 2);
            return dataC.Average();
        }
    }
}
