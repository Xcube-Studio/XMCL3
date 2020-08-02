using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XL.Core;
using XL.Core.Tools;

namespace XMCL.Pages
{
    /// <summary>
    /// Page4.xaml 的交互逻辑
    /// </summary>
    public partial class Page4 : Page
    {
        bool IsLoading = false;
        static Label Load;
        public Page4()
        {
            InitializeComponent();
            Load = Label1;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Resources.Remove("PrimaryHueMidBrush");
            this.Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            this.Resources.Remove("PrimaryHueLightBrush");
            this.Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            this.Resources.Remove("PrimaryHueDarkBrush");
            this.Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
            if (IsLoading)
                return;
            #region Update

            Label1.Content = "检查更新";
            try {
            await Task.Run(() =>
            {
                WebClient webClient = new WebClient();
                string[] a;
                if (App.version.Contains("Pre"))
                    a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/debug.html")).Split('#');
                else a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/api.html")).Split('#');
                if (a[0] != App.version)
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.NavigationService.Navigate(new Page3());
                    }));
                webClient.Dispose();
            });
            }
            catch { Label1.Content = "检查更新失败"; }
            #endregion
            #region JSON
            Label1.Content = "检查配置文件";
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

            if (Settings.JavaPath == null || Settings.JavaPath.Length == 0)
            {
                List<string> vs = Java.GetJavaList();
                if (vs != null)
                    Json.Write("Files", "JavaPath", vs[0]);
            }
            if (Json.ReadPaths().Count == 0)
                Json.AddPath("官方启动器", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft", "Grass_Block", false);
            if (Settings.GamePathName.Length == 0)
                Json.Write("Files", "GamePathName", Newtonsoft.Json.Linq.JObject.Parse(Json.ReadPaths()[0].ToString())["Name"].ToString());
            Json.ChangePath("官方启动器", "Path", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");

            ComboBox C1 = MainWindow.ComboBox;
            C1.ItemsSource = SomethingUseful.GetVersions(Settings.GamePath);
            if (Settings.LatestVerison.Contains(" "))
            {
                string[] vs = Settings.LatestVerison.Split(' ');
                if (vs[0] == Settings.GamePath)
                {
                    for (int i = 0; i < C1.Items.Count; i++)
                    {
                        if (C1.Items[i].ToString() == vs[1])
                            C1.SelectedIndex = i;
                    }
                }
            }
            #endregion
            #region Timer
            Label1.Content = "开启性能检查器";
            await Task.Run(() =>
            {
                MainWindow.performance = new Performance();
                MainWindow.timer = new System.Timers.Timer();
                MainWindow.timer.Elapsed += MainWindow.Timer_Elapsed;
                MainWindow.timer.Enabled = true;
                MainWindow.timer.Interval = 1500;
                MainWindow.timer.Start();
            });
            #endregion
            Label1.Content = "检查账户";
            MainWindow.Login();
            this.NavigationService.Navigate(null);
        }
        public async static void Loading(Action action,string text)
        {
            Page4 page4 = new Page4();
            page4.IsLoading = true;
            MainWindow.Frame1.Navigate(page4);
            Load.Content = text;
            await Task.Run(action);
            MainWindow.Frame1.Navigate(null);
        }
    }
}
