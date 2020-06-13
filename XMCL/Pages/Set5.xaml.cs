using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private void Loaded_()
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
                            listBoxItem.MouseDoubleClick += ListBoxItem_MouseDoubleClick;
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
            string str = AppDomain.CurrentDomain.BaseDirectory;
            Frame.Visibility = Visibility.Visible;
            Frame.Navigate(new SubPage2());
            FileStream fs = new FileStream(str + "tmp1001.tmp", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
            StreamWriter sw = new StreamWriter(fs); // 创建写入流
            sw.WriteLine("tmp"); 
            sw.Close();
            Task.Run(() =>
            {
                while (true)
                {
                    if (!File.Exists(str + "tmp1001.tmp"))
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Frame.Visibility = Visibility.Collapsed;
                    Loaded_();
                }));


            });

        }
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        /*不会分辨id，所以现在这写注释
         System.Threading.Tasks.Task.Run(() =>
            {
                string ConString = "server=106.14.64.250;User Id=User;password=User20202020server;Database=User";
                MySqlConnection conn = new MySqlConnection(ConString);//连接数据库 
                conn.Open();  
                string sql = "select * from issues where id='" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                string issues , by, reply , replied_by , title ;
                reader.Read();       
                state = (int)reader[5];
                issues = (string)reader[1];
                reply = (string)reader[2];
                replied_by = (string)reader[3];
                by = (string)reader[4];
                title = (string)reader[6];
                string str = AppDomain.CurrentDomain.BaseDirectory;
                Frame.Visibility = Visibility.Visible;
                Frame.Navigate(new SubPage2());
                FileStream fs = new FileStream(str + "tmp1002.tmp", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                StreamWriter sw = new StreamWriter(fs); // 创建写入流
                sw.WriteLine(id.Tostring()+"|"+issues+"|"+reply+"|"+reply_by+"|"+by+"|"+title+"|"+state.Tostring()); 
                sw.Close();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Frame.Visibility = Visibility.Visible;
                    Frame.Navigate(new SubPage3());
                }));
                
                while (true)
                {
                    if (!File.Exists(str + "tmp1002.tmp"))
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Frame.Visibility = Visibility.Collapsed;
                    Loaded_();
                }));
                
                
                
        });
         */
    }
}
