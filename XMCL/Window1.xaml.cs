using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XMCL.Core;

namespace XMCL
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public static void login()
        {
            Window1 window1 = new Window1();
            window1.Owner = Window;
            window1.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window1.ShowDialog();
        }
        public static Window Window { get; set; }
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
                PasswordBox.Visibility = Visibility.Visible;
            else PasswordBox.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            Button button = (Button)sender;
            PasswordBox.IsEnabled = NameTextBox.IsEnabled = ComboBox.IsEnabled = button.IsEnabled = false;
            if (ComboBox.SelectedIndex == 0)
            {
                try
                {
                    Task.Run(() =>
                    {
                        string email = null; string password = null;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            email = NameTextBox.Text;
                            password = PasswordBox.Password;
                        }));
                        if (Authenticate.Login(email, password, false))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Loading.Visibility = Visibility.Collapsed;
                                this.Close();
                            }));
                        }
                        else MainWindow.ShowTip("登陆失败,请检查密码.[或多次重复被服务器禁止登录]",1);
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            PasswordBox.IsEnabled = NameTextBox.IsEnabled = ComboBox.IsEnabled = button.IsEnabled = true;
                            Loading.Visibility = Visibility.Collapsed;
                        }));
                    });
                }
                catch { }
            }
            else
            {
                Loading.Visibility = Visibility.Collapsed;
                Authenticate.Offline(NameTextBox.Text);
                this.Close();
            }
        }
    }
}
