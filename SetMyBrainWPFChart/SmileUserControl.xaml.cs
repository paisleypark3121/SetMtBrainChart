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
        //#region index
        //private string _setSmileValueIndex;
        //public string SetSmileValueIndex
        //{
        //    get { return _setSmileValueIndex; }
        //    set { _setSmileValueIndex = value; OnPropertyChanged("SetSmileValueIndex"); }
        //}
        //#endregion

        //public string SmileIndexValue { get; set; }

        public string SmileIndexValue
        {
            get { return (string)GetValue(SmileIndexValueProperty); }
            set { SetValue(SmileIndexValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmileIndexValue.  This enables animation, styling, binding, etc...
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

            //this.PropertyChanged += SmileValueIndexView;

            DataContext = this;
        }

        //private void SmileValueIndexView(object sender, PropertyChangedEventArgs e)
        //{
        //    //if (e.PropertyName == "SetSmileValueIndex")
        //    //{
        //    //    //SmileIndexValue = SetSmileValueIndex;
        //    //    Console.WriteLine(SmileIndexValue);
        //    //}
        //}

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
