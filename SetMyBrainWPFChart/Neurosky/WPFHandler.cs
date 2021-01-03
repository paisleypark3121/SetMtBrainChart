using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Neurosky
{
    public class WPFHandler : IHandler
    {
        #region NeuroskyFrequencies
        ChartValues<DateChartModel> ChartValuesAlpha1 = null;
        ChartValues<DateChartModel> ChartValuesAlpha2 = null;
        ChartValues<DateChartModel> ChartValuesBeta1 = null;
        ChartValues<DateChartModel> ChartValuesBeta2 = null;
        ChartValues<DateChartModel> ChartValuesGamma1 = null;
        ChartValues<DateChartModel> ChartValuesGamma2 = null;
        ChartValues<DateChartModel> ChartValuesDelta = null;
        ChartValues<DateChartModel> ChartValuesTheta = null;
        #endregion

        #region NeuroskyIndex
        ChartValues<DateChartModel> ChartValuesAttention = null;
        ChartValues<DateChartModel> ChartValuesCreativity = null;
        ChartValues<DateChartModel> ChartValuesImmersion = null;
        ChartValues<DateChartModel> ChartValuesArousal = null;
        ChartValues<DateChartModel> ChartValuesEngagement = null;
        #endregion

        public WPFHandler(
            ChartValues<DateChartModel> _ChartValuesAlpha1,
            ChartValues<DateChartModel> _ChartValuesAlpha2,
            ChartValues<DateChartModel> _ChartValuesBeta1,
            ChartValues<DateChartModel> _ChartValuesBeta2,
            ChartValues<DateChartModel> _ChartValuesGamma1,
            ChartValues<DateChartModel> _ChartValuesGamma2,
            ChartValues<DateChartModel> _ChartValuesDelta,
            ChartValues<DateChartModel> _ChartValuesTheta,
            ChartValues<DateChartModel> _ChartValuesAttention,
            ChartValues<DateChartModel> _ChartValuesCreativity,
            ChartValues<DateChartModel> _ChartValuesImmersion,
            ChartValues<DateChartModel> _ChartValuesArousal,
            ChartValues<DateChartModel> _ChartValuesEngagement)
        {
            ChartValuesAlpha1 = _ChartValuesAlpha1;
            ChartValuesAlpha2 = _ChartValuesAlpha2;
            ChartValuesBeta1 = _ChartValuesBeta1;
            ChartValuesBeta2 = _ChartValuesBeta2;
            ChartValuesGamma1 = _ChartValuesGamma1;
            ChartValuesGamma2 = _ChartValuesGamma2;
            ChartValuesDelta = _ChartValuesDelta;
            ChartValuesTheta = _ChartValuesTheta;
            ChartValuesAttention = _ChartValuesAttention;
            ChartValuesCreativity = _ChartValuesCreativity;
            ChartValuesImmersion = _ChartValuesImmersion;
            ChartValuesArousal = _ChartValuesArousal;
            ChartValuesEngagement = _ChartValuesEngagement;
        }

        public void HandleFrequencies(object parameters)
        {
            NeuroskyFrequencies neuroskyFrequencies = (NeuroskyFrequencies)parameters;

            ChartValuesAlpha1.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.alpha1
            });
            ChartValuesAlpha2.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.alpha2
            });
            ChartValuesBeta1.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.beta1
            });
            ChartValuesBeta2.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.beta2
            });
            ChartValuesGamma1.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.gamma1
            });
            ChartValuesGamma2.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.gamma2
            });
            ChartValuesDelta.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.delta
            });
            ChartValuesTheta.Add(new DateChartModel
            {
                DateTime = neuroskyFrequencies.timestamp,
                Value = neuroskyFrequencies.theta
            });
        }

        public void HandleIndexes(object parameters)
        {
            SetMyBrainIndexes setMyBrainIndexes = (SetMyBrainIndexes)parameters;

            ChartValuesAttention.Add(new DateChartModel
            {
                DateTime = setMyBrainIndexes.timestamp,
                Value = setMyBrainIndexes.attention
            });
            ChartValuesCreativity.Add(new DateChartModel
            {
                DateTime = setMyBrainIndexes.timestamp,
                Value = setMyBrainIndexes.creativity
            });
            ChartValuesImmersion.Add(new DateChartModel
            {
                DateTime = setMyBrainIndexes.timestamp,
                Value = setMyBrainIndexes.immersion
            });
            ChartValuesArousal.Add(new DateChartModel
            {
                DateTime = setMyBrainIndexes.timestamp,
                Value = setMyBrainIndexes.arousal
            });
            ChartValuesGamma1.Add(new DateChartModel
            {
                DateTime = setMyBrainIndexes.timestamp,
                Value = setMyBrainIndexes.engagement
            });
        }
    }
}
