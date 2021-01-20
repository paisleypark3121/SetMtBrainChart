using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SetMyBrainWPFChart
{
    /// <summary>
    /// Logica di interazione per LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string app_id = "155800916106875";
        string redirect_uri = "https://www.facebook.com/connect/login_success.html";
        string source = "https://www.facebook.com/v9.0/dialog/oauth?client_id=";

        public LoginWindow()
        {
            InitializeComponent();

            source += app_id + "&redirect_uri=" + redirect_uri + "&response_type=token";

            //https://www.facebook.com/v9.0/dialog/oauth?client_id=155800916106875&redirect_uri=https://www.facebook.com/connect/login_success.html&response_type=token

            LoginBrowser.Navigated += (sender, e) =>
            {
                if (e.Uri.ToString().Contains("login_success"))
                {   
                    string fragment = e.Uri.Fragment;
                    fragment = fragment.Substring(1);
                    NameValueCollection coll = HttpUtility.ParseQueryString(fragment);
                    App.Current.Properties["access_token"] = coll["access_token"];

                    MessageBox.Show("LOGGED IN!");
                    this.Close();
                }
            };

            LoginBrowser.Navigate(source);
        }
    }
}
