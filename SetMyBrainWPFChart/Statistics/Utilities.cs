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

        /// <summary>
        /// Push into x,y the new values _x,_y
        /// X,_x is not necessary
        /// </summary>
        public static float Slope(float[] y)
        {
            //float AVG_x = 5.5F;
            float sum_x2 = 82.5F;
            
            float[] xx = { -4.5F, -3.5F, -2.5F, -1.5F, -0.5F, 0.5F, 1.5F, 2.5F, 3.5F, 4.5F };
            float[] x2 = { 20.25F, 12.25F, 6.25F, 2.25F, 0.25F, 0.25F, 2.25F, 6.25F, 12.25F, 20.25F };

            float AVG_y = y.Average();
            float[] yy = y.Select(e => e - AVG_y).ToArray();
            float[] xy= new float[10];
            for (int i = 0; i < 10; i++)
                xy[i] = xx[i] * yy[i];

            float sum_xy = xy.Sum();

            return sum_xy / sum_x2;
        }
    }
}
