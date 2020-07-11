using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;


namespace XMCL
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        static string UUID_Before;
        public Window2()
        {
            InitializeComponent();
            this.Owner = MainWindow.Window;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            UUID_Before = Settings.UUID;
        }
        public void GetUsersList()
        {
            List.Children.Clear();
            JArray jArray = Json.ReadUsers();
            for (int i=0;i<jArray.Count;i++)
            {
                Button button = new Button();
                button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                button.Margin = new System.Windows.Thickness(0, 0, 10, 0);
                button.Width = 45;
                button.Style = (Style)this.FindResource("MaterialDesignFlatButton");
                button.Background = button.BorderBrush = new SolidColorBrush(Colors.White);
                ShadowAssist.SetShadowDepth(button, ShadowDepth.Depth0);
                button.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#DD000000");
                button.Padding = new System.Windows.Thickness(0);
                button.Click += Button_Click1;
                button.Tag = JObject.Parse(jArray[i].ToString())["uuid"].ToString();

                PackIcon packIcon = new PackIcon();
                packIcon.Kind = PackIconKind.Delete;
                packIcon.Width = packIcon.Height = 22;
                button.Content = packIcon;

                RadioButton radioButton = new RadioButton();
                radioButton.Margin = new System.Windows.Thickness(10, 0, 0, 0);
                radioButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                radioButton.Width = 25;
                radioButton.GroupName = "A";
                radioButton.Tag = JObject.Parse(jArray[i].ToString())["uuid"].ToString();
                if (Settings.UUID == JObject.Parse(jArray[i].ToString())["uuid"].ToString())
                    radioButton.IsChecked = true;
                radioButton.Checked += RadioButton_Checked;

                Label label = new Label();
                label.Content = JObject.Parse(jArray[i].ToString())["userName"].ToString() + " - " + JObject.Parse(jArray[i].ToString())["Email"].ToString();
                label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                label.FontFamily = new FontFamily("Microsoft YaHei UI Light");
                label.FontSize = 14;
                label.Margin = new System.Windows.Thickness(30, 0, 90, 0);

                Grid grid = new Grid();
                grid.Height = 40;
                grid.Background = new SolidColorBrush(Colors.White);
                grid.Margin = new System.Windows.Thickness(5, 5, 5, 0);
                if (i == jArray.Count - 1)
                    grid.Margin = new System.Windows.Thickness(5, 5, 5, 5);

                grid.Children.Add(button);
                grid.Children.Add(radioButton);
                grid.Children.Add(label);

                List.Children.Add(grid);
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Json.ReMoveUser(button.Tag.ToString());
            GetUsersList();
            if (Json.ReadUsers().Count == 0)
            {
                Window1 window1 = new Window1();
                window1.Owner = this;
                window1.ShowDialog();
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            Json.Write("Login", "Choose", radioButton.Tag.ToString());
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetUsersList();
        }
        public static bool Show()
        {
            Window2 window = new Window2();
            window.ShowDialog();
            if (Settings.UUID == UUID_Before)
                return false;
            else return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.ShowDialog();
            GetUsersList();
        }
    }
}
