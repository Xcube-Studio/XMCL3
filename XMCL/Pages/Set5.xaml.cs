using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XMCL.Pages
{
    /// <summary>
    /// Set5.xaml 的交互逻辑
    /// </summary>
    public partial class Set5 : Page
    {
        public Set5()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Frame.Visibility = Visibility.Collapsed;
            try
            {

                Latest.Children.Clear();

                Task.Run(() =>
                {
                    string strcon = "server=106.14.64.250;User Id=User;password=User20202020server;Database=User";
                    string sql = $"select * from issues";
                    MySqlConnection conn = new MySqlConnection(strcon);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    int[] id = new int[200], state = new int[200];
                    string[] issues = new string[200], by = new string[200], reply = new string[200], replied_by = new string[200], title = new string[200];
                    int i = 1;
                    while (true)
                    {
                        try
                        {

                            reader.Read();
                            id[i] = (int)reader[0];
                            state[i] = (int)reader[5];
                            issues[i] = (string)reader[1];
                            reply[i] = (string)reader[2];
                            replied_by[i] = (string)reader[3];
                            by[i] = (string)reader[4];
                            title[i] = (string)reader[6];
                            if (i > id[i])
                                break;
                            i++;
                        }
                        catch { break; }
                    }
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {

                        for (int f = 1; f < i; f++)
                        {

                            ListBoxItem listBoxItem = new ListBoxItem();
                            ListBoxItem listBoxItem1 = new ListBoxItem();
                            listBoxItem.Margin = listBoxItem1.Margin = new Thickness(20, 0, 20, 0);
                            listBoxItem.Content = "(XMCL-" + id[f].ToString() + ") " + title[f] + "  By：" + by[f];
                            listBoxItem.Tag = "XMCL" + id[f].ToString();
                            Latest.Children.Add(listBoxItem);

                        }

                        pb1.Visibility = Visibility.Collapsed;


                    }));
                });
            }
            catch
            {
                MainWindow.ShowTip("从远程数据库拉取反馈失败,请重试", 1);
            }
        }

        private void Toissues_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new SubPage2());
        }
        //保留List的点击事件 SubPage3.xaml

    }
}
