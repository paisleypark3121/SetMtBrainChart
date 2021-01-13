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
    /// Logica di interazione per TimeUserControl.xaml
    /// </summary>
    public partial class TimeUserControl : UserControl, INotifyPropertyChanged
    {
        public DateTime Now
        {
            get { return (DateTime)GetValue(NowProperty); }
            set { SetValue(NowProperty, value); }
        }

        public static readonly DependencyProperty NowProperty =
            DependencyProperty.Register("Now", typeof(DateTime), typeof(TimeUserControl), new PropertyMetadata(default(DateTime)));

        public DateTime Start
        {
            get { return (DateTime)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(DateTime), typeof(TimeUserControl), new PropertyMetadata(default(DateTime)));

        public TimeSpan Elapsed
        {
            get { return (TimeSpan)GetValue(ElapsedProperty); }
            set { SetValue(ElapsedProperty, value); }
        }

        public static readonly DependencyProperty ElapsedProperty =
            DependencyProperty.Register("Elapsed", typeof(TimeSpan), typeof(TimeUserControl), new PropertyMetadata(default(TimeSpan)));

        public TimeUserControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
