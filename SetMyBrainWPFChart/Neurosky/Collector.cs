﻿using AppSettings;
using Connector;
using SetMyBrainWPFChart.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Neurosky
{
    public class Collector : ICollector,IDisposable
    {
        #region variables
        private IAppSettings appSettings = null;
        //private CancellationToken token;
        private IConnector connector;
        private IHandler handler;
        private Dictionary<string,ILog> log;

        private string[] args = null;
        #endregion

        public Collector(
            string[] _args,
            IAppSettings _appSettings,
            IConnector _connector,
            IHandler _handler,
            Dictionary<string, ILog> _log
            //,CancellationTokenSource tokenSource
            )
        {
            #region variables
            args = _args;
            appSettings = _appSettings;
            handler = _handler;
            //token = tokenSource.Token;
            connector = _connector;
            log = _log;
            #endregion

            connector.Connect();
        }

        public void Dispose()
        {
            connector.Disconnect();
        }

        public async Task DoWorkAsync(CancellationToken token)
        {
            #region structure
            Dictionary<string, object> previous = null;
            float previous_b = 0;

            #region baselines

            bool baseline = false;
            bool baseline_run = false;
            bool baseline_minmax_run = false;
            int slider = 0;
            int slider_limit = 30;
            int baseline_slider = 0;
            int baseline_slider_limit = 180;

            #region BASELINE - AVG & VAR 180 sec. (M_attention_180 & M_creativity_180)
            float baseline_attention_avg = 0;
            float baseline_attention_var = 0;
            float baseline_creativity_avg = 0;
            float baseline_creativity_var = 0;
            float baseline_engagement_avg = 0;
            float baseline_engagement_var = 0;
            float baseline_arousal_avg = 0;
            float baseline_arousal_var = 0;
            float baseline_immersion_avg = 0;
            float baseline_immersion_var = 0;
            #endregion

            #region BASELINE - min / max AVG & VAR 180 sec. (based on calculations on 30 sec. window)
            float min_grossIndex_attention = 999999;
            float max_grossIndex_attention = -999999;
            float min_grossIndex_creativity = 999999;
            float max_grossIndex_creativity = -999999;
            float min_grossIndex_engagement = 999999;
            float max_grossIndex_engagement = -999999;
            float min_grossIndex_arousal = 999999;
            float max_grossIndex_arousal = -999999;
            float min_grossIndex_immersion = 999999;
            float max_grossIndex_immersion = -999999;
            #endregion

            #region 30 sec. buffer for punctual -> each element contains value
            float[] attention_30 = new float[slider_limit];
            float[] creativity_30 = new float[slider_limit];
            float[] engagement_30 = new float[slider_limit];
            float[] arousal_30 = new float[slider_limit];
            float[] immersion_30 = new float[slider_limit];
            #endregion

            #region 30 sec. buffer for medians -> each element contains Median of last 30 sec. (attention_30, creativity_30 ...)
            float[] M_attention_30 = new float[slider_limit];
            float[] M_creativity_30 = new float[slider_limit];
            float[] M_engagement_30 = new float[baseline_slider_limit];
            float[] M_arousal_30 = new float[baseline_slider_limit];
            float[] M_immersion_30 = new float[baseline_slider_limit];
            #endregion

            #region 180 sec. buffer for medians -> each element contains Median of last 30 sec. (attention_30, creativity_30 ...)
            float[] M_attention_180 = new float[baseline_slider_limit];
            float[] M_creativity_180 = new float[baseline_slider_limit];
            float[] M_engagement_180 = new float[baseline_slider_limit];
            float[] M_arousal_180 = new float[baseline_slider_limit];
            float[] M_immersion_180 = new float[baseline_slider_limit];
            #endregion

            #region 180 sec. buffer for avg -> each element contains avg of Median of last 30 sec. (avg M_attention_30)
            float[] AVG_attention_180 = new float[baseline_slider_limit];
            float[] AVG_creativity_180 = new float[baseline_slider_limit];
            float[] AVG_engagement_180 = new float[baseline_slider_limit];
            float[] AVG_arousal_180 = new float[baseline_slider_limit];
            float[] AVG_immersion_180 = new float[baseline_slider_limit];
            #endregion

            #region 180 sec. buffer for var -> each element contains var of Median of last 30 sec. (var M_attention_30)
            float[] VAR_attention_180 = new float[baseline_slider_limit];
            float[] VAR_creativity_180 = new float[baseline_slider_limit];
            float[] VAR_engagement_180 = new float[baseline_slider_limit];
            float[] VAR_arousal_180 = new float[baseline_slider_limit];
            float[] VAR_immersion_180 = new float[baseline_slider_limit];
            #endregion

            #region 30 sec. initialization
            for (int i = 0; i < slider_limit; i++)
            {
                attention_30[i] = 0;
                creativity_30[i] = 0;
                engagement_30[i] = 0;
                arousal_30[i] = 0;
                immersion_30[i] = 0;

                M_attention_30[i] = 0;
                M_creativity_30[i] = 0;
                M_engagement_30[i] = 0;
                M_arousal_30[i] = 0;
                M_immersion_30[i] = 0;
            }
            #endregion

            #region 180 sec. initialization
            for (int i = 0; i < baseline_slider_limit; i++)
            {
                M_attention_180[i] = 0;
                M_creativity_180[i] = 0;
                M_engagement_180[i] = 0;
                M_arousal_180[i] = 0;
                M_immersion_180[i] = 0;

                AVG_attention_180[i] = 0;
                AVG_creativity_180[i] = 0;
                AVG_engagement_180[i] = 0;
                AVG_arousal_180[i] = 0;
                AVG_immersion_180[i] = 0;

                VAR_attention_180[i] = 0;
                VAR_creativity_180[i] = 0;
                VAR_engagement_180[i] = 0;
                VAR_arousal_180[i] = 0;
                VAR_immersion_180[i] = 0;
            }
            #endregion

            Stopwatch stopWatch_baseline = new Stopwatch();

            #endregion

            #region flow
            float[] SlopeThetaRelPower = new float[10];
            float[] SlopeAlphaHighRelPower = new float[10];
            float[] SlopeBetaLowRelPower = new float[10];
            float[] SlopePower = new float[10];

            for (int i=0;i<10;i++)
            {
                SlopeThetaRelPower[i] = 0;
                SlopeAlphaHighRelPower[i] = 0;
                SlopeBetaLowRelPower[i] = 0;
                SlopePower[i] = 0;
            }
            #endregion

            #endregion
            #region try
            try
            {
                while (true)
                {
                    #region timing
                    DateTime _datetime = DateTime.Now;
                    #endregion

                    #region get data      
                    int limit = 1000;
                    int count = limit;
                    while ((count > 0) && !(bool)connector.GetData())
                    {
                        Console.WriteLine("Connecting... " + (limit - count + 1).ToString() + "/" + limit);
                        Thread.Sleep(100);
                        count--;
                    }
                    if (count == 0)
                    {
                        string error = "Cannot connect to headset: please restart";
                        Console.WriteLine(error);
                        throw new Exception(error);
                    }
                    #endregion

                    #region get values
                    float TG_DATA_RAW = (float)connector.GetValue(4);
                    float TG_DATA_ALPHA1 = (float)connector.GetValue(7);
                    float TG_DATA_ALPHA2 = (float)connector.GetValue(8);
                    float TG_DATA_BETA1 = (float)connector.GetValue(9);
                    float TG_DATA_BETA2 = (float)connector.GetValue(10);
                    float TG_DATA_DELTA = (float)connector.GetValue(5);
                    float TG_DATA_GAMMA1 = (float)connector.GetValue(11);
                    float TG_DATA_GAMMA2 = (float)connector.GetValue(12);
                    float TG_DATA_THETA = (float)connector.GetValue(6);
                    float TG_DATA_MEDITATION = (float)connector.GetValue(3);
                    float TG_DATA_ATTENTION = (float)connector.GetValue(2);
                    float TG_DATA_POOR_SIGNAL = (float)connector.GetValue(1);
                    #endregion

                    handler.HandlePoorSignal(TG_DATA_POOR_SIGNAL);

                    #region baseline calculation
                    if (!baseline)
                    {
                        #region start
                        if (!stopWatch_baseline.IsRunning)
                        {
                            if ((TG_DATA_ALPHA1 != 0) &&
                                (TG_DATA_ALPHA2 != 0) &&
                                (TG_DATA_THETA != 0))
                            {
                                stopWatch_baseline.Start();
                                float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);
                                previous_b = RelPower;

                                attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                                creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
                                engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
                                arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                                immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

                                slider++;

                                Utilities.SetSlopes(
                                    ref SlopeThetaRelPower, 
                                    ref SlopeBetaLowRelPower, 
                                    ref SlopeAlphaHighRelPower, 
                                    ref SlopePower, 
                                    TG_DATA_ALPHA2, 
                                    TG_DATA_BETA1, 
                                    TG_DATA_DELTA, 
                                    RelPower);

                                float slopeThetaRelPower = Statistics.Utilities.Slope(SlopeThetaRelPower);
                                float slopeBetaLowRelPower = Statistics.Utilities.Slope(SlopeBetaLowRelPower);
                                float slopeAlphaHighRelPower = Statistics.Utilities.Slope(SlopeAlphaHighRelPower);
                                float slopePower = Statistics.Utilities.Slope(SlopePower);

                                await Utilities.LogSlopesAsync(log, _datetime, slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                                #region chartValues
                                handler.HandleSlopes(
                                    new SetMyBrainSlopes(
                                        _datetime,
                                        slopeThetaRelPower,
                                        slopeBetaLowRelPower,
                                        slopeAlphaHighRelPower,
                                        slopePower
                                    )
                                );
                                #endregion

                                bool flow =Utilities.InTheFlow(slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                                handler.HandleFlow(flow);
                            }
                        }
                        #endregion
                        #region stop
                        else if ((baseline_slider == baseline_slider_limit - 1) && (!baseline))
                        {
                            stopWatch_baseline.Stop();

                            #region baseline calculation
                            baseline_attention_avg = M_attention_180.Average();
                            baseline_attention_var = Statistics.Utilities.Variance(M_attention_180);
                            baseline_creativity_avg = M_creativity_180.Average();
                            baseline_creativity_var = Statistics.Utilities.Variance(M_creativity_180);
                            baseline_engagement_avg = M_engagement_180.Average();
                            baseline_engagement_var = Statistics.Utilities.Variance(M_engagement_180);
                            baseline_arousal_avg = M_arousal_180.Average();
                            baseline_arousal_var = Statistics.Utilities.Variance(M_arousal_180);
                            baseline_immersion_avg = M_immersion_180.Average();
                            baseline_immersion_var = Statistics.Utilities.Variance(M_immersion_180);
                            #endregion

                            #region grossindex - min / max
                            Utilities.SetMinMax(
                                AVG_attention_180,
                                VAR_attention_180,
                                baseline_attention_avg,
                                baseline_attention_var,
                                baseline_slider_limit,
                                slider_limit,
                                ref min_grossIndex_attention,
                                ref max_grossIndex_attention);
                            Utilities.SetMinMax(
                                AVG_creativity_180,
                                VAR_creativity_180,
                                baseline_creativity_avg,
                                baseline_creativity_var,
                                baseline_slider_limit,
                                slider_limit,
                                ref min_grossIndex_creativity,
                                ref max_grossIndex_creativity);
                            Utilities.SetMinMax(
                                AVG_engagement_180,
                                VAR_engagement_180,
                                baseline_engagement_avg,
                                baseline_engagement_var,
                                baseline_slider_limit,
                                slider_limit,
                                ref min_grossIndex_engagement,
                                ref max_grossIndex_engagement);
                            Utilities.SetMinMax(
                                AVG_arousal_180,
                                VAR_arousal_180,
                                baseline_arousal_avg,
                                baseline_arousal_var,
                                baseline_slider_limit,
                                slider_limit,
                                ref min_grossIndex_arousal,
                                ref max_grossIndex_arousal);
                            Utilities.SetMinMax(
                                AVG_immersion_180,
                                VAR_immersion_180,
                                baseline_immersion_avg,
                                baseline_immersion_var,
                                baseline_slider_limit,
                                slider_limit,
                                ref min_grossIndex_immersion,
                                ref max_grossIndex_immersion);
                            #endregion

                            slider = 0;

                            baseline = true;
                        }
                        #endregion
                        #region running
                        else
                        {
                            float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);

                            #region not valid data
                            if (previous_b == RelPower)
                            {
                                Thread.Sleep(100);
                            }
                            #endregion
                            #region valid data -> produce
                            else
                            {
                                #region chartValues
                                handler.HandleFrequencies(
                                    new NeuroskyFrequencies(
                                        _datetime,
                                        TG_DATA_ALPHA1,
                                        TG_DATA_ALPHA2,
                                        TG_DATA_BETA1,
                                        TG_DATA_BETA2,
                                        TG_DATA_GAMMA1,
                                        TG_DATA_GAMMA2,
                                        TG_DATA_DELTA,
                                        TG_DATA_THETA
                                    )
                                );
                                #endregion

                                await Utilities.LogFrequenciesAsync(
                                    log,
                                    _datetime,
                                    TG_DATA_ALPHA1,
                                    TG_DATA_ALPHA2,
                                    TG_DATA_BETA1,
                                    TG_DATA_BETA2,
                                    TG_DATA_GAMMA1,
                                    TG_DATA_GAMMA2,
                                    TG_DATA_DELTA,
                                    TG_DATA_THETA);

                                Utilities.SetSlopes(
                                    ref SlopeThetaRelPower,
                                    ref SlopeBetaLowRelPower,
                                    ref SlopeAlphaHighRelPower,
                                    ref SlopePower,
                                    TG_DATA_ALPHA2,
                                    TG_DATA_BETA1,
                                    TG_DATA_DELTA,
                                    RelPower);

                                float slopeThetaRelPower = Statistics.Utilities.Slope(SlopeThetaRelPower);
                                float slopeBetaLowRelPower = Statistics.Utilities.Slope(SlopeBetaLowRelPower);
                                float slopeAlphaHighRelPower = Statistics.Utilities.Slope(SlopeAlphaHighRelPower);
                                float slopePower = Statistics.Utilities.Slope(SlopePower);

                                await Utilities.LogSlopesAsync(log, _datetime, slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                                #region chartValues
                                handler.HandleSlopes(
                                    new SetMyBrainSlopes(
                                        _datetime,
                                        slopeThetaRelPower,
                                        slopeBetaLowRelPower,
                                        slopeAlphaHighRelPower,
                                        slopePower
                                    )
                                );
                                #endregion

                                bool flow = Utilities.InTheFlow(slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                                handler.HandleFlow(flow);

                                previous_b = RelPower;

                                attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                                creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
                                engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
                                arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                                immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

                                slider++;

                                if (slider == slider_limit)
                                {
                                    slider = 0;
                                    if (!baseline_run)
                                        baseline_run = true;
                                }

                                if (baseline_run)
                                {
                                    M_attention_180[baseline_slider] = Statistics.Utilities.Median(attention_30);
                                    M_creativity_180[baseline_slider] = Statistics.Utilities.Median(creativity_30);
                                    M_engagement_180[baseline_slider] = Statistics.Utilities.Median(engagement_30);
                                    M_arousal_180[baseline_slider] = Statistics.Utilities.Median(arousal_30);
                                    M_immersion_180[baseline_slider] = Statistics.Utilities.Median(immersion_30);
                                    baseline_slider++;

                                    int index = slider - 1;
                                    if (index < 0)
                                        index = slider_limit - 1;

                                    M_attention_30[index] = Statistics.Utilities.Median(attention_30);
                                    M_creativity_30[index] = Statistics.Utilities.Median(creativity_30);
                                    M_engagement_30[index] = Statistics.Utilities.Median(engagement_30);
                                    M_arousal_30[index] = Statistics.Utilities.Median(arousal_30);
                                    M_immersion_30[index] = Statistics.Utilities.Median(immersion_30);

                                    if (!baseline_minmax_run)
                                    {
                                        if (slider == slider_limit - 1)
                                            baseline_minmax_run = true;
                                    }

                                    if (baseline_minmax_run)
                                    {
                                        AVG_attention_180[baseline_slider] = M_attention_30.Average();
                                        VAR_attention_180[baseline_slider] = Statistics.Utilities.Variance(M_attention_30);
                                        AVG_creativity_180[baseline_slider] = M_creativity_30.Average();
                                        VAR_creativity_180[baseline_slider] = Statistics.Utilities.Variance(M_creativity_30);
                                        AVG_engagement_180[baseline_slider] = M_engagement_30.Average();
                                        VAR_engagement_180[baseline_slider] = Statistics.Utilities.Variance(M_engagement_30);
                                        AVG_arousal_180[baseline_slider] = M_arousal_30.Average();
                                        VAR_arousal_180[baseline_slider] = Statistics.Utilities.Variance(M_arousal_30);
                                        AVG_immersion_180[baseline_slider] = M_immersion_30.Average();
                                        VAR_immersion_180[baseline_slider] = Statistics.Utilities.Variance(M_immersion_30);
                                    }
                                }

                                Thread.Sleep(900);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region processing
                    else
                    {
                        #region not valid data
                        if (Utilities.AreEqual(previous,
                            _datetime,
                            TG_DATA_RAW,
                            TG_DATA_ALPHA1,
                            TG_DATA_ALPHA2,
                            TG_DATA_BETA1,
                            TG_DATA_BETA2,
                            TG_DATA_DELTA,
                            TG_DATA_GAMMA1,
                            TG_DATA_GAMMA2,
                            TG_DATA_THETA))
                        {
                            Thread.Sleep(100);
                        }
                        #endregion
                        #region valid data -> produce
                        else
                        {
                            #region chartValues
                            handler.HandleFrequencies(
                                new NeuroskyFrequencies(
                                    _datetime,
                                    TG_DATA_ALPHA1,
                                    TG_DATA_ALPHA2,
                                    TG_DATA_BETA1,
                                    TG_DATA_BETA2,
                                    TG_DATA_GAMMA1,
                                    TG_DATA_GAMMA2,
                                    TG_DATA_DELTA,
                                    TG_DATA_THETA
                                )
                            );
                            #endregion

                            await Utilities.LogFrequenciesAsync(
                                log, 
                                _datetime,
                                TG_DATA_ALPHA1,
                                TG_DATA_ALPHA2,
                                TG_DATA_BETA1,
                                TG_DATA_BETA2,
                                TG_DATA_GAMMA1,
                                TG_DATA_GAMMA2,
                                TG_DATA_DELTA,
                                TG_DATA_THETA);

                            #region new parameters

                            float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);

                            attention_30[slider] = Utilities.Attention(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                            creativity_30[slider] = Utilities.Creativity(TG_DATA_ALPHA2, RelPower);
                            engagement_30[slider] = Utilities.Engagement(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_THETA);
                            arousal_30[slider] = Utilities.Arousal(TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_ALPHA1, TG_DATA_ALPHA2);
                            immersion_30[slider] = Utilities.Immersion(TG_DATA_THETA, TG_DATA_ALPHA1, TG_DATA_ALPHA2);

                            M_attention_30[slider] = Statistics.Utilities.Median(attention_30);
                            M_creativity_30[slider] = Statistics.Utilities.Median(creativity_30);
                            M_engagement_30[slider] = Statistics.Utilities.Median(engagement_30);
                            M_arousal_30[slider] = Statistics.Utilities.Median(arousal_30);
                            M_immersion_30[slider] = Statistics.Utilities.Median(immersion_30);

                            slider++;

                            if (slider == slider_limit)
                                slider = 0;

                            float grossIndex_attention = Utilities.grossIndex(
                                M_attention_30.Average(),
                                Statistics.Utilities.Variance(M_attention_30),
                                slider_limit, baseline_attention_avg, baseline_attention_var, baseline_slider_limit);
                            float grossIndex_creativity = Utilities.grossIndex(
                                M_creativity_30.Average(),
                                Statistics.Utilities.Variance(M_creativity_30),
                                slider_limit, baseline_creativity_avg, baseline_creativity_var, baseline_slider_limit);
                            float grossIndex_engagement = Utilities.grossIndex(
                                M_engagement_30.Average(),
                                Statistics.Utilities.Variance(M_engagement_30),
                                slider_limit, baseline_engagement_avg, baseline_engagement_var, baseline_slider_limit);
                            float grossIndex_arousal = Utilities.grossIndex(
                                M_arousal_30.Average(),
                                Statistics.Utilities.Variance(M_arousal_30),
                                slider_limit, baseline_arousal_avg, baseline_arousal_var, baseline_slider_limit);
                            float grossIndex_immersion = Utilities.grossIndex(
                                M_immersion_30.Average(),
                                Statistics.Utilities.Variance(M_immersion_30),
                                slider_limit, baseline_immersion_avg, baseline_immersion_var, baseline_slider_limit);

                            float grossIndex_attention_normalized = Utilities.grossIndexNormalized(grossIndex_attention, min_grossIndex_attention, max_grossIndex_attention);
                            float grossIndex_creativity_normalized = Utilities.grossIndexNormalized(grossIndex_creativity, min_grossIndex_creativity, max_grossIndex_creativity);
                            float grossIndex_engagement_normalized = Utilities.grossIndexNormalized(grossIndex_engagement, min_grossIndex_engagement, max_grossIndex_engagement);
                            float grossIndex_arousal_normalized = Utilities.grossIndexNormalized(grossIndex_arousal, min_grossIndex_arousal, max_grossIndex_arousal);
                            float grossIndex_immersion_normalized = Utilities.grossIndexNormalized(grossIndex_immersion, min_grossIndex_immersion, max_grossIndex_immersion);

                            await Utilities.LogIndexesAsync(
                                log, _datetime, 
                                grossIndex_attention_normalized, 
                                grossIndex_creativity_normalized, 
                                grossIndex_engagement_normalized, 
                                grossIndex_arousal_normalized, 
                                grossIndex_immersion_normalized);

                            #endregion

                            #region chartValues
                            handler.HandleIndexes(
                                new SetMyBrainIndexes(
                                    _datetime,
                                    grossIndex_attention_normalized,
                                    grossIndex_creativity_normalized,
                                    grossIndex_immersion_normalized,
                                    grossIndex_arousal_normalized,
                                    grossIndex_engagement_normalized
                                )
                            );
                            #endregion

                            Utilities.SetSlopes(
                                    ref SlopeThetaRelPower,
                                    ref SlopeBetaLowRelPower,
                                    ref SlopeAlphaHighRelPower,
                                    ref SlopePower,
                                    TG_DATA_ALPHA2,
                                    TG_DATA_BETA1,
                                    TG_DATA_DELTA,
                                    RelPower);

                            float slopeThetaRelPower = Statistics.Utilities.Slope(SlopeThetaRelPower);
                            float slopeBetaLowRelPower = Statistics.Utilities.Slope(SlopeBetaLowRelPower);
                            float slopeAlphaHighRelPower = Statistics.Utilities.Slope(SlopeAlphaHighRelPower);
                            float slopePower = Statistics.Utilities.Slope(SlopePower);

                            await Utilities.LogSlopesAsync(log, _datetime, slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                            #region chartValues
                            handler.HandleSlopes(
                                new SetMyBrainSlopes(
                                    _datetime,
                                    slopeThetaRelPower,
                                    slopeBetaLowRelPower,
                                    slopeAlphaHighRelPower,
                                    slopePower
                                )
                            );
                            #endregion

                            bool flow = Utilities.InTheFlow(slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                            handler.HandleFlow(flow);

                            Thread.Sleep(800);
                        }
                        #endregion
                    }
                    #endregion

                    #region cancellation
                    if (token.IsCancellationRequested)
                    {
                        //Console.WriteLine("Task cancelled");
                        token.ThrowIfCancellationRequested();
                    }
                    #endregion

                    #region visibility limit
                    //SetAxisXLimits(_datetime);
                    //if (ChartValuesAlpha1.Count > visibility_limit)
                    //    ChartValuesAlpha1.RemoveAt(0);
                    //if (ChartValuesAlpha2.Count > visibility_limit)
                    //    ChartValuesAlpha2.RemoveAt(0);
                    //if (ChartValuesBeta1.Count > visibility_limit)
                    //    ChartValuesBeta1.RemoveAt(0);
                    //if (ChartValuesBeta2.Count > visibility_limit)
                    //    ChartValuesBeta2.RemoveAt(0);
                    //if (ChartValuesGamma1.Count > visibility_limit)
                    //    ChartValuesGamma1.RemoveAt(0);
                    //if (ChartValuesGamma2.Count > visibility_limit)
                    //    ChartValuesGamma2.RemoveAt(0);
                    //if (ChartValuesDelta.Count > visibility_limit)
                    //    ChartValuesDelta.RemoveAt(0);
                    //if (ChartValuesTheta.Count > visibility_limit)
                    //    ChartValuesTheta.RemoveAt(0);
                    //if (ChartValuesAttention.Count > visibility_limit)
                    //    ChartValuesAttention.RemoveAt(0);
                    //if (ChartValuesCreativity.Count > visibility_limit)
                    //    ChartValuesCreativity.RemoveAt(0);
                    //if (ChartValuesImmersion.Count > visibility_limit)
                    //    ChartValuesImmersion.RemoveAt(0);
                    //if (ChartValuesArousal.Count > visibility_limit)
                    //    ChartValuesArousal.RemoveAt(0);
                    //if (ChartValuesEngagement.Count > visibility_limit)
                    //    ChartValuesEngagement.RemoveAt(0);
                    #endregion
                }
            }
            #endregion
            #region catch
            catch (OperationCanceledException)
            {
                Trace.TraceInformation("OperationCancelled");
            }
            #endregion
        }
    }
}
