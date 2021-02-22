using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetMyBrainWPFChart.Log;
using System;
using System.Threading.Tasks;

namespace SetMyBrainChartTest
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void DoWriteTest()
        {
            string[] args = new string[] { "test.txt" };
            ILog log = new FileLog(args);

            log.Trace("this is my test");
        }

        [TestMethod]
        public Task DoWriteAsyncTest()
        {
            string[] args = new string[] { "test.txt" };
            ILog log = new FileLog(args);

            return log.TraceAsync("this is my test");
            
        }
    }
}
