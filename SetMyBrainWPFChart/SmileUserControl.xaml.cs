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
    /// Logica di interazione per SmileUserControl.xaml
    /// </summary>
    public partial class SmileUserControl : UserControl, INotifyPropertyChanged
    {
        private bool _flow;
        public bool Flow
        {
            get { return _flow; }
            set
            {
                _flow = value;
                OnPropertyChanged("FlowChanged");
            }
        }

        public string SmileIndexValue
        {
            get { return (string)GetValue(SmileIndexValueProperty); }
            set { SetValue(SmileIndexValueProperty, value); }
        }

        public static readonly DependencyProperty SmileIndexValueProperty =
            DependencyProperty.Register("SmileIndexValue", typeof(string), typeof(SmileUserControl), new PropertyMetadata(default(string)));

        #region visibility
        private bool _redValueVisibility;
        private bool _orangeValueVisibility;
        private bool _yellowValueVisibility;
        private bool _greenValueVisibility;
        private bool _green2ValueVisibility;

        public bool RedValueVisibility
        {
            get { return _redValueVisibility; }
            set
            {
                _redValueVisibility = value;
                OnPropertyChanged("RedValueVisibility");
            }
        }
        public bool OrangeValueVisibility
        {
            get { return _orangeValueVisibility; }
            set
            {
                _orangeValueVisibility = value;
                OnPropertyChanged("OrangeValueVisibility");
            }
        }
        public bool YellowValueVisibility
        {
            get { return _yellowValueVisibility; }
            set
            {
                _yellowValueVisibility = value;
                OnPropertyChanged("YellowValueVisibility");
            }
        }
        public bool GreenValueVisibility
        {
            get { return _greenValueVisibility; }
            set
            {
                _greenValueVisibility = value;
                OnPropertyChanged("GreenValueVisibility");
            }
        }
        public bool Green2ValueVisibility
        {
            get { return _green2ValueVisibility; }
            set
            {
                _green2ValueVisibility = value;
                OnPropertyChanged("Green2ValueVisibility");
            }
        }
        #endregion

        public SmileUserControl()
        {
            InitializeComponent();

            _redValueVisibility = false;
            _orangeValueVisibility = false;
            _yellowValueVisibility = true;
            _greenValueVisibility = false;
            _green2ValueVisibility = false;

            SmileIndexValue = "Index: 0";

            this.PropertyChanged += SmileChange;

            DataContext = this;
        }

        private void SmileChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FlowChanged")
            {
                if (_flow == true)
                {
                    RedValueVisibility = false;
                    OrangeValueVisibility = false;
                    YellowValueVisibility = false;
                    GreenValueVisibility = false;
                    Green2ValueVisibility = true;
                    SmileIndexValue = "In the flow!";
                }
                else 
                {
                    RedValueVisibility = true;
                    OrangeValueVisibility = false;
                    YellowValueVisibility = false;
                    GreenValueVisibility = false;
                    Green2ValueVisibility = false;
                    SmileIndexValue = "Non yet in the flow...";
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
