using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart
{
    public class SetMyBrainIndexes
    {
        #region fields
        public DateTime timestamp { get; set; }
        public float attention { get; set; }
        public float creativity { get; set; }
        public float immersion { get; set; }
        public float arousal { get; set; }
        public float engagement { get; set; }
        #endregion

        public SetMyBrainIndexes(
            DateTime _timestamp,
            float _attention,
            float _creativity,
            float _immersion,
            float _arousal,
            float _engagement
            )
        {
            timestamp = _timestamp;
            attention = _attention;
            creativity = _creativity;
            immersion = _immersion;
            arousal = _arousal;
            engagement = _engagement;
        }
    }
}