using AppSettings;
using Connector;
using LiveCharts;
using SetMyBrainWPFChart.Neurosky;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetMyBrainWPFChart
{
    /// <summary>
    /// Logica di interazione per BackupMainWindow.xaml
    /// </summary>
    public partial class BackupMainWindow : Window
    {
        Random random = new Random();
        Collector collector = null;
        CancellationTokenSource tokenSource = null;        

        #region AppSettings
        IAppSettings appSettings = null;
        #endregion

        #region ThinkGear
        IConnector connector = null;
        #endregion

        #region IHandler
        IHandler handler = null;
        #endregion

        #region limits
        private int visibility_limit = 150;
        #endregion

        public BackupMainWindow()
        {
            InitializeComponent();

            appSettings = new AppSettings.AppSettings();
            connector = new ThinkgearConnector(new string[] { appSettings["COM"] });
            connector.Connect();
            visibility_limit = int.Parse(appSettings["visibility_limit"]);

            handler = new WPFHandler(
                0,
                NFUC.ChartValuesAlpha1,
                NFUC.ChartValuesAlpha2,
                NFUC.ChartValuesBeta1,
                NFUC.ChartValuesBeta2,
                NFUC.ChartValuesGamma1,
                NFUC.ChartValuesGamma2,
                NFUC.ChartValuesDelta,
                NFUC.ChartValuesTheta,
                SMBIUC.ChartValuesAttention,
                SMBIUC.ChartValuesCreativity,
                SMBIUC.ChartValuesImmersion,
                SMBIUC.ChartValuesArousal,
                SMBIUC.ChartValuesEngagement);

            NFUC.visibility_limit = visibility_limit;

            tokenSource = new CancellationTokenSource();
            collector = new Collector(null, appSettings, connector, handler, tokenSource);

            IsReading = false;
        }

        public bool IsReading { get; set; }

        #region deprecated
        //private void Read()
        //{
        //    #region structure
        //    Dictionary<string, object> previous = null;
        //    float previous_b = 0;

        //    #region baselines

        //    bool baseline = false;
        //    bool baseline_run = false;
        //    bool baseline_minmax_run = false;
        //    int slider = 0;
        //    int slider_limit = 30;
        //    int baseline_slider = 0;
        //    int baseline_slider_limit = 180;

        //    #region BASELINE - AVG & VAR 180 sec. (M_attention_180 & M_creativity_180)
        //    float baseline_attention_avg = 0;
        //    float baseline_attention_var = 0;
        //    float baseline_creativity_avg = 0;
        //    float baseline_creativity_var = 0;
        //    float baseline_engagement_avg = 0;
        //    float baseline_engagement_var = 0;
        //    float baseline_arousal_avg = 0;
        //    float baseline_arousal_var = 0;
        //    float baseline_immersion_avg = 0;
        //    float baseline_immersion_var = 0;
        //    #endregion

        //    #region BASELINE - min / max AVG & VAR 180 sec. (based on calculations on 30 sec. window)
        //    float min_grossIndex_attention = 999999;
        //    float max_grossIndex_attention = -999999;
        //    float min_grossIndex_creativity = 999999;
        //    float max_grossIndex_creativity = -999999;
        //    float min_grossIndex_engagement = 999999;
        //    float max_grossIndex_engagement = -999999;
        //    float min_grossIndex_arousal = 999999;
        //    float max_grossIndex_arousal = -999999;
        //    float min_grossIndex_immersion = 999999;
        //    float max_grossIndex_immersion = -999999;
        //    #endregion

        //    #region 30 sec. buffer for punctual -> each element contains value
        //    float[] attention_30 = new float[slider_limit];
        //    float[] creativity_30 = new float[slider_limit];
        //    float[] engagement_30 = new float[slider_limit];
        //    float[] arousal_30 = new float[slider_limit];
        //    float[] immersion_30 = new float[slider_limit];
        //    #endregion

        //    #region 30 sec. buffer for medians -> each element contains Median of last 30 sec. (attention_30, creativity_30 ...)
        //    float[] M_attention_30 = new float[slider_limit];
        //    float[] M_creativity_30 = new float[slider_limit];
        //    float[] M_engagement_30 = new float[baseline_slider_limit];
        //    float[] M_arousal_30 = new float[baseline_slider_limit];
        //    float[] M_immersion_30 = new float[baseline_slider_limit];
        //    #endregion

        //    #region 180 sec. buffer for medians -> each element contains Median of last 30 sec. (attention_30, creativity_30 ...)
        //    float[] M_attention_180 = new float[baseline_slider_limit];
        //    float[] M_creativity_180 = new float[baseline_slider_limit];
        //    float[] M_engagement_180 = new float[baseline_slider_limit];
        //    float[] M_arousal_180 = new float[baseline_slider_limit];
        //    float[] M_immersion_180 = new float[baseline_slider_limit];
        //    #endregion

        //    #region 180 sec. buffer for avg -> each element contains avg of Median of last 30 sec. (avg M_attention_30)
        //    float[] AVG_attention_180 = new float[baseline_slider_limit];
        //    float[] AVG_creativity_180 = new float[baseline_slider_limit];
        //    float[] AVG_engagement_180 = new float[baseline_slider_limit];
        //    float[] AVG_arousal_180 = new float[baseline_slider_limit];
        //    float[] AVG_immersion_180 = new float[baseline_slider_limit];
        //    #endregion

        //    #region 180 sec. buffer for var -> each element contains var of Median of last 30 sec. (var M_attention_30)
        //    float[] VAR_attention_180 = new float[baseline_slider_limit];
        //    float[] VAR_creativity_180 = new float[baseline_slider_limit];
        //    float[] VAR_engagement_180 = new float[baseline_slider_limit];
        //    float[] VAR_arousal_180 = new float[baseline_slider_limit];
        //    float[] VAR_immersion_180 = new float[baseline_slider_limit];
        //    #endregion

        //    #region 30 sec. initialization
        //    for (int i = 0; i < slider_limit; i++)
        //    {
        //        attention_30[i] = 0;
        //        creativity_30[i] = 0;
        //        engagement_30[i] = 0;
        //        arousal_30[i] = 0;
        //        immersion_30[i] = 0;

        //        M_attention_30[i] = 0;
        //        M_creativity_30[i] = 0;
        //        M_engagement_30[i] = 0;
        //        M_arousal_30[i] = 0;
        //        M_immersion_30[i] = 0;
        //    }
        //    #endregion

        //    #region 180 sec. initialization
        //    for (int i = 0; i < baseline_slider_limit; i++)
        //    {
        //        M_attention_180[i] = 0;
        //        M_creativity_180[i] = 0;
        //        M_engagement_180[i] = 0;
        //        M_arousal_180[i] = 0;
        //        M_immersion_180[i] = 0;

        //        AVG_attention_180[i] = 0;
        //        AVG_creativity_180[i] = 0;
        //        AVG_engagement_180[i] = 0;
        //        AVG_arousal_180[i] = 0;
        //        AVG_immersion_180[i] = 0;

        //        VAR_attention_180[i] = 0;
        //        VAR_creativity_180[i] = 0;
        //        VAR_engagement_180[i] = 0;
        //        VAR_arousal_180[i] = 0;
        //        VAR_immersion_180[i] = 0;
        //    }
        //    #endregion

        //    Stopwatch stopWatch_baseline = new Stopwatch();

        //    #endregion

        //    #endregion
        //    #region try
        //    try
        //    {
        //        while (true)
        //        {
        //            #region timing
        //            DateTime _datetime = DateTime.Now;
        //            #endregion

        //            #region get data      
        //            int limit = 1000;
        //            int count = limit;
        //            while ((count > 0) && !(bool)connector.GetData())
        //            {
        //                Console.WriteLine("Connecting... " + (limit - count + 1).ToString() + "/" + limit);
        //                Thread.Sleep(100);
        //                count--;
        //            }
        //            if (count == 0)
        //            {
        //                string error = "Cannot connect to headset: please restart";
        //                Console.WriteLine(error);
        //                throw new Exception(error);
        //            }
        //            #endregion

        //            #region get values
        //            float TG_DATA_RAW = (float)connector.GetValue(4);
        //            float TG_DATA_ALPHA1 = (float)connector.GetValue(7);
        //            float TG_DATA_ALPHA2 = (float)connector.GetValue(8);
        //            float TG_DATA_BETA1 = (float)connector.GetValue(9);
        //            float TG_DATA_BETA2 = (float)connector.GetValue(10);
        //            float TG_DATA_DELTA = (float)connector.GetValue(5);
        //            float TG_DATA_GAMMA1 = (float)connector.GetValue(11);
        //            float TG_DATA_GAMMA2 = (float)connector.GetValue(12);
        //            float TG_DATA_THETA = (float)connector.GetValue(6);
        //            float TG_DATA_MEDITATION = (float)connector.GetValue(3);
        //            float TG_DATA_ATTENTION = (float)connector.GetValue(2);
        //            float TG_DATA_POOR_SIGNAL = (float)connector.GetValue(1);
        //            #endregion

        //            #region baseline calculation
        //            if (!baseline)
        //            {
        //                #region start
        //                if (!stopWatch_baseline.IsRunning)
        //                {
        //                    if ((TG_DATA_ALPHA1 != 0) &&
        //                        (TG_DATA_ALPHA2 != 0) &&
        //                        (TG_DATA_THETA != 0))
        //                    {
        //                        stopWatch_baseline.Start();
        //                        float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);
        //                        previous_b = RelPower;

        //                        attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                        creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
        //                        engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
        //                        arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                        immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

        //                        slider++;
        //                    }
        //                }
        //                #endregion
        //                #region stop
        //                else if ((baseline_slider == baseline_slider_limit - 1) && (!baseline))
        //                {
        //                    stopWatch_baseline.Stop();

        //                    #region baseline calculation
        //                    baseline_attention_avg = M_attention_180.Average();
        //                    baseline_attention_var = Statistics.Utilities.Variance(M_attention_180);
        //                    baseline_creativity_avg = M_creativity_180.Average();
        //                    baseline_creativity_var = Statistics.Utilities.Variance(M_creativity_180);
        //                    baseline_engagement_avg = M_engagement_180.Average();
        //                    baseline_engagement_var = Statistics.Utilities.Variance(M_engagement_180);
        //                    baseline_arousal_avg = M_arousal_180.Average();
        //                    baseline_arousal_var = Statistics.Utilities.Variance(M_arousal_180);
        //                    baseline_immersion_avg = M_immersion_180.Average();
        //                    baseline_immersion_var = Statistics.Utilities.Variance(M_immersion_180);
        //                    #endregion

        //                    #region grossindex - min / max
        //                    Utilities.SetMinMax(
        //                        AVG_attention_180,
        //                        VAR_attention_180,
        //                        baseline_attention_avg,
        //                        baseline_attention_var,
        //                        baseline_slider_limit,
        //                        slider_limit,
        //                        ref min_grossIndex_attention,
        //                        ref max_grossIndex_attention);
        //                    Utilities.SetMinMax(
        //                        AVG_creativity_180,
        //                        VAR_creativity_180,
        //                        baseline_creativity_avg,
        //                        baseline_creativity_var,
        //                        baseline_slider_limit,
        //                        slider_limit,
        //                        ref min_grossIndex_creativity,
        //                        ref max_grossIndex_creativity);
        //                    Utilities.SetMinMax(
        //                        AVG_engagement_180,
        //                        VAR_engagement_180,
        //                        baseline_engagement_avg,
        //                        baseline_engagement_var,
        //                        baseline_slider_limit,
        //                        slider_limit,
        //                        ref min_grossIndex_engagement,
        //                        ref max_grossIndex_engagement);
        //                    Utilities.SetMinMax(
        //                        AVG_arousal_180,
        //                        VAR_arousal_180,
        //                        baseline_arousal_avg,
        //                        baseline_arousal_var,
        //                        baseline_slider_limit,
        //                        slider_limit,
        //                        ref min_grossIndex_arousal,
        //                        ref max_grossIndex_arousal);
        //                    Utilities.SetMinMax(
        //                        AVG_immersion_180,
        //                        VAR_immersion_180,
        //                        baseline_immersion_avg,
        //                        baseline_immersion_var,
        //                        baseline_slider_limit,
        //                        slider_limit,
        //                        ref min_grossIndex_immersion,
        //                        ref max_grossIndex_immersion);
        //                    #endregion

        //                    slider = 0;

        //                    baseline = true;
        //                }
        //                #endregion
        //                #region running
        //                else
        //                {
        //                    float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);

        //                    #region not valid data
        //                    if (previous_b == RelPower)
        //                    {
        //                        Thread.Sleep(100);
        //                    }
        //                    #endregion
        //                    #region valid data -> produce
        //                    else
        //                    {
        //                        #region chartValues
        //                        Chart(NFUC.ChartValuesAlpha1, _datetime, TG_DATA_ALPHA1);
        //                        Chart(NFUC.ChartValuesAlpha2, _datetime, TG_DATA_ALPHA2);
        //                        Chart(NFUC.ChartValuesBeta1, _datetime, TG_DATA_BETA1);
        //                        Chart(NFUC.ChartValuesBeta2, _datetime, TG_DATA_BETA2);
        //                        Chart(NFUC.ChartValuesGamma1, _datetime, TG_DATA_GAMMA1);
        //                        Chart(NFUC.ChartValuesGamma2, _datetime, TG_DATA_GAMMA2);
        //                        Chart(NFUC.ChartValuesDelta, _datetime, TG_DATA_DELTA);
        //                        Chart(NFUC.ChartValuesTheta, _datetime, TG_DATA_THETA);
        //                        //ChartValuesAlpha1.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_ALPHA1
        //                        //});

        //                        //ChartValuesAlpha2.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_ALPHA2
        //                        //});

        //                        //ChartValuesBeta1.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_BETA1
        //                        //});

        //                        //ChartValuesBeta2.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_BETA2
        //                        //});

        //                        //ChartValuesGamma1.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_GAMMA1
        //                        //});

        //                        //ChartValuesGamma2.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_GAMMA2
        //                        //});

        //                        //ChartValuesDelta.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_DELTA
        //                        //});

        //                        //ChartValuesTheta.Add(new DateChartModel
        //                        //{
        //                        //    DateTime = _datetime,
        //                        //    Value = TG_DATA_THETA
        //                        //});
        //                        #endregion

        //                        previous_b = RelPower;

        //                        attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                        creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
        //                        engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
        //                        arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                        immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

        //                        slider++;

        //                        if (slider == slider_limit)
        //                        {
        //                            slider = 0;
        //                            if (!baseline_run)
        //                                baseline_run = true;
        //                        }

        //                        if (baseline_run)
        //                        {
        //                            M_attention_180[baseline_slider] = Statistics.Utilities.Median(attention_30);
        //                            M_creativity_180[baseline_slider] = Statistics.Utilities.Median(creativity_30);
        //                            M_engagement_180[baseline_slider] = Statistics.Utilities.Median(engagement_30);
        //                            M_arousal_180[baseline_slider] = Statistics.Utilities.Median(arousal_30);
        //                            M_immersion_180[baseline_slider] = Statistics.Utilities.Median(immersion_30);
        //                            baseline_slider++;

        //                            int index = slider - 1;
        //                            if (index < 0)
        //                                index = slider_limit - 1;

        //                            M_attention_30[index] = Statistics.Utilities.Median(attention_30);
        //                            M_creativity_30[index] = Statistics.Utilities.Median(creativity_30);
        //                            M_engagement_30[index] = Statistics.Utilities.Median(engagement_30);
        //                            M_arousal_30[index] = Statistics.Utilities.Median(arousal_30);
        //                            M_immersion_30[index] = Statistics.Utilities.Median(immersion_30);

        //                            if (!baseline_minmax_run)
        //                            {
        //                                if (slider == slider_limit - 1)
        //                                    baseline_minmax_run = true;
        //                            }

        //                            if (baseline_minmax_run)
        //                            {
        //                                AVG_attention_180[baseline_slider] = M_attention_30.Average();
        //                                VAR_attention_180[baseline_slider] = Statistics.Utilities.Variance(M_attention_30);
        //                                AVG_creativity_180[baseline_slider] = M_creativity_30.Average();
        //                                VAR_creativity_180[baseline_slider] = Statistics.Utilities.Variance(M_creativity_30);
        //                                AVG_engagement_180[baseline_slider] = M_engagement_30.Average();
        //                                VAR_engagement_180[baseline_slider] = Statistics.Utilities.Variance(M_engagement_30);
        //                                AVG_arousal_180[baseline_slider] = M_arousal_30.Average();
        //                                VAR_arousal_180[baseline_slider] = Statistics.Utilities.Variance(M_arousal_30);
        //                                AVG_immersion_180[baseline_slider] = M_immersion_30.Average();
        //                                VAR_immersion_180[baseline_slider] = Statistics.Utilities.Variance(M_immersion_30);
        //                            }
        //                        }

        //                        Thread.Sleep(900);
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //            }
        //            #endregion
        //            #region processing
        //            else
        //            {
        //                #region not valid data
        //                if (Utilities.AreEqual(previous,
        //                    _datetime,
        //                    TG_DATA_RAW,
        //                    TG_DATA_ALPHA1,
        //                    TG_DATA_ALPHA2,
        //                    TG_DATA_BETA1,
        //                    TG_DATA_BETA2,
        //                    TG_DATA_DELTA,
        //                    TG_DATA_GAMMA1,
        //                    TG_DATA_GAMMA2,
        //                    TG_DATA_THETA))
        //                {
        //                    Thread.Sleep(100);
        //                }
        //                #endregion
        //                #region valid data -> produce
        //                else
        //                {
        //                    #region chartValues
        //                    //ChartValuesAlpha1.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_ALPHA1
        //                    //});

        //                    //ChartValuesAlpha2.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_ALPHA2
        //                    //});

        //                    //ChartValuesBeta1.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_BETA1
        //                    //});

        //                    //ChartValuesBeta2.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_BETA2
        //                    //});

        //                    //ChartValuesGamma1.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_GAMMA1
        //                    //});

        //                    //ChartValuesGamma2.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_GAMMA2
        //                    //});

        //                    //ChartValuesDelta.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_DELTA
        //                    //});

        //                    //ChartValuesTheta.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = TG_DATA_THETA
        //                    //});
        //                    #endregion

        //                    #region new parameters

        //                    float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);

        //                    attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                    creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
        //                    engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
        //                    arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
        //                    immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

        //                    M_attention_30[slider] = Statistics.Utilities.Median(attention_30);
        //                    M_creativity_30[slider] = Statistics.Utilities.Median(creativity_30);
        //                    M_engagement_30[slider] = Statistics.Utilities.Median(engagement_30);
        //                    M_arousal_30[slider] = Statistics.Utilities.Median(arousal_30);
        //                    M_immersion_30[slider] = Statistics.Utilities.Median(immersion_30);

        //                    slider++;

        //                    if (slider == slider_limit)
        //                        slider = 0;

        //                    float grossIndex_attention = Utilities.grossIndex(
        //                        M_attention_30.Average(),
        //                        Statistics.Utilities.Variance(M_attention_30),
        //                        slider_limit, baseline_attention_avg, baseline_attention_var, baseline_slider_limit);
        //                    float grossIndex_creativity = Utilities.grossIndex(
        //                        M_creativity_30.Average(),
        //                        Statistics.Utilities.Variance(M_creativity_30),
        //                        slider_limit, baseline_creativity_avg, baseline_creativity_var, baseline_slider_limit);
        //                    float grossIndex_engagement = Utilities.grossIndex(
        //                        M_engagement_30.Average(),
        //                        Statistics.Utilities.Variance(M_engagement_30),
        //                        slider_limit, baseline_engagement_avg, baseline_engagement_var, baseline_slider_limit);
        //                    float grossIndex_arousal = Utilities.grossIndex(
        //                        M_arousal_30.Average(),
        //                        Statistics.Utilities.Variance(M_arousal_30),
        //                        slider_limit, baseline_arousal_avg, baseline_arousal_var, baseline_slider_limit);
        //                    float grossIndex_immersion = Utilities.grossIndex(
        //                        M_immersion_30.Average(),
        //                        Statistics.Utilities.Variance(M_immersion_30),
        //                        slider_limit, baseline_immersion_avg, baseline_immersion_var, baseline_slider_limit);

        //                    float grossIndex_attention_normalized = Utilities.grossIndexNormalized(grossIndex_attention, min_grossIndex_attention, max_grossIndex_attention);
        //                    float grossIndex_creativity_normalized = Utilities.grossIndexNormalized(grossIndex_creativity, min_grossIndex_creativity, max_grossIndex_creativity);
        //                    float grossIndex_engagement_normalized = Utilities.grossIndexNormalized(grossIndex_engagement, min_grossIndex_engagement, max_grossIndex_engagement);
        //                    float grossIndex_arousal_normalized = Utilities.grossIndexNormalized(grossIndex_arousal, min_grossIndex_arousal, max_grossIndex_arousal);
        //                    float grossIndex_immersion_normalized = Utilities.grossIndexNormalized(grossIndex_immersion, min_grossIndex_immersion, max_grossIndex_immersion);

        //                    #endregion

        //                    #region chartValue
        //                    //ChartValuesAttention.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = grossIndex_attention_normalized
        //                    //});
        //                    //ChartValuesCreativity.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = grossIndex_creativity_normalized
        //                    //});
        //                    //ChartValuesImmersion.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = grossIndex_immersion_normalized
        //                    //});
        //                    //ChartValuesArousal.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = grossIndex_arousal_normalized
        //                    //});
        //                    //ChartValuesEngagement.Add(new DateChartModel
        //                    //{
        //                    //    DateTime = _datetime,
        //                    //    Value = grossIndex_engagement_normalized
        //                    //});
        //                    #endregion

        //                    Thread.Sleep(800);
        //                }
        //                #endregion
        //            }
        //            #endregion

        //            #region cancellation
        //            //if (token.IsCancellationRequested)
        //            //{
        //            //    Console.WriteLine("Task cancelled");
        //            //    token.ThrowIfCancellationRequested();
        //            //}
        //            #endregion

        //            #region visibility limit
        //            //SetAxisXLimits(_datetime);
        //            //if (ChartValuesAlpha1.Count > visibility_limit)
        //            //    ChartValuesAlpha1.RemoveAt(0);
        //            //if (ChartValuesAlpha2.Count > visibility_limit)
        //            //    ChartValuesAlpha2.RemoveAt(0);
        //            //if (ChartValuesBeta1.Count > visibility_limit)
        //            //    ChartValuesBeta1.RemoveAt(0);
        //            //if (ChartValuesBeta2.Count > visibility_limit)
        //            //    ChartValuesBeta2.RemoveAt(0);
        //            //if (ChartValuesGamma1.Count > visibility_limit)
        //            //    ChartValuesGamma1.RemoveAt(0);
        //            //if (ChartValuesGamma2.Count > visibility_limit)
        //            //    ChartValuesGamma2.RemoveAt(0);
        //            //if (ChartValuesDelta.Count > visibility_limit)
        //            //    ChartValuesDelta.RemoveAt(0);
        //            //if (ChartValuesTheta.Count > visibility_limit)
        //            //    ChartValuesTheta.RemoveAt(0);
        //            //if (ChartValuesAttention.Count > visibility_limit)
        //            //    ChartValuesAttention.RemoveAt(0);
        //            //if (ChartValuesCreativity.Count > visibility_limit)
        //            //    ChartValuesCreativity.RemoveAt(0);
        //            //if (ChartValuesImmersion.Count > visibility_limit)
        //            //    ChartValuesImmersion.RemoveAt(0);
        //            //if (ChartValuesArousal.Count > visibility_limit)
        //            //    ChartValuesArousal.RemoveAt(0);
        //            //if (ChartValuesEngagement.Count > visibility_limit)
        //            //    ChartValuesEngagement.RemoveAt(0);
        //            #endregion
        //        }
        //    }
        //    #endregion
        //    #region catch
        //    catch (OperationCanceledException)
        //    {

        //    }
        //    #endregion
        //}
        #endregion

        private void MockRead(CancellationToken token)
        {
            try
            {
                NFUC.ChartValuesAlpha1.Clear();
                SMBIUC.ChartValuesAttention.Clear();

                while (true)
                {
                    DateTime _dateTime = DateTime.Now;

                    float alpha1 = random.Next(0, 100);
                    NFUC.NeuroskyFrequencies = new NeuroskyFrequencies(_dateTime, alpha1, 0, 0, 0, 0, 0, 0, 0);
                    NFUC.SetAxisXLimits(_dateTime);
                    if (NFUC.ChartValuesAlpha1.Count > visibility_limit)
                        NFUC.ChartValuesAlpha1.RemoveAt(0);

                    float attention = random.Next(0, 100);
                    SMBIUC.SetMyBrainIndexes = new SetMyBrainIndexes(_dateTime, attention, 0, 0, 0, 0);
                    SMBIUC.SetAxisXLimits(_dateTime);
                    if (SMBIUC.ChartValuesAttention.Count > visibility_limit)
                        SMBIUC.ChartValuesAttention.RemoveAt(0);

                    float totalValue = alpha1 + attention;
                    Dispatcher.Invoke(() =>{
                        SUC.SmileIndexValue = "Index: " + totalValue;
                        }
                    );

                    if (totalValue >= 160)
                    {
                        SUC.RedValueVisibility = false;
                        SUC.OrangeValueVisibility = false;
                        SUC.YellowValueVisibility = false;
                        SUC.GreenValueVisibility = false;
                        SUC.Green2ValueVisibility = true;
                    }
                    else if (totalValue >= 120)
                    {
                        SUC.RedValueVisibility = false;
                        SUC.OrangeValueVisibility = false;
                        SUC.YellowValueVisibility = false;
                        SUC.GreenValueVisibility = true;
                        SUC.Green2ValueVisibility = false;
                    }
                    else if (totalValue >= 80)
                    {
                        SUC.RedValueVisibility = false;
                        SUC.OrangeValueVisibility = false;
                        SUC.YellowValueVisibility = true;
                        SUC.GreenValueVisibility = false;
                        SUC.Green2ValueVisibility = false;
                    }
                    else if (totalValue >= 40)
                    {
                        SUC.RedValueVisibility = false;
                        SUC.OrangeValueVisibility = true;
                        SUC.YellowValueVisibility = false;
                        SUC.GreenValueVisibility = false;
                        SUC.Green2ValueVisibility = false;
                    }
                    else 
                    {
                        SUC.RedValueVisibility = true;
                        SUC.OrangeValueVisibility = false;
                        SUC.YellowValueVisibility = false;
                        SUC.GreenValueVisibility = false;
                        SUC.Green2ValueVisibility = false;
                    }

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void InjectStopOnClick(object sender, RoutedEventArgs e)
        {
            Task readTask;
            if (!IsReading)
            {
                tokenSource = new CancellationTokenSource();

                IsReading = !IsReading;
                //readTask= Task.Run(() => collector.DoWork(), tokenSource.Token);
                readTask = Task.Run(() => this.MockRead(tokenSource.Token), tokenSource.Token);
            }
            else
            {
                IsReading = !IsReading;
                tokenSource.Cancel();
            }                
        }

        //private void Chart(ChartValues<DateChartModel> chartValues, DateTime datetime, float TG_DATA)
        //{
        //    chartValues.Add(new DateChartModel
        //    {
        //        DateTime = datetime,
        //        Value = TG_DATA
        //    });
        //}
    }
}