using AppSettings;
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
    public class SmartCollector : ICollector,IDisposable
    {
        #region variables
        private IAppSettings appSettings = null;
        //private CancellationToken token;
        private IConnector connector;
        private IHandler handler;
        private Dictionary<string,ILog> log;

        private string[] args = null;
        #endregion

        public SmartCollector(
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
            int window_lenght = 10;

            #region indexes
            float[] AttentionCollection = new float[window_lenght];
            float[] CreativityCollection = new float[window_lenght];
            float[] EngagementCollection = new float[window_lenght];
            float[] ArousalCollection = new float[window_lenght];
            float[] ImmersionCollection = new float[window_lenght];

            for (int i = 0; i < window_lenght; i++)
            {
                AttentionCollection[i] = 0;
                CreativityCollection[i] = 0;
                EngagementCollection[i] = 0;
                ArousalCollection[i] = 0;
                ImmersionCollection[i] = 0;
            }
            #endregion

            #region slopes
            float[] ThetaRelPowerCollection = new float[window_lenght];
            float[] AlphaHighRelPowerCollection = new float[window_lenght];
            float[] BetaLowRelPowerCollection = new float[window_lenght];
            float[] PowerCollection = new float[window_lenght];

            for (int i=0;i< window_lenght; i++)
            {
                ThetaRelPowerCollection[i] = 0;
                AlphaHighRelPowerCollection[i] = 0;
                BetaLowRelPowerCollection[i] = 0;
                PowerCollection[i] = 0;
            }
            #endregion

            #region REPORT FILE
            string folder_name = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string tag = appSettings["tag"];
            string report_file = folder_name + "/" + tag + "_report_- " + DateTime.Now.Ticks + ".txt";
            #endregion

            #region STOPWATCHES
            Stopwatch stopwatch_poorSignal = new Stopwatch();
            Stopwatch stopwatch_flow = new Stopwatch();
            #endregion

            #endregion
            #region try
            try
            {
                #region STOPWATCHES
                stopwatch_poorSignal.Start();
                #endregion

                #region REPORT
                string message = "***START ANALYSIS***\r\n\r\n";
                message += "DateTime: " + DateTime.Now.ToString();
                await Utilities.report(report_file, message);
                #endregion

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

                    #region REPORT POOLSIGNAL
                    if (stopwatch_poorSignal.IsRunning)
                    {
                        message = "DateTime: " + DateTime.Now.ToString() + "\r\n";
                        message += "Signal: " + TG_DATA_POOR_SIGNAL;
                        await Utilities.report(report_file, message);

                        if (stopwatch_poorSignal.ElapsedMilliseconds > 5000)
                            stopwatch_poorSignal.Stop();
                    }
                    #endregion

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
                        float RelPower = Utilities.RelPower(TG_DATA_ALPHA1, TG_DATA_ALPHA2, TG_DATA_BETA1, TG_DATA_BETA2, TG_DATA_DELTA, TG_DATA_GAMMA1, TG_DATA_GAMMA2, TG_DATA_THETA);

                        #region chartValues frequencies
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

                        #region log frequencies
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
                        #endregion

                        #region index calculation parameters
                        Utilities.SetIndexes(
                                ref AttentionCollection,
                                ref CreativityCollection,
                                ref EngagementCollection,
                                ref ArousalCollection,
                                ref ImmersionCollection,
                                TG_DATA_ALPHA1,
                                TG_DATA_ALPHA2,
                                TG_DATA_BETA1,
                                TG_DATA_BETA2,
                                TG_DATA_DELTA,
                                TG_DATA_GAMMA1,
                                TG_DATA_GAMMA2,
                                TG_DATA_THETA,
                                RelPower);

                        float attention = Statistics.Utilities.Median(AttentionCollection);
                        float creativity = Statistics.Utilities.Median(CreativityCollection);
                        float engagement = Statistics.Utilities.Median(EngagementCollection);
                        float arousal = Statistics.Utilities.Median(ArousalCollection);
                        float immersion = Statistics.Utilities.Median(ImmersionCollection);
                        #endregion

                        #region chartValues - indexes
                        handler.HandleIndexes(
                            new SetMyBrainIndexes(
                                _datetime,
                                attention,
                                creativity,
                                immersion,
                                arousal,
                                engagement
                            )
                        );
                        #endregion

                        #region log indexes
                        await Utilities.LogIndexesAsync(
                            log, _datetime,
                            attention,
                            creativity,
                            engagement,
                            arousal,
                            immersion);
                        #endregion

                        #region slopes calculation
                        Utilities.SetSlopes(
                                ref ThetaRelPowerCollection,
                                ref BetaLowRelPowerCollection,
                                ref AlphaHighRelPowerCollection,
                                ref PowerCollection,
                                TG_DATA_ALPHA2,
                                TG_DATA_BETA1,
                                TG_DATA_DELTA,
                                RelPower);

                        float slopeThetaRelPower = Statistics.Utilities.Slope(ThetaRelPowerCollection);
                        float slopeBetaLowRelPower = Statistics.Utilities.Slope(BetaLowRelPowerCollection);
                        float slopeAlphaHighRelPower = Statistics.Utilities.Slope(AlphaHighRelPowerCollection);
                        float slopePower = Statistics.Utilities.Slope(PowerCollection);
                        #endregion

                        #region chartValues - slopes
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

                        #region log slopes
                        await Utilities.LogSlopesAsync(
                            log, _datetime,
                            slopeThetaRelPower,
                            slopeBetaLowRelPower,
                            slopeAlphaHighRelPower,
                            slopePower);
                        #endregion

                        bool flow = Utilities.InTheFlow(slopeThetaRelPower, slopeBetaLowRelPower, slopeAlphaHighRelPower, slopePower);

                        #region REPORT
                        if (flow)
                        {
                            if (!stopwatch_flow.IsRunning)
                            {
                                message = "DateTime: " + DateTime.Now.ToString() + "- INTHEFLOW!";
                                await Utilities.report(report_file, message);
                                stopwatch_flow.Start();
                            }

                            if (stopwatch_flow.ElapsedMilliseconds % 1000 == 0)
                            {
                                message = "DateTime: " + DateTime.Now.ToString() + "- STILL INTHEFLOW!";
                                await Utilities.report(report_file, message);
                                stopwatch_flow.Start();
                            }

                        } else
                        {
                            if (stopwatch_flow.IsRunning)
                            {
                                message = "DateTime: " + DateTime.Now.ToString() + "- NO MORE INTHEFLOW!";
                                await Utilities.report(report_file, message);
                                stopwatch_flow.Stop();
                            }
                        }
                        #endregion

                        handler.HandleFlow(flow);

                        Thread.Sleep(800);
                    }
                    #endregion

                    #region cancellation
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
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
            #region finally
            finally
            {
                #region REPORT
                string message = "\r\n\r\n***END ANALYSIS***\r\n\r\n";
                message += "DateTime: " + DateTime.Now.ToString();
                await Utilities.report(report_file, message);
                #endregion
            }
            #endregion
        }
    }
}
