using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System;
namespace XMCL.Pages
{
    /// <summary>
    /// SubPage3.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage3 : Page
    {
        public static Page Page;
        public SubPage3()
        {
            InitializeComponent();
            Page = this;
        }
        public static int id = 0;
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));

            Task.Run(() =>
            {
                string strcon = "server=106.14.64.250;User Id=User;password=User20202020server;Database=User";
                string sql = $"select * from issues where id='" + id.ToString() + "'";
                MySqlConnection conn = new MySqlConnection(strcon);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                string issues, by, reply, replied_by, title;
                int state;
                reader.Read();

                state = (int)reader[5];
                issues = (string)reader[1];
                reply = (string)reader[2];
                replied_by = (string)reader[3];
                by = (string)reader[4];
                title = (string)reader[6];
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Title1.Content = issues;
                    if (state >= 1 || state < 10)
                        state1.Content = "反馈类型：BUG";
                    else state1.Content = "反馈类型：建议";
                    if (state >= 2 & state < 10 | state > 10)
                    {
                        state2.Content = "已回应";
                        TextBox1.Text += "回复的反馈: " + by + "     " + "回复的开发者：" + replied_by + "\r\n";

                        TextBox1.Text += "================正文================" + "\r\n";
                        TextBox1.Text += issues + "\r\n";
                        TextBox1.Text += "================回复================" + "\r\n";
                        TextBox1.Text += reply + "\r\n";
                        TextBox1.Text += "===================================" + "\r\n";
                    }
                    else state2.Content = "开发者暂未查看";
                }));
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(null);
        }
    }
}
