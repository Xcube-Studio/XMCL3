using MySql.Data.MySqlClient;
using System;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage2.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage2 : Page
    {
        public SubPage2()
        {
            InitializeComponent();
        }
        int state;
        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if((bool)state_button.IsChecked)
            {
                state_text.Content = "反馈选中：BUG";
                state = 1;
            }
            else
            {
                state_text.Content = "反馈选中：建议";
                state = 10;
            }
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            write.Visibility = Visibility.Collapsed;
            Get.Visibility = Visibility.Visible;
            System.Threading.Tasks.Task.Run(() =>
            {
                string ConString = "server=106.14.64.250;User Id=User;password=User20202020server;Database=User";
                MySqlConnection conn = new MySqlConnection(ConString);//连接数据库 
                conn.Open();   //open的时候可以套个try防止boom 
                int id = 1;
                int i;
                string sql = "select * from issues where id='" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                for (; ; id++)
                {

                    sql = "select * from issues where id='" + id + "'";
                    cmd.CommandText = sql;
                    i = Convert.ToInt32(cmd.ExecuteScalar());
                    if (i <= 0)
                    {
                        MessageBox.Show(id.ToString());
                        break;
                    }
                    

                }

               
                Dispatcher.Invoke(new Action(() =>
                {
                      sql = "Insert Into issues(id, text,reply,replied_by,_by,state,title)Values('" + id + "','" + text.Text + "','null','null','" + Name.Text + "','" + state + "','" + Title.Text + "')";
                      MessageBox.Show(sql);
                      cmd.CommandText = sql;
                }));

             
                
                i = Convert.ToInt32(cmd.ExecuteNonQuery());


                if (i > 0)
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                       OK.Visibility = Visibility.Visible;
                        Get.Visibility = Visibility.Collapsed;
                    }));
                
                else
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MainWindow.ShowTip("提交失败",5);
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "tmp1001.tmp");
                    }));
                





            });
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            OK.Visibility = Visibility.Collapsed;
            Get.Visibility = Visibility.Collapsed;
            state_text.Content = "反馈选中：BUG(请选择反馈类型)";
            state = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(AppDomain.CurrentDomain.BaseDirectory+"tmp1001.tmp");
        }
    }
}
