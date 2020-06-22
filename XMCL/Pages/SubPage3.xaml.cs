using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;

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
        int id = 0;
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
                string sql = $"select * from issues where id‘" + id.ToString() + "'";
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
                Title1.Content = issues;
                By.Content = by;
                if (state >= 1 || state < 10)
                    state1.Content = "反馈类型：BUG";
                else
                    state1.Content = "反馈类型：建议";
                Text.Content = issues;
                if (state >= 2 & state < 10 | state > 10)
                {
                    state2.Content = "开发者已经回应";
                    reply_by.Content = "回复：" + replied_by;
                    reply_.Content = "回复 \n " + reply;
                }
                else
                {
                    state2.Content = "开发者暂未查看";
                    reply_by.Visibility = Visibility.Collapsed;
                    reply_.Visibility = Visibility.Collapsed;
                }
            });
        }
    }
}
