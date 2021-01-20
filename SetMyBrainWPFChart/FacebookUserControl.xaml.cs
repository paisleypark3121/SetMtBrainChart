using Newtonsoft.Json.Linq;
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
    /// Logica di interazione per FacebookUserControl.xaml
    /// </summary>
    public partial class FacebookUserControl : UserControl
    {
        string app_id = "155800916106875";
        string redirect_uri = "https://www.facebook.com/connect/login_success.html";
        string source;
        string access_token = "";

        //WebBrowser wb;

        public FacebookUserControl()
        {
            InitializeComponent();

            //source= "https://www.facebook.com/v9.0/dialog/oauth?client_id=" + app_id + "&redirect_uri=" + redirect_uri + "&response_type=token";

            ////https://www.facebook.com/v9.0/dialog/oauth?client_id=155800916106875&redirect_uri=https://www.facebook.com/connect/login_success.html&response_type=token

            //wb = new WebBrowser();

            //wb.Navigated += (sender, e) =>
            //{
            //    if (e.Uri.ToString().Contains("login_success"))
            //    {
            //        MessageBox.Show("OK!");
            //    }
            //};
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //wb.Navigate(source);
            LoginWindow loginWindow = new LoginWindow();
            bool? ret=loginWindow.ShowDialog();
            

            //SocialBrowser.Navigate(source);

            //SocialBrowser.Navigated += (sender, e) =>
            //{
            //    if (e.Uri.ToString().Contains("login_success"))
            //    {
            //        MessageBox.Show("OK!");
            //    }
            //};
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
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
