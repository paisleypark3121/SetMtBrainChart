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
        private float _trend;
        private static Random random = new Random();

        public BasicChartUserControl()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<DateChartModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<DateChartModel>(mapper);

            //the values property will store our values array
            //ChartValues = new ChartValues<DateChartModel>();
            ChartValuesAlpha1 = new ChartValues<DateChartModel>();
            ChartValuesAlpha2 = new ChartValues<DateChartModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

            IsReading = false;

            DataContext = this;
        }


        //public ChartValues<DateChartModel> ChartValues { get; set; }
        public ChartValues<DateChartModel> ChartValuesAlpha1 { get; set; }
        public ChartValues<DateChartModel> ChartValuesAlpha2 { get; set; }

        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

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
            //var r = new Random();

            while (IsReading)
            {
                Thread.Sleep(150);
                var now = DateTime.Now;

                //_trend += r.Next(-8, 10);
                float alpha1 = random.Next(1, 20);
                float alpha2 = random.Next(1, 20);

                //ChartValues.Add(new DateChartModel
                //{
                //    DateTime = now,
                //    Value = _trend
                //});

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

                //lets only use the last 150 values
                if (ChartValuesAlpha1.Count > 150)
                    ChartValuesAlpha1.RemoveAt(0);
                if (ChartValuesAlpha2.Count > 150)
                    ChartValuesAlpha2.RemoveAt(0);
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
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