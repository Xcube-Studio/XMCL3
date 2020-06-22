using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace XMCL.Pages
{
    /// <summary>
    /// Page4.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        string[] a;
        public static Page Page;
        public Page3()
        {
            InitializeComponent();
            Page = this;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.ColorZone.Visibility = Visibility.Collapsed;
            Task.Run(() =>
            {
                WebClient webClient = new WebClient();
                if (App.version.Contains("Pre"))
                    a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/debug.html")).Split('#');
                else a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/api.html")).Split('#');
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ProgressBar1.Visibility = Visibility.Collapsed;
                    LV.Content += App.version + "    →" + a[0];
                }));
            });
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));

        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Button_Update.IsChecked = true;
            Button_Update.IsEnabled = false;
            ProgressBar1.Visibility = Visibility.Visible;
            if (a[0] != App.version)
            {
                try
                {
                    Task.Run(async delegate
                    {
                        if (!System.IO.Directory.Exists(App.Folder_XMCL + "\\Download\\"))
                            System.IO.Directory.CreateDirectory(App.Folder_XMCL + "\\Download\\");
                        DownloadFile(a[1], App.Folder_XMCL + "\\Download\\XMCL.exe", ProgressBar1);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ProgressBar1.IsIndeterminate = true;
                        }));
                        await Task.Delay(1500);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Application.Current.Shutdown();
                        }));
                    });
                }
                catch
                {
                    MainWindow.ShowTip("下载发生问题！请重试", 1);
                    Button_Update.IsEnabled = true; ProgressBar1.Visibility = Visibility.Collapsed;
                }
            }
        }
        public void DownloadFile(string URL, string filename, ProgressBar prog)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                prog.IsIndeterminate = false;
            }));
            HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
            request.Timeout = 3000;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            this.Dispatcher.Invoke(new Action(() =>
            {
                prog.Maximum = response.ContentLength;
            }));
            Stream responseStream = response.GetResponseStream();
            Stream stream = new FileStream(filename, FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    prog.Value += size;
                }));
            }
            request.Abort();
            stream.Dispose();
            responseStream.Dispose();
            response.Dispose();
            this.Dispatcher.Invoke(new Action(() =>
            {
                App.HasUpdated = true;
            }));
        }
    }
}
