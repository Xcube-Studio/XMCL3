using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XL.Core.Authentication;
using Newtonsoft.Json.Linq;

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
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        public static void login(Window Window)
        {
            Window1 window1 = new Window1();
            window1.Owner = Window;
            window1.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window1.ShowDialog();
        }
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
                        YggdrasilAuthenticator yggdrasilAuthenticator = new YggdrasilAuthenticator(email, password);
                        yggdrasilAuthenticator.Authenticate();
                        if (yggdrasilAuthenticator.GetUserType() == "Mojang")
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                JObject jObject = yggdrasilAuthenticator.GetResult();
                                Json.AddUsers((string)jObject["selectedProfile"]["name"], (string)jObject["selectedProfile"]["id"], (string)jObject["accessToken"], "正版", email);
                                Json.Write("Login", "Choose", (string)jObject["selectedProfile"]["id"]);
                                Loading.Visibility = Visibility.Collapsed;
                                this.Close();
                            }));
                        else this.Dispatcher.BeginInvoke(new Action(() =>
                             {
                                 MainWindow.ShowTip("登陆失败,请检查密码.[或多次重复被服务器禁止登录]", 3);
                             })); 
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
                OfflineAuthenticator offlineAuthenticator = new OfflineAuthenticator(NameTextBox.Text);
                offlineAuthenticator.Authenticate();
                JObject jObject = offlineAuthenticator.GetResult();
                Json.AddUsers((string)jObject["selectedProfile"]["name"], (string)jObject["selectedProfile"]["id"], (string)jObject["accessToken"], "离线", "");
                Json.Write("Login", "Choose", (string)jObject["selectedProfile"]["id"]);
                Loading.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }
    }
}
