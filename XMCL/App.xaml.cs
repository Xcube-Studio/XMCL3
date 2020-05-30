using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace XMCL
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string version = "Pre"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
        public static bool HasUpdated = false;

        [DllImport("kernel32.dll")]
        static extern int WinExec(string exeName, int operType);
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (HasUpdated)
            {
                List<string> vs = new List<string>();
                vs.Add("TIMEOUT /T 3");
                vs.Add("del \"" + Directory.GetCurrentDirectory() + "\\XMCL.exe\"");
                vs.Add("move \"" + Folder_XMCL + "\\Download\\XMCL.exe\" \"" + Directory.GetCurrentDirectory() + "\\XMCL.exe\"");
                vs.Add("TIMEOUT /T 1");
                vs.Add("start /d \"" + Directory.GetCurrentDirectory() + "\" XMCL.exe");
                vs.Add("del \"" + Directory.GetCurrentDirectory() + "\\Update.bat\"");
                File.WriteAllLines(Directory.GetCurrentDirectory() + "\\Update.bat", vs.ToArray());
                WinExec(Directory.GetCurrentDirectory() + "\\Update.bat", 0);
            }
        }
    }
}
