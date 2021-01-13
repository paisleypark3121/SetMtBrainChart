using LiveCharts;
using LiveCharts.Wpf;
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
    /// Logica di interazione per InTheFlowUserControl.xaml
    /// </summary>
    public partial class InTheFlowUserControl : UserControl, INotifyPropertyChanged
    {
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        private SetMyBrainIndexes _setMyBrainIndexes;
        public SetMyBrainIndexes SetMyBrainIndexes
        {
            get { return _setMyBrainIndexes; }
            set { _setMyBrainIndexes = value; OnPropertyChanged("SetMyBrainIndexes"); }
        }

        public InTheFlowUserControl()
        {
            InitializeComponent();

            //SeriesCollection = new SeriesCollection();

            SeriesCollection = new SeriesCollection() {
                new RowSeries
                {   
                    Title="Top Mental",
                    Values = new ChartValues<double> {1,2,3,4,5}
                } 
            };

            //SeriesCollection = new SeriesCollection
            //{
            //    new RowSeries
            //    {
            //        Title = "2015",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 }
            //    }
            //};

            ////adding series will update and animate the chart automatically
            //SeriesCollection.Add(new RowSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            ////also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            //Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Labels = new[] { "Attention", "Creativity", "Immersion", "Arousal", "Engagement" };
            Formatter = value => value.ToString("N");

            this.PropertyChanged += IndexesView;

            DataContext = this;
        }

        private void IndexesView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SetMyBrainIndexes")
            {
                //SeriesCollection[0].Values[0]=SetMyBrainIndexes.attention;

                SeriesCollection[0].Values[0]=(double)SetMyBrainIndexes.attention;
                SeriesCollection[0].Values[1]=(double)SetMyBrainIndexes.creativity;
                SeriesCollection[0].Values[2]=(double)SetMyBrainIndexes.immersion;
                SeriesCollection[0].Values[3]=(double)SetMyBrainIndexes.arousal;
                SeriesCollection[0].Values[4]=(double)SetMyBrainIndexes.engagement;


                //SeriesCollection.Add(
                //    new RowSeries
                //    {
                //        Title="Attention",
                //        Values = new ChartValues<double> {
                //            SetMyBrainIndexes.attention 
                //        }
                //    });

                //SeriesCollection =new SeriesCollection() {
                //    new RowSeries
                //    {
                //        Values = new ChartValues<double> {
                //            SetMyBrainIndexes.attention
                //        }
                //    } };
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
