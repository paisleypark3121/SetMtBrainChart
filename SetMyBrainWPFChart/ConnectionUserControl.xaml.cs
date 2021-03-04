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
        private float _poorSignal;
        
        public float PoorSignal
        {
            get { return _poorSignal; }
            set
            {
                _poorSignal = value;
                OnPropertyChanged("PoorSignalChanged");

                //if (value == 200)
                //{
                //    Label = "NO CONNECTION";
                //    Color = "#FF0000";
                //}
                //else if (value > 200)
                //{
                //    Label = "Connected";
                //    Color = "#32CD32";
                //}
                //else if (value > 100)
                //{
                //    Label = "Poor Connection";
                //    Color = "#FFFF00";
                //}
                //else
                //{
                //    Label = "Not Connected";
                //    Color = "#FF0000";
                //}
            }
        }

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(ConnectionUserControl), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ConnectionUserControl), new PropertyMetadata(default(string)));

        public ConnectionUserControl()
        {
            InitializeComponent();

            //Color = "#32CD32";
            //Label = "Connected";

            Color = "#FF0000";
            Label = "KO";

            this.PropertyChanged += ColorAndLabelChange;

            DataContext = this;
        }

        private void ColorAndLabelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PoorSignalChanged")
            {
                if (_poorSignal == 200)
                {
                    Label = "KO";
                    Color = "#FF0000";
                }
                else if (_poorSignal < 30)
                {
                    Label = "OK";
                    Color = "#32CD32";
                }
                else if (_poorSignal < 120)
                {
                    Label = "Poor";
                    Color = "#FFFF00";
                }
                else if (_poorSignal < 160)
                {
                    Label = "Very Poor";
                    Color = "#FFFF00";
                }
                else
                {
                    Label = "Not Connected";
                    Color = "#FF0000";
                }
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
