using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SetMyBrainWPFChart
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                MainWindow mainWindow = new MainWindow();

                string mode = ConfigurationManager.AppSettings["mode"];
                if (mode == "overlay")
                {   
                    //mainWindow.Loaded += mw_Loaded;
                    mainWindow.AllowsTransparency = true;
                    mainWindow.Opacity = 1;
                    mainWindow.WindowStyle = WindowStyle.None;
                }

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    //Uri uri = StartupUri;
        //    try
        //    {
        //        string mode = ConfigurationManager.AppSettings["mode"];
        //        if (mode=="overlay")
        //        {

        //            MainWindow mainWindow = new MainWindow();
        //            mainWindow.AllowsTransparency = true;
        //            mainWindow.Opacity = 1;
        //            mainWindow.WindowStyle = WindowStyle.None;
        //            mainWindow.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Trace.TraceError(ex.Message);
        //    }
        //}
    }
}
