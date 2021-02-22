using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
    /// Logica di interazione per TestUserControl.xaml
    /// </summary>
    public partial class TestUserControl : UserControl
    {
        public TestUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Utilities.Utilities.CaptureScreen("capture.jpg");
        }
    }
}
