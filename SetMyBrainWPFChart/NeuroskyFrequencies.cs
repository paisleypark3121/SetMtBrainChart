﻿using System;
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
        #region private fields
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
    }
}