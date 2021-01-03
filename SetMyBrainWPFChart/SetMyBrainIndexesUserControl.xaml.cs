using LiveCharts;
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
    /// Logica di interazione per SetMyBrainIndexesUserControl.xaml
    /// </summary>
    public partial class SetMyBrainIndexesUserControl : UserControl, INotifyPropertyChanged
    {
        #region limits
        public int visibility_limit = 150;
        #endregion

        #region visibility
        private bool _attentionVisibility;
        private bool _creativitySeriesVisibility;
        private bool _immersionSeriesVisibility;
        private bool _arousalSeriesVisibility;
        private bool _engagementSeriesVisibility;

        public bool AttentionSeriesVisibility
        {
            get { return _attentionVisibility; }
            set
            {
                _attentionVisibility = value;
                OnPropertyChanged("AttentionSeriesVisibility");
            }
        }
        public bool CreativitySeriesVisibility
        {
            get { return _creativitySeriesVisibility; }
            set
            {
                _creativitySeriesVisibility = value;
                OnPropertyChanged("CreativitySeriesVisibility");
            }
        }
        public bool ImmersionSeriesVisibility
        {
            get { return _immersionSeriesVisibility; }
            set
            {
                _immersionSeriesVisibility = value;
                OnPropertyChanged("ImmersionSeriesVisibility");
            }
        }
        public bool ArousalSeriesVisibility
        {
            get { return _arousalSeriesVisibility; }
            set
            {
                _arousalSeriesVisibility = value;
                OnPropertyChanged("ArousalSeriesVisibility");
            }
        }
        public bool EngagementSeriesVisibility
        {
            get { return _engagementSeriesVisibility; }
            set
            {
                _engagementSeriesVisibility = value;
                OnPropertyChanged("EngagementSeriesVisibility");
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
        private SetMyBrainIndexes _setMyBrainIndexes;
        public SetMyBrainIndexes SetMyBrainIndexes
        {
            get { return _setMyBrainIndexes; }
            set { _setMyBrainIndexes = value; OnPropertyChanged("SetMyBrainIndexes"); }
        }
        #endregion

        #region chartValues
        public ChartValues<DateChartModel> ChartValuesAttention { get; set; }
        public ChartValues<DateChartModel> ChartValuesCreativity { get; set; }
        public ChartValues<DateChartModel> ChartValuesImmersion { get; set; }
        public ChartValues<DateChartModel> ChartValuesArousal { get; set; }
        public ChartValues<DateChartModel> ChartValuesEngagement { get; set; }
        #endregion

        public SetMyBrainIndexesUserControl()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<DateChartModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value);

            Charting.For<DateChartModel>(mapper);

            ChartValuesAttention = new ChartValues<DateChartModel>();
            ChartValuesCreativity = new ChartValues<DateChartModel>();
            ChartValuesImmersion = new ChartValues<DateChartModel>();
            ChartValuesArousal = new ChartValues<DateChartModel>();
            ChartValuesEngagement = new ChartValues<DateChartModel>();

            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");

            AxisXStep = TimeSpan.FromSeconds(1).Ticks;
            AxisXUnit = TimeSpan.TicksPerSecond;

            SetAxisXLimits(DateTime.Now);
            SetAxisYLimits(0, 100);

            _attentionVisibility = true;
            _creativitySeriesVisibility = true;
            _immersionSeriesVisibility = true;
            _arousalSeriesVisibility = true;
            _engagementSeriesVisibility = true;

            this.PropertyChanged += FrequenciesView;

            DataContext = this;
        }

        #region event handler
        private void SetIndexes()
        {
            ChartValuesAttention.Add(new DateChartModel
            {
                DateTime = SetMyBrainIndexes.timestamp,
                Value = SetMyBrainIndexes.attention
            });
            ChartValuesCreativity.Add(new DateChartModel
            {
                DateTime = SetMyBrainIndexes.timestamp,
                Value = SetMyBrainIndexes.creativity
            });
            ChartValuesImmersion.Add(new DateChartModel
            {
                DateTime = SetMyBrainIndexes.timestamp,
                Value = SetMyBrainIndexes.immersion
            });
            ChartValuesArousal.Add(new DateChartModel
            {
                DateTime = SetMyBrainIndexes.timestamp,
                Value = SetMyBrainIndexes.arousal
            });
            ChartValuesEngagement.Add(new DateChartModel
            {
                DateTime = SetMyBrainIndexes.timestamp,
                Value = SetMyBrainIndexes.engagement
            });
        }
        private void SetVisibility()
        {
            SetAxisXLimits(SetMyBrainIndexes.timestamp);

            if (ChartValuesAttention.Count > visibility_limit)
                ChartValuesAttention.RemoveAt(0);
            if (ChartValuesCreativity.Count > visibility_limit)
                ChartValuesCreativity.RemoveAt(0);
            if (ChartValuesImmersion.Count > visibility_limit)
                ChartValuesImmersion.RemoveAt(0);
            if (ChartValuesArousal.Count > visibility_limit)
                ChartValuesArousal.RemoveAt(0);
            if (ChartValuesEngagement.Count > visibility_limit)
                ChartValuesEngagement.RemoveAt(0);
        }
        private void FrequenciesView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SetMyBrainIndexes")
            {
                SetIndexes();
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
