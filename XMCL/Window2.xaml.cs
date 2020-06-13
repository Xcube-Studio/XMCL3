using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using XMCL.Core;

namespace XMCL
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            this.Owner = MainWindow.Window;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            G1.Visibility = Visibility.Visible; G2.Visibility = Visibility.Collapsed;
            Get();
        }
        public void Get()
        {
            string uuid = Json.Read("Login", "choose");
            Label_name.Content = Json.ReadUser(uuid, "userName");
            Label_uuid.Content = uuid;
            Label_accessToken.Content = Json.ReadUser(uuid, "accessToken");
            Label_LoginMode.Content = Json.ReadUser(uuid, "LoginMode");
        }
        private void TreeViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            GC.Collect();
        }
        private void TreeViewItem_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            G1.Visibility = Visibility.Visible; G2.Visibility = Visibility.Collapsed;
            Get();
        }
        private void TreeViewItem_PreviewMouseDown_2(object sender, MouseButtonEventArgs e)
        {
            G1.Visibility = Visibility.Collapsed; G2.Visibility = Visibility.Visible;
            JArray jArray = JArray.Parse(Json.ReadUsers());
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject jObject = JObject.Parse(jArray[i].ToString());
                Grid grid = new Grid();
                Label label = new Label();
                Label label1 = new Label();
                label1.Width = StackPanel1.Width;
                label1.Height = 5;
                if (i == 0)
                    grid.Margin = new Thickness(20, 20, 20, 0);
                else if (i == jArray.Count)
                    grid.Margin = new Thickness(20, 0, 20, 20);
                else grid.Margin = new Thickness(20, 0, 20, 0);
                grid.Background = new SolidColorBrush(Colors.White);
                label.Foreground = new SolidColorBrush(Colors.Black);
                label.Margin = new Thickness(10);
                label.FontFamily = new FontFamily("Microsoft YaHei UI Light");
                label.Content = "名字: " + jObject["userName"].ToString() + " uuid: " + jObject["uuid"].ToString() + "\r\n登录方式: " + jObject["LoginMode"].ToString();
                if (jObject["uuid"].ToString() == Json.Read("Login", "choose"))
                    label.Content += "   正在使用";
                label.VerticalContentAlignment = VerticalAlignment.Center;
                grid.Height = label.Height + 20;
                grid.Children.Add(label);
                StackPanel1.Children.Add(label1);
                StackPanel1.Children.Add(grid);
            }
        }
    }
}
