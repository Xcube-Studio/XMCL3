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
        public Page4()
        {
            InitializeComponent();
        }
        string ver1 = "Debug3.001";
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ver1.Contains("Debug"))
            {
                try
                {
                    string[] a = MainWindow.get_string("http://api.axing6.cn/debug.html");
                    if (a[0] != ver1)
                    {

                        WebClient myWebClient = new WebClient();
                        myWebClient.DownloadFile(a[1], "DebugXMCL.exe");
                    }
                }
                catch { MessageBox.Show("下载发生问题！请重启XMCL ");  }
            }
            else
            {
                string[] a = MainWindow.get_string("http://api.axing6.cn/api.html");
                if (a[0] != ver1)
                {
                    try
                    {
                        WebClient myWebClient = new WebClient();
                        myWebClient.DownloadFile(a[1], "XMCL.exe");
                    }
                    catch { MessageBox.Show("下载发生问题！请重启XMCL "); }
                }
            }
        }
    }
}
