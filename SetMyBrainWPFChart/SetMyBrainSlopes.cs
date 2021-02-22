using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart
{
    public class SetMyBrainSlopes
    {
        #region fields
        public DateTime timestamp { get; set; }
        public float theta { get; set; }
        public float beta { get; set; }
        public float alpha { get; set; }
        public float power { get; set; }
        #endregion

        public SetMyBrainSlopes(
            DateTime _timestamp,
            float _theta,
            float _beta,
            float _alpha,
            float _power
            )
        {
            timestamp = _timestamp;
            theta = _theta;
            beta = _beta;
            alpha = _alpha;
            power = _power;
        }
    }
}