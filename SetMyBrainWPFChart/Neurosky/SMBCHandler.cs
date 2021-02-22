using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SetMyBrainWPFChart.Neurosky
{
    public class SMBCHandler : IHandler
    {
        private SetMyBrainChart setMyBrainChart;
        private ConnectionUserControl connectionUserControl;
        private SmileUserControl smileUserControl;

        public SMBCHandler(
            SetMyBrainChart _setMyBrainChart,
            ConnectionUserControl _connectionUserControl,
            SmileUserControl _smileUserControl
            )
        {
            setMyBrainChart = _setMyBrainChart;
            connectionUserControl = _connectionUserControl;
            smileUserControl = _smileUserControl;
        }

        public void HandlePoorSignal(object parameters)
        {
            Action action = () => connectionUserControl.PoorSignal = (float)parameters;
            Application.Current.Dispatcher.Invoke(action);
        }

        public void HandleFrequencies(object parameters)
        {
            setMyBrainChart.NeuroskyFrequencies = (NeuroskyFrequencies)parameters;
        }

        public void HandleIndexes(object parameters)
        {
            setMyBrainChart.SetMyBrainIndexes = (SetMyBrainIndexes)parameters;
        }

        public void HandleSlopes(object parameters)
        {
            setMyBrainChart.SetMyBrainSlopes = (SetMyBrainSlopes)parameters;
        }

        public void HandleFlow(object parameters)
        {
            Action action = () => smileUserControl.Flow = (bool)parameters;
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
