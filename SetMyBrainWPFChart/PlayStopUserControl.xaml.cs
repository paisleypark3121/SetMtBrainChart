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
    /// Logica di interazione per PlayStopUserControl.xaml
    /// </summary>
    public partial class PlayStopUserControl : UserControl, INotifyPropertyChanged
    {
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { 
                SetValue(ImageSourceProperty, value);
                if (ImageSource == "/img/play.png")
                    OnPropertyChanged("play");
                else
                    OnPropertyChanged("stop");
            }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(PlayStopUserControl), new PropertyMetadata(default(string)));

        public PlayStopUserControl()
        {
            InitializeComponent();

            ImageSource = "/img/play.png";

            DataContext = this;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hello, world!");

            if (ImageSource == "/img/play.png")
                ImageSource = "/img/stop.png";
            else
                ImageSource = "/img/play.png";
        }
    }
}
