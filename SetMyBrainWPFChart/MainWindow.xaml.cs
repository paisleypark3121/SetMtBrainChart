﻿using AppSettings;
using Connector;
using LiveCharts;
using SetMyBrainWPFChart.Log;
using SetMyBrainWPFChart.Neurosky;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetMyBrainWPFChart
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region SOCIAL 

        #endregion

        public bool transparency = false;

        Random random = new Random();
        ICollector collector = null;
        CancellationTokenSource tokenSource = null;
        CancellationTokenSource tokenTimeUserControl = null;

        #region AppSettings
        IAppSettings appSettings = null;
        #endregion

        #region ThinkGear
        IConnector connector = null;
        #endregion

        #region IHandler
        IHandler handler = null;
        #endregion

        #region appsettings keys
        private int visibility_limit = 150;
        string tag = "SetMyBrain";
        private string startup = "mock";
        Dictionary<string,ILog> log = null;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            this.Top = 0;
            this.Left = 0;

            appSettings = new AppSettings.AppSettings();
            connector = new ThinkgearConnector(new string[] { appSettings["COM"] });
            visibility_limit = int.Parse(appSettings["visibility_limit"]);
            startup = appSettings["startup"];
            if (string.IsNullOrEmpty(startup))
                startup = "mock";
            if ((startup != "mock") && (startup != "real"))
                startup = "mock";
            tag = appSettings["tag"];
            bool _log = false;
            bool.TryParse(appSettings["log"].ToString(),out _log);
            if (_log)
            {
                string folder_name = DateTime.Now.Date.ToString("yyyy-MM-dd");
                if (!Directory.Exists(folder_name))
                    Directory.CreateDirectory(folder_name);
                string fileNameFrequencies = folder_name+"/"+tag + " - Frequencies - " + DateTime.Now.Ticks+".csv";
                ILog logFrequencies = new FileLog(new string[] { fileNameFrequencies });
                string fileNameIndexes = folder_name + "/" + tag + " - Indexes - " + DateTime.Now.Ticks + ".csv";
                ILog logIndexes = new FileLog(new string[] { fileNameIndexes });
                string fileNameSlopes = folder_name + "/" + tag + " - Slopes - " + DateTime.Now.Ticks + ".csv";
                ILog logSlopes = new FileLog(new string[] { fileNameSlopes });

                log = new Dictionary<string, ILog>();
                log.Add("Frequencies", logFrequencies);
                log.Add("Indexes", logIndexes);
                log.Add("Slopes", logSlopes);

                log["Frequencies"].Trace("timestamp,TG_DATA_ALPHA1,TG_DATA_ALPHA2,TG_DATA_BETA1,TG_DATA_BETA2,TG_DATA_GAMMA1,TG_DATA_GAMMA2,TG_DATA_DELTA,TG_DATA_THETA");
                log["Indexes"].Trace("timestamp,attention,creativity,engagement,arousal,immersion");
                log["Slopes"].Trace("timestamp,slopeThetaRelPower,slopeBetaLowRelPower,slopeAlphaHighRelPower,slopePower");
            }

            handler = new SMBCHandler(SMBC, CUC, SUC);

            PSUC.PropertyChanged += PSUCChangedEventHandler;
            WUC.PropertyChanged += WUCChangedEventHandler;

            TUC.Now = DateTime.Now;

            SMBC.visibility_limit = visibility_limit;

            string _collector = appSettings["collector"];
            if (_collector == "normal")
            {
                collector = new Collector(
                    null,
                    appSettings,
                    connector,
                    handler,
                    log
                    );
            }
            else if (_collector == "smart")
            {
                collector = new SmartCollector(
                    null,
                    appSettings,
                    connector,
                    handler,
                    log
                    );
            }
            else
                throw new Exception("Error in starting application");

            bool _visibilityChart = false;
            SMBC.Alpha1SeriesVisibility= (bool.TryParse(appSettings["Alpha1"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.Alpha2SeriesVisibility = (bool.TryParse(appSettings["Alpha2"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.Beta1SeriesVisibility = (bool.TryParse(appSettings["Beta1"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.Beta2SeriesVisibility = (bool.TryParse(appSettings["Beta2"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.Gamma1SeriesVisibility = (bool.TryParse(appSettings["Gamma1"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.Gamma2SeriesVisibility = (bool.TryParse(appSettings["Gamma2"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.DeltaSeriesVisibility = (bool.TryParse(appSettings["Delta"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.ThetaSeriesVisibility = (bool.TryParse(appSettings["Theta"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.AttentionSeriesVisibility = (bool.TryParse(appSettings["Attention"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.CreativitySeriesVisibility = (bool.TryParse(appSettings["Creativity"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.EngagementSeriesVisibility = (bool.TryParse(appSettings["Engagement"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.ArousalSeriesVisibility = (bool.TryParse(appSettings["Arousal"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.ImmersionSeriesVisibility = (bool.TryParse(appSettings["Immersion"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.SlopeAlphaSeriesVisibility = (bool.TryParse(appSettings["SlopeAlpha"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.SlopeBetaSeriesVisibility = (bool.TryParse(appSettings["SlopeBeta"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.SlopeThetaSeriesVisibility = (bool.TryParse(appSettings["SlopeTheta"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;
            SMBC.SlopePowerSeriesVisibility = (bool.TryParse(appSettings["SlopePower"].ToString(), out _visibilityChart) == true) ? _visibilityChart : true;

            IsReading = false;

            Task.Run(() => this.TimeTask());

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
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

        private void TimeTask()
        {
            while (true)
            {
                Dispatcher.Invoke(
                    () => {
                        TUC.Now = DateTime.Now;
                    });
                Thread.Sleep(1000);
            }
        }

        private void StartStopTimeTask(CancellationToken token)
        {
            #region NOW
            Dispatcher.Invoke(
                () => {
                    TUC.Start = DateTime.Now;
                });
            #endregion

            #region ELAPSED
            Stopwatch sw = new Stopwatch();

            try
            {
                while (true)
                {
                    sw.Start();
                    Dispatcher.Invoke(
                        () =>
                        {
                            TUC.Elapsed = sw.Elapsed;
                        });
                    Thread.Sleep(1000);

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("CANCELLED SSTT");
                sw.Stop();
            }
            #endregion
        }

        private void MockRead(CancellationToken token)
        {
            try
            {
                SMBC.ChartValuesAlpha1.Clear();
                SMBC.ChartValuesAttention.Clear();
                SMBC.ChartValuesSlopePower.Clear();

                while (true)
                {
                    DateTime _dateTime = DateTime.Now;

                    float alpha1 = random.Next(0, 100);
                    SMBC.NeuroskyFrequencies = new NeuroskyFrequencies(_dateTime, alpha1, 0, 0, 0, 0, 0, 0, 0);
                    SMBC.SetAxisXLimits(_dateTime);
                    if (SMBC.ChartValuesAlpha1.Count > visibility_limit)
                        SMBC.ChartValuesAlpha1.RemoveAt(0);

                    float attention = random.Next(0, 100);
                    SMBC.SetMyBrainIndexes = new SetMyBrainIndexes(_dateTime, attention, 0, 0, 0, 0);
                    SMBC.SetAxisXLimits(_dateTime);
                    if (SMBC.ChartValuesAttention.Count > visibility_limit)
                        SMBC.ChartValuesAttention.RemoveAt(0);

                    float power = random.Next(0, 100);
                    SMBC.SetMyBrainSlopes = new SetMyBrainSlopes(_dateTime, 0, 0, 0, power);
                    SMBC.SetAxisXLimits(_dateTime);
                    if (SMBC.ChartValuesSlopePower.Count > visibility_limit)
                        SMBC.ChartValuesSlopePower.RemoveAt(0);

                    //float creativity = random.Next(0, 100);
                    //Dispatcher.Invoke(() => {
                    //    ITFUC.SetMyBrainIndexes = new SetMyBrainIndexes(_dateTime, attention, creativity, 0, 0, 0);
                    //});

                    float poor_signal = random.Next(0, 255);

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

                    float poorSignal = random.Next(0, 255);
                    Dispatcher.Invoke(() =>
                    {
                        CUC.PoorSignal = poorSignal;
                    });

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void MockReadHandler(CancellationToken token)
        {
            try
            {
                IHandler handler = new SMBCHandler(SMBC, CUC, SUC);
                
                
                while (true)
                {   
                    DateTime _dateTime = DateTime.Now;
                    float alpha1 = random.Next(0, 100);
                    float poorSignal = random.Next(0, 255);
                    NeuroskyFrequencies NeuroskyFrequencies = new NeuroskyFrequencies(_dateTime, alpha1, 0, 0, 0, 0, 0, 0, 0);
                    handler.HandleFrequencies(NeuroskyFrequencies);
                    handler.HandlePoorSignal(poorSignal);

                    //SMBC.NeuroskyFrequencies = new NeuroskyFrequencies(_dateTime, alpha1, 0, 0, 0, 0, 0, 0, 0); // generates property changed

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void PSUCChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            Task readTask;
            Task timeTask;
            if (!IsReading)
            {
                tokenSource = new CancellationTokenSource();
                tokenTimeUserControl = new CancellationTokenSource();

                IsReading = !IsReading;
                if (startup == "mock")
                    //readTask = Task.Run(() => this.MockRead(tokenSource.Token), tokenSource.Token);
                    readTask = Task.Run(() => this.MockReadHandler(tokenSource.Token), tokenSource.Token);
                else
                    readTask = Task.Run(() => collector.DoWorkAsync(tokenSource.Token), tokenSource.Token);

                timeTask = Task.Run(() => this.StartStopTimeTask(tokenTimeUserControl.Token), tokenTimeUserControl.Token);
            }
            else
            {
                IsReading = !IsReading;
                tokenSource.Cancel();
                tokenTimeUserControl.Cancel();
            }
        }

        private void WUCChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "move")
            {
                move();
            }

            if (e.PropertyName == "minimize")
            {   
                WindowState = WindowState.Minimized;
            }

            if (e.PropertyName == "close")
            {
                Application.Current.Shutdown();
            }

            if (e.PropertyName == "anchor")
            {
                this.Topmost = !this.Topmost;
            }

            if (e.PropertyName == "opacity")
            {
                opacity();
            }
        }

        public double current_width = 0;
        public double current_height = 0;

        private void minimize()
        {
            if (WUC.Visibility == Visibility.Visible)
            {
                LoginUC.Visibility = Visibility.Hidden;
                SMBC.Visibility = Visibility.Hidden;
                CUC.Visibility = Visibility.Hidden;
                PSUC.Visibility = Visibility.Hidden;
                WUC.Visibility = Visibility.Hidden;
                TUC.Visibility = Visibility.Hidden;
                WUC.Visibility = Visibility.Hidden;
                current_width = Width;
                current_height = Height;
                Width = 90;
                Height = 80;

            }
            else
            {
                LoginUC.Visibility = Visibility.Visible;
                SMBC.Visibility = Visibility.Visible;
                CUC.Visibility = Visibility.Visible;
                PSUC.Visibility = Visibility.Visible;
                WUC.Visibility = Visibility.Visible;
                TUC.Visibility = Visibility.Visible;
                WUC.Visibility = Visibility.Visible;
                Width = current_width;
                Height = current_height;
            }
        }

        private void move()
        {
            if ((this.Top == 0) && (this.Left == 0))
            {
                this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
            }
            else if ((this.Top == 0) && (this.Left == SystemParameters.PrimaryScreenWidth - this.Width))
            {
                this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
            }
            else if ((this.Top == SystemParameters.PrimaryScreenHeight - this.Height) && (this.Left == SystemParameters.PrimaryScreenWidth - this.Width))
            {
                this.Left = 0;
            }
            else if ((this.Top == SystemParameters.PrimaryScreenHeight - this.Height) && (this.Left == 0))
            {
                this.Top = 0;
            }
        }

        private void opacity()
        {
            if (this.Opacity == 1)
                this.Opacity = 0.8;
            else if (this.Opacity == 0.8)
                this.Opacity = 0.6;
            else if (this.Opacity == 0.6)
                this.Opacity = 0.4;
            else if (this.Opacity == 0.4)
                this.Opacity = 0.2;
            else
                this.Opacity = 1;
        }

        private void SUC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            minimize();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (Width > 550)
                    Width -= 40;
            }

            if (e.Key == Key.Right)
            {
                Width += 40;
            }

            if (e.Key == Key.Up)
            {
                if (Height > 440)
                    Height -= 40;
            }

            if (e.Key == Key.Down)
            {
                Height += 40;
            }

            if ((e.Key == Key.LeftCtrl) || (e.Key == Key.LeftCtrl))
            {
                minimize();
            }

            if (e.Key == Key.Tab)
            {
                move();
            }

            if (e.Key == Key.O)
            {
                opacity();
            }

            if (e.Key ==Key.A)
            {
                this.Topmost = !this.Topmost;
            }
        }
    }
}