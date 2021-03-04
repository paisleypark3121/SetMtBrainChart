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
    /// Logica di interazione per WIndowUserControl.xaml
    /// </summary>
    public partial class WIndowUserControl : UserControl, INotifyPropertyChanged
    {
        private bool _move;
        public bool Move
        {
            get { return _move; }
            set { _move = value; OnPropertyChanged("move"); }
        }

        private bool _opacity;
        public bool OOpacity
        {
            get { return _opacity; }
            set { _opacity = value; OnPropertyChanged("opacity"); }
        }

        private bool _anchor;
        public bool Anchor
        {
            get { return _anchor; }
            set { _anchor = value; OnPropertyChanged("anchor"); }
        }

        private bool _minimize;
        public bool Minimize
        {
            get { return _minimize; }
            set { _minimize = value; OnPropertyChanged("minimize"); }
        }

        private bool _close;
        public bool Close
        {
            get { return _close; }
            set { _close = value; OnPropertyChanged("close"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WIndowUserControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            Move = true;
        }

        private void Opacity_Click(object sender, RoutedEventArgs e)
        {
            OOpacity = true;
        }

        private void Anchor_Click(object sender, RoutedEventArgs e)
        {
            Anchor = true;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Minimize = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close = true;
        }

        private void UP_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this).Height > 440)
                Window.GetWindow(this).Height -= 40;
        }

        private void DOWN_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Height += 40;
        }

        private void RIGHT_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Width += 40;
        }

        private void LEFT_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this).Width>550)
                Window.GetWindow(this).Width -= 40;
        }
    }
}
