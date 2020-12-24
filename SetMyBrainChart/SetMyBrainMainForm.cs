using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetMyBrainChart
{
    public partial class SetMyBrainMainForm : Form
    {
        public SeriesCollection series { get; set; }
        public Func<double, string> Formatter { get; set; }

        public SetMyBrainMainForm()
        {
            InitializeComponent();
            Publisher publisher = new Publisher("Neurosky Publisher", 1000);
            publisher.OnPublish += OnNotificationReceived;
            Task task = Task.Factory.StartNew(() => publisher.Publish());
        }

        private void UpdateChart(object sender, EventArgs e)
        {   
            cartesianChart1.Update();
        }

        protected virtual void OnNotificationReceived(Publisher p, NotificationEvent e)
        {
            Console.WriteLine("Received from "+p.PublisherName+" - "+e.NotificationMessage.timestamp);
            //series[0].Values.Add(new ObservablePoint(e.NotificationMessage.timestamp.Second, e.NotificationMessage.alpha1));
            //series[1].Values.Add(new ObservablePoint(e.NotificationMessage.timestamp.Second, e.NotificationMessage.alpha2));
            series[0].Values.Add(new DateChartModel() {DateTime = e.NotificationMessage.timestamp, Value=e.NotificationMessage.alpha1 });
            series[1].Values.Add(new DateChartModel() { DateTime = e.NotificationMessage.timestamp, Value = e.NotificationMessage.alpha2 });
            //cartesianChart1.Update(); // DOESN'T WORK BECAUSE OF DIFFERENT THREAD -> needed internal event handler
            this.Invoke(new EventHandler(UpdateChart));
        }

        private void SetMyBrainMainForm_Load(object sender, EventArgs e)
        {
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis()
            {
                Title = "DateTime"
            });
            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis()
            {
                Title = "NeuroskyData"
            });
            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Right;

            //series = new SeriesCollection()
            //{
            //    new LineSeries
            //    {
            //        Title="alpha1",
            //        Values=new ChartValues<ObservablePoint>
            //        {
            //        //    new ObservablePoint(0,1),
            //        //    new ObservablePoint(1,2),
            //        //    new ObservablePoint(2,4),
            //        //    new ObservablePoint(3,8)
            //        }
            //    },
            //    new LineSeries
            //    {
            //        Title="alpha2",
            //        Values=new ChartValues<ObservablePoint>
            //        {
            //        //    new ObservablePoint(0,3),
            //        //    new ObservablePoint(1,2),
            //        //    new ObservablePoint(2,1),
            //        //    new ObservablePoint(3,9)
            //        }
            //    },
            //};

            var dayConfig = Mappers.Xy<DateChartModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromSeconds(1).Ticks)
                .Y(dayModel => dayModel.Value);

            //Formatter = value => new DateTime((long)(value * TimeSpan.FromSeconds(1).Ticks)).ToString("t");
            Formatter = (value) =>
            {
                Console.WriteLine(value);
                if (value < 0)
                    value = 0;
                return new DateTime((long)(value * TimeSpan.FromSeconds(1).Ticks)).ToString("T");
            };

            series = new SeriesCollection(dayConfig)
            {   
                new LineSeries
                {
                    Title="alpha1",
                    Values=new ChartValues<DateChartModel>
                    {
                        new DateChartModel()
                        {
                            DateTime=DateTime.Now,
                            Value=0,
                        }
                    }
                },
                new LineSeries
                {
                    Title="alpha2",
                    Values=new ChartValues<DateChartModel>
                    {
                        new DateChartModel()
                        {
                            DateTime=DateTime.Now,
                            Value=0,
                        }
                    }
                },
            };

            cartesianChart1.Series = series;
            

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = Formatter
            });
        }
    }
}
