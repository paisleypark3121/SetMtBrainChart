using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Logica di interazione per LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public bool logged = false;

        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (!logged)
            {
                //MessageBox.Show("LOGGING IN!");
                logged = true;

                //LoginWindow loginWindow = new LoginWindow();

                LogInOut.Text = "Logout";
            }
            else
            {
                //MessageBox.Show("LOGGING OUT!");
                logged = false;

                //logout();

                LogInOut.Text = "Login";
            }
        }

        private void logout()
        {
            string url = "https://graph.facebook.com/v9.0/me/permissions?debug=all&method=DELETE&access_token=";

            if (App.Current.Properties.Contains("access_token") &&
                !string.IsNullOrEmpty(App.Current.Properties["access_token"].ToString()))
            {
                string access_token = App.Current.Properties["access_token"].ToString();
                url += access_token;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string _response = reader.ReadToEnd();
                            MessageBox.Show(_response);
                        }
                    }
                }
            }
        }
    }
}
