using System;
using System.Collections.Generic;
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

namespace XMCL.Pages
{
    /// <summary>
    /// Page4.xaml 的交互逻辑
    /// </summary>
    public partial class Page4 : Page
    {
        string[] a;
        public Page4()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
         {
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
                    Task.Run(delegate
                    {
                        if (!System.IO.Directory.Exists(App.Folder_XMCL + "\\Download\\"))
                            System.IO.Directory.CreateDirectory(App.Folder_XMCL + "\\Download\\");
                        DownloadFile(a[1], App.Folder_XMCL + "\\Download\\XMCL.exe", ProgressBar1);
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
            System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
            long totalBytes = myrp.ContentLength;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
            }));

            System.IO.Stream st = myrp.GetResponseStream();
            System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            long totalDownloadedByte = 0;
            byte[] by = new byte[1024];
            int osize = st.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {
                totalDownloadedByte = osize + totalDownloadedByte;
                so.Write(by, 0, osize);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                }));
                osize = st.Read(by, 0, (int)by.Length);
            }
            myrp.Dispose();
            so.Close();
            st.Close();
            this.Dispatcher.Invoke(new Action(() =>
            {
                App.HasUpdated = true;
                Application.Current.Shutdown();
            }));
        }
    }
}
