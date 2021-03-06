﻿using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Logica di interazione per NeuroskyFrequenciesUserControl.xaml
    /// </summary>
    public partial class NeuroskyFrequenciesUserControl : UserControl, INotifyPropertyChanged
    {
        #region limits
        public int visibility_limit = 150;
        #endregion

        #region visibility
        private bool _alpha1SeriesVisibility;
        private bool _alpha2SeriesVisibility;
        private bool _beta1SeriesVisibility;
        private bool _beta2SeriesVisibility;
        private bool _gamma1SeriesVisibility;
        private bool _gamma2SeriesVisibility;
        private bool _deltaSeriesVisibility;
        private bool _thetaSeriesVisibility;

        public bool Alpha1SeriesVisibility
        {
            get { return _alpha1SeriesVisibility; }
            set
            {
                _alpha1SeriesVisibility = value;
                OnPropertyChanged("Alpha1SeriesVisibility");
            }
        }
        public bool Alpha2SeriesVisibility
        {
            get { return _alpha2SeriesVisibility; }
            set
            {
                _alpha2SeriesVisibility = value;
                OnPropertyChanged("Alpha2SeriesVisibility");
            }
        }
        public bool Beta1SeriesVisibility
        {
            get { return _beta1SeriesVisibility; }
            set
            {
                _beta1SeriesVisibility = value;
                OnPropertyChanged("Beta1SeriesVisibility");
            }
        }
        public bool Beta2SeriesVisibility
        {
            get { return _beta2SeriesVisibility; }
            set
            {
                _beta2SeriesVisibility = value;
                OnPropertyChanged("Beta2SeriesVisibility");
            }
        }
        public bool Gamma1SeriesVisibility
        {
            get { return _gamma1SeriesVisibility; }
            set
            {
                _gamma1SeriesVisibility = value;
                OnPropertyChanged("Gamma1SeriesVisibility");
            }
        }
        public bool Gamma2SeriesVisibility
        {
            get { return _gamma2SeriesVisibility; }
            set
            {
                _gamma2SeriesVisibility = value;
                OnPropertyChanged("Gamma2SeriesVisibility");
            }
        }
        public bool DeltaSeriesVisibility
        {
            get { return _deltaSeriesVisibility; }
            set
            {
                _deltaSeriesVisibility = value;
                OnPropertyChanged("DeltaSeriesVisibility");
            }
        }
        public bool ThetaSeriesVisibility
        {
            get { return _thetaSeriesVisibility; }
            set
            {
                _thetaSeriesVisibility = value;
                OnPropertyChanged("ThetaSeriesVisibility");
            }
        }
        #endregion

        #region axis
        private double _axisXMax;
        private double _axisXMin;
        public double AxisXStep { get; set; }
        public double AxisXUnit { get; set; }
        public double AxisXMax
        {
            get { return _axisXMax; }
            set
            {
                _axisXMax = value;
                OnPropertyChanged("AxisXMax");
            }
        }
        public double AxisXMin
        {
            get { return _axisXMin; }
            set
            {
                _axisXMin = value;
                OnPropertyChanged("AxisXMin");
            }
        }
        public void SetAxisXLimits(DateTime now)
        {
            AxisXMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks;
            AxisXMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks;
        }
        public Func<double, string> DateTimeFormatter { get; set; }

        private double _axisYMax;
        private double _axisYMin;

        public double AxisYMax
        {
            get { return _axisYMax; }
            set
            {
                _axisYMax = value;
                OnPropertyChanged("AxisYMax");
            }
        }
        public double AxisYMin
        {
            get { return _axisYMin; }
            set
            {
                _axisYMin = value;
                OnPropertyChanged("AxisYMin");
            }
        }

        private void SetAxisYLimits(int min, int max)
        {
            AxisYMax = max;
            AxisYMin = min;
        }

        #endregion

        #region frequencies
        private NeuroskyFrequencies _neuroskyFrequencies;
        public NeuroskyFrequencies NeuroskyFrequencies 
        { 
            get { return _neuroskyFrequencies; } 
            set { _neuroskyFrequencies = value; OnPropertyChanged("NeuroskyFrequencies"); } 
        }
        #endregion

        #region chartValues
        public ChartValues<DateChartModel> ChartValuesAlpha1 { get; set; }
        public ChartValues<DateChartModel> ChartValuesAlpha2 { get; set; }
        public ChartValues<DateChartModel> ChartValuesBeta1 { get; set; }
        public ChartValues<DateChartModel> ChartValuesBeta2 { get; set; }
        public ChartValues<DateChartModel> ChartValuesGamma1 { get; set; }
        public ChartValues<DateChartModel> ChartValuesGamma2 { get; set; }
        public ChartValues<DateChartModel> ChartValuesDelta { get; set; }
        public ChartValues<DateChartModel> ChartValuesTheta { get; set; }
        #endregion

        public NeuroskyFrequenciesUserControl()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<DateChartModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value);

            Charting.For<DateChartModel>(mapper);

            ChartValuesAlpha1 = new ChartValues<DateChartModel>();
            ChartValuesAlpha2 = new ChartValues<DateChartModel>();
            ChartValuesBeta1 = new ChartValues<DateChartModel>();
            ChartValuesBeta2 = new ChartValues<DateChartModel>();
            ChartValuesGamma1 = new ChartValues<DateChartModel>();
            ChartValuesGamma2 = new ChartValues<DateChartModel>();
            ChartValuesDelta = new ChartValues<DateChartModel>();
            ChartValuesTheta = new ChartValues<DateChartModel>();

            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");

            AxisXStep = TimeSpan.FromSeconds(1).Ticks;
            AxisXUnit = TimeSpan.TicksPerSecond;

            SetAxisXLimits(DateTime.Now);
            SetAxisYLimits(0, 100);
            //SetAxisYLimits(-1000, 10000);

            _alpha1SeriesVisibility = true;
            _alpha2SeriesVisibility = true;
            _beta1SeriesVisibility = true;
            _beta2SeriesVisibility = true;
            _gamma1SeriesVisibility = true;
            _gamma2SeriesVisibility = true;
            _deltaSeriesVisibility = true;
            _thetaSeriesVisibility = true;

            this.PropertyChanged += FrequenciesView;

            DataContext = this;
        }

        #region event handler
        private void SetFrequencies()
        {
            ChartValuesAlpha1.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.alpha1
            });
            ChartValuesAlpha2.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.alpha2
            });
            ChartValuesBeta1.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.beta1
            });
            ChartValuesBeta2.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.beta2
            });
            ChartValuesGamma1.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.gamma1
            });
            ChartValuesGamma2.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.gamma2
            });
            ChartValuesDelta.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.delta
            });
            ChartValuesTheta.Add(new DateChartModel
            {
                DateTime = NeuroskyFrequencies.timestamp,
                Value = NeuroskyFrequencies.theta
            });
        }
        private void SetVisibility()
        {
            SetAxisXLimits(NeuroskyFrequencies.timestamp);

            if (ChartValuesAlpha1.Count > visibility_limit)
                ChartValuesAlpha1.RemoveAt(0);
            if (ChartValuesAlpha2.Count > visibility_limit)
                ChartValuesAlpha2.RemoveAt(0);
            if (ChartValuesBeta1.Count > visibility_limit)
                ChartValuesBeta1.RemoveAt(0);
            if (ChartValuesBeta2.Count > visibility_limit)
                ChartValuesBeta2.RemoveAt(0);
            if (ChartValuesGamma1.Count > visibility_limit)
                ChartValuesGamma1.RemoveAt(0);
            if (ChartValuesGamma2.Count > visibility_limit)
                ChartValuesGamma2.RemoveAt(0);
            if (ChartValuesDelta.Count > visibility_limit)
                ChartValuesDelta.RemoveAt(0);
            if (ChartValuesTheta.Count > visibility_limit)
                ChartValuesTheta.RemoveAt(0);
        }
        private void FrequenciesView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NeuroskyFrequencies") 
            {
                SetFrequencies();
                SetVisibility();
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
