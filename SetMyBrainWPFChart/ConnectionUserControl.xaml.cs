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
    /// Logica di interazione per ConnectionUserControl.xaml
    /// </summary>
    public partial class ConnectionUserControl : UserControl, INotifyPropertyChanged
    {
        public float PoorSignal
        {
            get { return (float)GetValue(PoorSignalProperty); }
            set
            {
                SetValue(PoorSignalProperty, value);

                if (PoorSignal == 200)
                {
                    Label = "NO CONNECTION";
                    Color = "#FF0000";
                }
                else if (PoorSignal > 200)
                {
                    Label = "Connected";
                    Color = "#32CD32";
                }
                else if (PoorSignal > 100)
                {
                    Label = "Poor Connection";
                    Color = "#FFFF00";
                }
                else
                {
                    Label = "Not Connected";
                    Color = "#FF0000";
                }
            }
        }

        // Using a DependencyProperty as the backing store for PoorSignal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PoorSignalProperty =
            DependencyProperty.Register("PoorSignal", typeof(float), typeof(ConnectionUserControl), new PropertyMetadata(default(float)));

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(ConnectionUserControl), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ConnectionUserControl), new PropertyMetadata(default(string)));

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ConnectionUserControl()
        {
            InitializeComponent();

            //Color = "#32CD32";
            //Label = "Connected";

            Color = "#FF0000";
            Label = "Not Connected";

            DataContext = this;
        }
    }
}
