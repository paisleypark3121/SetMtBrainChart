using AppSettings;
using SetMyBrainWPFChart.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Neurosky
{
    public class Utilities
    {
        public static object get(IAppSettings appSettings, string[] args, string key)
        {
            #region preconditions
            if (string.IsNullOrEmpty(key))
                return null;
            #endregion

            #region defauls values
            if ((args == null) || (args.Length < 2))
            {
                if (key == "-c")
                    return appSettings["COM"];
                else if (key == "-t")
                    return appSettings["TAG"];
                else if (key == "-d")
                {
                    return bool.Parse(appSettings["DEBUG"]);
                }
                else if (key == "-e")
                    return appSettings["ENDPOINT"];
                else
                    return null;
            }
            #endregion
            #region ARGS
            else
            {
                if (args.Contains(key))
                {
                    if (key == "-d")
                        return bool.Parse(args[Array.IndexOf(args, key) + 1]);
                    else
                        return args[Array.IndexOf(args, key) + 1];
                }
                else
                    return null;
            }
            #endregion
        }

        public static Dictionary<string, object> setDictionary(
            string tag,
            string ticks,
            DateTime datetime,
            float TG_DATA_RAW,
            float TG_DATA_ALPHA1,
            float TG_DATA_ALPHA2,
            float TG_DATA_BETA1,
            float TG_DATA_BETA2,
            float TG_DATA_DELTA,
            float TG_DATA_GAMMA1,
            float TG_DATA_GAMMA2,
            float TG_DATA_THETA,
            float TG_DATA_MEDITATION,
            float TG_DATA_ATTENTION,
            float TG_DATA_POOR_SIGNAL,
            float grossIndex_attention,
            float grossIndex_creativity,
            float grossIndex_engagement,
            float grossIndex_arousal,
            float grossIndex_immersion,
            float grossIndex_attention_normalized,
            float grossIndex_creativity_normalized,
            float grossIndex_engagement_normalized,
            float grossIndex_arousal_normalized,
            float grossIndex_immersion_normalized
            )
        {
            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                { "tag",tag },
                { "ticks",ticks },
                { "datetime", datetime },
                { "TG_DATA_RAW",TG_DATA_RAW},
                { "TG_DATA_ALPHA1",TG_DATA_ALPHA1},
                { "TG_DATA_ALPHA2",TG_DATA_ALPHA2},
                { "TG_DATA_BETA1",TG_DATA_BETA1},
                { "TG_DATA_BETA2",TG_DATA_BETA2},
                { "TG_DATA_DELTA",TG_DATA_DELTA},
                { "TG_DATA_GAMMA1",TG_DATA_GAMMA1},
                { "TG_DATA_GAMMA2",TG_DATA_GAMMA2},
                { "TG_DATA_THETA",TG_DATA_THETA},
                { "TG_DATA_MEDITATION",TG_DATA_MEDITATION},
                { "TG_DATA_ATTENTION",TG_DATA_ATTENTION},
                { "TG_DATA_POOR_SIGNAL",TG_DATA_POOR_SIGNAL},
                { "grossIndex_attention",grossIndex_attention},
                { "grossIndex_creativity",grossIndex_creativity},
                { "grossIndex_engagement",grossIndex_engagement},
                { "grossIndex_arousal",grossIndex_arousal},
                { "grossIndex_immersion",grossIndex_immersion},
                { "grossIndex_attention_normalized",grossIndex_attention_normalized},
                { "grossIndex_creativity_normalized",grossIndex_creativity_normalized},
                { "grossIndex_engagement_normalized",grossIndex_engagement_normalized},
                { "grossIndex_arousal_normalized",grossIndex_arousal_normalized},
                { "grossIndex_immersion_normalized",grossIndex_immersion_normalized},
            };

            return dict;
        }

        public static bool AreEqual(Dictionary<string, object> previous, Dictionary<string, object> request)
        {
            if (previous == null)
                return false;

            DateTime previous_date = (DateTime)previous["datetime"];
            DateTime current_date = (DateTime)request["datetime"];

            if ((previous_date.Year == current_date.Year) &&
                (previous_date.Month == current_date.Month) &&
                (previous_date.Day == current_date.Day) &&
                (previous_date.Hour == current_date.Hour) &&
                (previous_date.Minute == current_date.Minute) &&
                (previous_date.Second == current_date.Second))
                return true;

            return ((previous["TG_DATA_RAW"] == request["TG_DATA_RAW"]) &&
                (previous["TG_DATA_ALPHA1"] == request["TG_DATA_ALPHA1"]) &&
                (previous["TG_DATA_ALPHA2"] == request["TG_DATA_ALPHA2"]) &&
                (previous["TG_DATA_BETA1"] == request["TG_DATA_BETA1"]) &&
                (previous["TG_DATA_BETA2"] == request["TG_DATA_BETA2"]) &&
                (previous["TG_DATA_DELTA"] == request["TG_DATA_DELTA"]) &&
                (previous["TG_DATA_GAMMA1"] == request["TG_DATA_GAMMA1"]) &&
                (previous["TG_DATA_GAMMA2"] == request["TG_DATA_GAMMA2"]) &&
                (previous["TG_DATA_THETA"] == request["TG_DATA_THETA"]));
        }

        public static bool AreEqual(
            Dictionary<string, object> previous,
            DateTime current_date,
            float TG_DATA_RAW,
            float TG_DATA_ALPHA1,
            float TG_DATA_ALPHA2,
            float TG_DATA_BETA1,
            float TG_DATA_BETA2,
            float TG_DATA_DELTA,
            float TG_DATA_GAMMA1,
            float TG_DATA_GAMMA2,
            float TG_DATA_THETA)
        {
            if (previous == null)
                return false;

            DateTime previous_date = (DateTime)previous["datetime"];

            if ((previous_date.Year == current_date.Year) &&
                (previous_date.Month == current_date.Month) &&
                (previous_date.Day == current_date.Day) &&
                (previous_date.Hour == current_date.Hour) &&
                (previous_date.Minute == current_date.Minute) &&
                (previous_date.Second == current_date.Second))
                return true;

            return (previous["TG_DATA_RAW"].ToString() == TG_DATA_RAW.ToString()) &&
                (previous["TG_DATA_ALPHA1"].ToString() == TG_DATA_ALPHA1.ToString()) &&
                (previous["TG_DATA_ALPHA2"].ToString() == TG_DATA_ALPHA2.ToString()) &&
                (previous["TG_DATA_BETA1"].ToString() == TG_DATA_BETA1.ToString()) &&
                (previous["TG_DATA_BETA2"].ToString() == TG_DATA_BETA2.ToString()) &&
                (previous["TG_DATA_DELTA"].ToString() == TG_DATA_DELTA.ToString()) &&
                (previous["TG_DATA_GAMMA1"].ToString() == TG_DATA_GAMMA1.ToString()) &&
                (previous["TG_DATA_GAMMA2"].ToString() == TG_DATA_GAMMA2.ToString()) &&
                (previous["TG_DATA_THETA"].ToString() == TG_DATA_THETA.ToString());
        }

        public static float RelPower(
            float TG_DATA_ALPHA1,
            float TG_DATA_ALPHA2,
            float TG_DATA_BETA1,
            float TG_DATA_BETA2,
            float TG_DATA_DELTA,
            float TG_DATA_GAMMA1,
            float TG_DATA_GAMMA2,
            float TG_DATA_THETA)
        {
            return TG_DATA_ALPHA1 + TG_DATA_ALPHA2 + TG_DATA_BETA1 + TG_DATA_BETA2 + TG_DATA_DELTA + TG_DATA_GAMMA1 + TG_DATA_GAMMA2 + TG_DATA_THETA;
        }

        public static float Engagement(float TG_DATA_BETA1, float TG_DATA_BETA2, float TG_DATA_ALPHA1, float TG_DATA_ALPHA2, float TG_DATA_THETA)
        {
            return (TG_DATA_BETA1 + TG_DATA_BETA2) / (TG_DATA_ALPHA1 + TG_DATA_ALPHA2 + TG_DATA_THETA);
        }

        public static float Arousal(float TG_DATA_BETA1, float TG_DATA_BETA2, float TG_DATA_ALPHA1, float TG_DATA_ALPHA2)
        {
            return (TG_DATA_BETA1 + TG_DATA_BETA2) / (TG_DATA_ALPHA1 + TG_DATA_ALPHA2);
        }

        public static float Immersion(float TG_DATA_THETA, float TG_DATA_ALPHA1, float TG_DATA_ALPHA2)
        {
            return TG_DATA_THETA / (TG_DATA_ALPHA1 + TG_DATA_ALPHA2);
        }

        public static float Attention(float TG_DATA_THETA, float TG_DATA_ALPHA1, float TG_DATA_ALPHA2)
        {
            float thetaRelPower = TG_DATA_THETA;
            float alphaRelPower = TG_DATA_ALPHA1 + TG_DATA_ALPHA2;

            return (thetaRelPower - alphaRelPower) / (thetaRelPower + alphaRelPower);
        }

        public static float Creativity(float TG_DATA_ALPHA2, float RelPower)
        {
            return (TG_DATA_ALPHA2 / RelPower);
        }

        public static float grossIndex(
            float avgSlider,
            float varSlider,
            int slider_limit,
            float avgBaseline,
            float varBaseline,
            int baseline_limit)
        {
            float Num = avgSlider - avgBaseline;
            float Den = (float)Math.Sqrt(varBaseline / baseline_limit + varSlider / slider_limit);
            return Num / Den;
        }

        public static float grossIndexNormalized(
            float grossIndex,
            float min_grossIndex,
            float max_grossIndex)
        {
            if (grossIndex < min_grossIndex)
                return 5;
            else if (grossIndex > max_grossIndex)
                return 90;
            else
                return (100 * (grossIndex - min_grossIndex)) / (max_grossIndex - min_grossIndex);

            //if (grossIndex < (min_grossIndex * 0.9))
            //    return 5;
            //else if (grossIndex > (max_grossIndex * 0.9))
            //    return 90;
            //else
            //    return (100 * (grossIndex-min_grossIndex)) / (max_grossIndex - min_grossIndex);
        }

        public static void SetMinMax(float value, ref float min, ref float max)
        {
            if (value < min)
                min = value;
            if (value > max)
                max = value;
        }

        public static void SetMinMax(
            float[] AVG_180,
            float[] VAR_180,
            float baseline_avg,
            float baseline_var,
            int baseline_slider_limit,
            int slider_limit,
            ref float min,
            ref float max)
        {
            min = 999999;
            max = -999999;

            for (int i = slider_limit - 1; i < baseline_slider_limit; i++)
            {
                float grossIndex = Utilities.grossIndex(AVG_180[i], VAR_180[i], slider_limit, baseline_avg, baseline_var, baseline_slider_limit);
                if (grossIndex < min)
                    min = grossIndex;
                //if (grossIndex > max)
                //    max = grossIndex;
            }

            max = min;

            //if (min >= 0)
            //    min = -Math.Abs(max) * 1.3F;
            //else
            //    //min = (-1 - max) * 1.3F;
            //    min = (-1 - Math.Abs(max)) * 1.3F;

            //max = Math.Abs(max) * 1.3F;
            //if (max <= 0)
            //    max = Math.Abs(min) * 1.3F;
            //else
            //    max = (1 + max) * 1.3F;

            if (min >= 0)
                min = -Math.Abs(max);
            else
                min = (-1 - Math.Abs(max));

            max = Math.Abs(max);
            if (max <= 0)
                max = Math.Abs(min);
            else
                max = (1 + max);
        }

        public static void SetMinMaxGross(
            ref float minGross,
            ref float maxGross,
            float grossIndex)
        {
            //if (minGross)

            //Min_grossIndex_120sec = MIN(grossIndex, 120 sc)
            //Max_grossIndex_120sec = MIN(grossIndex, 120 sc)

            //Min_grossIndex =
            //If Min_grossIndex_120sec >= 0  -ABS(Max_grossIndex_120sec) * 1.3
            //If Min_grossIndex_120sec<0  (-1 - Max_grossIndex_120sec) * 1.3

            //Max_grossIndex =
            //ABS(Max_grossIndex_120sec) * 1.3
            //If Max_grossIndex_120sec <= 0  ABS(Min_grossIndex_120sec) * 1.3
            //If Max_grossIndex_120sec > 0  (1 + Max_grossIndex_120sec) * 1.3


        }

        public static void SetMinMaxGross(
            ref float minGross,
            ref float maxGross,
            float minAvg,
            float maxAvg,
            float var,
            float minVar,
            int baseline_slider_limit,
            int slider_limit)
        {
            float DEN = (float)Math.Sqrt(var / baseline_slider_limit + minVar / slider_limit);
            minGross = -Math.Abs(minAvg / DEN);
            maxGross = -minGross;

            //if (minGross)

            //Min_grossIndex_120sec = MIN(grossIndex, 120 sc)
            //Max_grossIndex_120sec = MIN(grossIndex, 120 sc)

            //Min_grossIndex =
            //If Min_grossIndex_120sec >= 0  -ABS(Max_grossIndex_120sec) * 1.3
            //If Min_grossIndex_120sec<0  (-1 - Max_grossIndex_120sec) * 1.3

            //Max_grossIndex =
            //ABS(Max_grossIndex_120sec) * 1.3
            //If Max_grossIndex_120sec <= 0  ABS(Min_grossIndex_120sec) * 1.3
            //If Max_grossIndex_120sec > 0  (1 + Max_grossIndex_120sec) * 1.3


        }

        public static void SetSlopes(
            ref float[] SlopeThetaRelPower,
            ref float[] SlopeBetaLowRelPower, 
            ref float[] SlopeAlphaHighRelPower,
            ref float[] SlopePower,
            float TG_DATA_ALPHA2,
            float TG_DATA_BETA1,
            float TG_DATA_DELTA,
            float RelPower)
        {
            SlopeThetaRelPower = Utilities.PushElement(SlopeThetaRelPower, TG_DATA_DELTA / RelPower);
            SlopeBetaLowRelPower = Utilities.PushElement(SlopeBetaLowRelPower, TG_DATA_BETA1 / RelPower);
            SlopeAlphaHighRelPower = Utilities.PushElement(SlopeAlphaHighRelPower, TG_DATA_ALPHA2 / RelPower);
            SlopePower = Utilities.PushElement(SlopePower, (TG_DATA_DELTA / RelPower + TG_DATA_ALPHA2 / RelPower + TG_DATA_BETA1 / RelPower) / RelPower);
        }

        public static float[] PushElement(float[] y,float _y)
        {
            var yList = y.ToList();
            yList.RemoveAt(0);
            yList.Add(_y);
            return yList.ToArray();
        }

        public static bool InTheFlow(
            float SlopeThetaRelPower,
            float SlopeBetaLowRelPower,
            float SlopeAlphaHighRelPower,
            float SlopePower)
        {
            float soglia1 = 0.18F;
            float soglia2 = 0.1F;

            if (
                ((SlopeThetaRelPower >= soglia1) && (SlopeAlphaHighRelPower >= soglia1) && (SlopeBetaLowRelPower >= soglia1))
                &&
                ((SlopeThetaRelPower >= 0) && (SlopeAlphaHighRelPower >= soglia1) && (SlopeBetaLowRelPower >= soglia1))
                &&
                ((SlopeThetaRelPower >= soglia1) && (SlopeAlphaHighRelPower >= 0) && (SlopeBetaLowRelPower >= soglia1))
                &&
                ((SlopeThetaRelPower >= soglia1) && (SlopeAlphaHighRelPower >= soglia1) && (SlopeBetaLowRelPower >= 0))
                )
                return true;

            if (
                ((SlopeThetaRelPower >= soglia1) && (SlopeAlphaHighRelPower >= 0) && (SlopeBetaLowRelPower >= 0) && (SlopePower > soglia2))
                &&
                ((SlopeThetaRelPower >= 0) && (SlopeAlphaHighRelPower >= soglia1) && (SlopeBetaLowRelPower >= 0) && (SlopePower > soglia2))
                &&
                ((SlopeThetaRelPower >= 0) && (SlopeAlphaHighRelPower >= 0) && (SlopeBetaLowRelPower >= soglia1) && (SlopePower > soglia2))
                )
                return true;

            return false;
        }

        public static Task LogSlopesAsync(
            Dictionary<string, ILog> log,
            DateTime timestamp,
            float slopeThetaRelPower,
            float slopeBetaLowRelPower,
            float slopeAlphaHighRelPower,
            float slopePower)
        {
            if (log == null)
                return null;

            string message = timestamp.Ticks+","+
                slopeThetaRelPower.ToString().Replace(',', '.') + "," +
                slopeBetaLowRelPower.ToString().Replace(',', '.') + "," +
                slopeAlphaHighRelPower.ToString().Replace(',', '.') + "," +
                slopePower.ToString().Replace(',', '.');
            return log["Slopes"].TraceAsync(message);
        }

        public static Task LogIndexesAsync(
            Dictionary<string, ILog> log,
            DateTime timestamp,
            float attention,
            float creativity,
            float engagement,
            float arousal,
            float immersion)
        {
            if (log == null)
                return null;

            string message = timestamp.Ticks + "," +
                attention.ToString().Replace(',', '.') + "," +
                creativity.ToString().Replace(',', '.') + "," +
                engagement.ToString().Replace(',', '.') + "," +
                arousal.ToString().Replace(',', '.') + "," +
                immersion.ToString().Replace(',', '.');
            return log["Indexes"].TraceAsync(message);
        }

        public static Task LogFrequenciesAsync(
            Dictionary<string, ILog> log,
            DateTime timestamp,
            float TG_DATA_ALPHA1,
            float TG_DATA_ALPHA2,
            float TG_DATA_BETA1,
            float TG_DATA_BETA2,
            float TG_DATA_GAMMA1,
            float TG_DATA_GAMMA2,
            float TG_DATA_DELTA,
            float TG_DATA_THETA)
        {
            if (log == null)
                return null;

            string message = timestamp.Ticks + "," + TG_DATA_ALPHA1 + "," + TG_DATA_ALPHA2 + "," + TG_DATA_BETA1 + "," + TG_DATA_BETA2 + "," + TG_DATA_GAMMA1 +"," + TG_DATA_GAMMA2 + "," + TG_DATA_DELTA + "," + TG_DATA_THETA;
            return log["Frequencies"].TraceAsync(message);
        }
    }
}
