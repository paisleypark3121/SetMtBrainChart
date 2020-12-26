using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logica di interazione per BasicChartUserControl.xaml
    /// </summary>
    public partial class BasicChartUserControl : UserControl, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;
        private static Random random = new Random();
        public ChartValues<DateChartModel> ChartValuesAlpha1 { get; set; }
        public ChartValues<DateChartModel> ChartValuesAlpha2 { get; set; }

        private bool _alpha1SeriesVisibility;
        private bool _alpha2SeriesVisibility;
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }


        public BasicChartUserControl()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<DateChartModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value);

            Charting.For<DateChartModel>(mapper);

            ChartValuesAlpha1 = new ChartValues<DateChartModel>();
            ChartValuesAlpha2 = new ChartValues<DateChartModel>();
            
            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");

            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            IsReading = false;
            _alpha1SeriesVisibility = true;
            _alpha2SeriesVisibility = true;

            DataContext = this;
        }

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

        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        public bool IsReading { get; set; }

        private void Read()
        {
            while (IsReading)
            {
                Thread.Sleep(150);
                var now = DateTime.Now;

                float alpha1 = random.Next(1, 20);
                float alpha2 = random.Next(1, 20);

                ChartValuesAlpha1.Add(new DateChartModel
                {
                    DateTime = now,
                    Value = alpha1
                });

                ChartValuesAlpha2.Add(new DateChartModel
                {
                    DateTime = now,
                    Value = alpha2
                });

                SetAxisLimits(now);
                if (ChartValuesAlpha1.Count > 150)
                    ChartValuesAlpha1.RemoveAt(0);
                if (ChartValuesAlpha2.Count > 150)
                    ChartValuesAlpha2.RemoveAt(0);
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks;
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks;
        }

        private void InjectStopOnClick(object sender, RoutedEventArgs e)
        {
            IsReading = !IsReading;
            if (IsReading) 
                Task.Factory.StartNew(Read);
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        //var dayConfig = Mappers.Xy<DateChartModel>()
        //    .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromSeconds(1).Ticks)
        //    .Y(dayModel => dayModel.Value);

        ////Notice you can also configure this type globally, so you don't need to configure every
        ////SeriesCollection instance using the type.
        ////more info at http://lvcharts.net/App/Index#/examples/v1/wpf/Types%20and%20Configuration

        //Series = new SeriesCollection(dayConfig)
        //{
        //    new LineSeries
        //    {
        //        Values = new ChartValues<DateChartModel>
        //        {
        //            new DateChartModel
        //            {
        //                DateTime = System.DateTime.Now,
        //                Value = 5
        //            },
        //            new DateChartModel
        //            {
        //                DateTime = System.DateTime.Now.AddSeconds(2),
        //                Value = 9
        //            },
        //            new DateChartModel
        //            {
        //                DateTime = System.DateTime.Now.AddSeconds(5),
        //                Value = 10
        //            },
        //            new DateChartModel
        //            {
        //                DateTime = System.DateTime.Now.AddSeconds(10),
        //                Value = 7
        //            },
        //            new DateChartModel
        //            {
        //                DateTime = System.DateTime.Now.AddSeconds(13),
        //                Value = 5
        //            }
        //        },
        //        Fill = Brushes.Transparent
        //    },
        //};

        //Formatter = value => new System.DateTime((long)(value * TimeSpan.FromSeconds(1).Ticks)).ToString("T");

        //DataContext = this;
        //}

        //public Func<double, string> Formatter { get; set; }
        //public SeriesCollection Series { get; set; }
    }
}