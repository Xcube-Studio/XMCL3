using System;
using System.Windows;
using System.Windows.Media;
using System.IO;

namespace XMCL
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL"))
            { }
            else Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL");
        }
        public static string Folder_XMCL = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL";
        public static void Thems(string color1,string color2,string color3)
        {
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueMidBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((Color)ColorConverter.ConvertFromString(color1)));
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueLightBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((Color)ColorConverter.ConvertFromString(color2)));
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueDarkBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((Color)ColorConverter.ConvertFromString(color3)));
        }
    }
}
