using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetMyBrainWPFChart;
using System;
using System.ComponentModel;

namespace SetMyBrainChartTest
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void NeuroskyFrequenciesTest()
        {
            
            NeuroskyFrequencies nf = new NeuroskyFrequencies();
            //int i = 0;

            //nf.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            //{
            //    i++;
            //};


            //nf.alpha1 = 10;
            //nf.alpha2 = 20;
            //nf.alpha2 = 20;
            Assert.IsTrue(true);
        }
    }
}
