using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart
{
    public class NeuroskyFrequencies
    {
        #region fields
        public DateTime timestamp { get; set; }
        public float alpha1 { get; set; }
        public float alpha2 { get; set; }
        public float beta1 { get; set; }
        public float beta2 { get; set; }
        public float gamma1 { get; set; }
        public float gamma2 { get; set; }
        public float delta { get; set; }
        public float theta { get; set; }
        #endregion

        public NeuroskyFrequencies(
            DateTime _timestamp,
            float _alpha1,
            float _alpha2,
            float _beta1,
            float _beta2,
            float _gamma1,
            float _gamma2,
            float _delta,
            float _theta)
        {
            timestamp = _timestamp;
            alpha1 = _alpha1;
            alpha2 = _alpha2;
            beta1 = _beta1;
            beta2 = _beta2;
            gamma1 = _gamma1;
            gamma2 = _gamma2;
            delta = _delta;
            theta = _theta;
        }
    }
}
