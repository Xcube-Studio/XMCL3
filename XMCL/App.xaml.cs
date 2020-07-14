using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using XL.Core;

namespace XMCL
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string version = "Pre" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
               typeof(Timeline),
               new FrameworkPropertyMetadata { DefaultValue = 65 }
               );
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL"))
            { }
            else Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL");
            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\XMCL.json"))
            { }
            else
            {
                FileStream fs1 = new FileStream(System.Environment.CurrentDirectory + "\\XMCL.json", FileMode.Create, FileAccess.ReadWrite);
                try
                {
                    fs1.Write(XMCL.Properties.Resources.XMCL, 0, XMCL.Properties.Resources.XMCL.Length);
                    fs1.Flush();
                    fs1.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (Settings.JavaPath.Length > 0)
            { }
            else
            {
                List<string> vs = Java.GetJavaList();
                if (vs != null)
                    Json.Write("Files", "JavaPath", vs[0]);
            }
        }
        public static string Folder_XMCL = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.XMCL";
        public static void Themes(string color_str , string color_str1 , string color_str2)
        {
            Json.Write("Individualization", "PrimaryHueMidBrush", color_str);
            Json.Write("Individualization", "PrimaryHueLightBrush", color_str1);
            Json.Write("Individualization", "PrimaryHueDarkBrush", color_str2);
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueMidBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueLightBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            XMCL.MainWindow.Window.Resources.Remove("PrimaryHueDarkBrush");
            XMCL.MainWindow.Window.Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
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
