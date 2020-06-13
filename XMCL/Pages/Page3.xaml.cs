using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XMCL.Core;

namespace XMCL.Pages
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        static StackPanel StackPanel;
        static Page Page;
        public Page3()
        {
            InitializeComponent();
            StackPanel = List;
            Page = this;
        }
        public static void AddDownloadTask(string url)
        {
            Task.Run(() =>
            {
                ProgressBar progressBar = null;
                Label label1 = null;
                string Filename = System.IO.Path.GetFileName(url);
                Page.Dispatcher.Invoke(new Action(() =>
                {
                    ListBoxItem listBoxItem = new ListBoxItem();
                    Grid grid = new Grid();
                    grid.Margin = new Thickness(0, 5, 0, 5);
                    grid.Width = 721;
                    Label label = new Label();
                    label.Margin = new Thickness(10, 0, 10, 10);
                    label.Content = Filename;
                    progressBar = new ProgressBar();
                    progressBar.Margin = new Thickness(5, 25, 5, 4);
                    label1 = new Label();
                    label1.HorizontalContentAlignment = HorizontalAlignment.Right;
                    grid.Children.Add(label);
                    grid.Children.Add(label1);
                    grid.Children.Add(progressBar);
                    listBoxItem.Content = grid;
                    StackPanel.Children.Add(listBoxItem);
                    if (Filename.Contains(".json"))
                    {
                        string GamePath;
                        if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                            GamePath = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");
                        else GamePath = Json.Read("Files", "GamePath");
                        if (!(Directory.Exists(GamePath + "\\versions\\" + Filename.Replace(".json", "") + "\\")))
                            Directory.CreateDirectory(GamePath + "\\versions\\" + Filename.Replace(".json", "") + "\\");
                        Filename = GamePath + "\\versions\\" + Filename.Replace(".json", "") + "\\" + Filename;
                    }
                    else Filename = App.Folder_XMCL + "\\Download\\" + Filename;
                }));
                DownloadFile(url, Filename, progressBar, label1);
            });
        }
        public static void DownloadFile(string URL, string filename, ProgressBar prog, Label label)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            float percent = 0;
            System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
            System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
            long totalBytes = myrp.ContentLength;
            Page.Dispatcher.BeginInvoke(new Action(() =>
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
                Page.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                }));
                osize = st.Read(by, 0, (int)by.Length);
                percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                Page.Dispatcher.BeginInvoke(new Action(() =>
                {
                    label.Content = "正在下载..." + percent.ToString() + "%";
                }));
            }
            myrp.Dispose();
            so.Close();
            st.Close();
            Page.Dispatcher.BeginInvoke(new Action(() =>
            {
                label.Content = "下载完成";
                MainWindow.ShowTip("下载完成   " + filename, 1);
            }));
        }
    }
}
