using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using WinForm = System.Windows.Forms;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Windows.Data;

namespace XMCL.Pages
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        int egg = 0;
        public static Page Page;
        public Page2()
        {
            InitializeComponent();
            Page = this;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            try { this.NavigationService.Navigate(null); } catch { }
        }
        private void Button_OpenJava_Click(object sender, RoutedEventArgs e)
        {
            WinForm.OpenFileDialog dialog = new WinForm.OpenFileDialog();
            dialog.Filter = "javaw.exe|javaw.exe";
            dialog.Title = "选择Java";
            dialog.ShowDialog();
            TextBox_JavaPath.Text = dialog.FileName;
            dialog.Dispose();
        }
        private void Toissues_Click(object sender, RoutedEventArgs e)
        {
            Frame.Visibility = Visibility.Visible;
            Frame.Navigate(new SubPage2());
        }
        private void MySQL_Loaded()
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
                            listBoxItem.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown;
                            listBoxItem.Margin = new Thickness(20, 0, 20, 0);
                            listBoxItem.Content = "(XMCL-" + id[f].ToString() + ") " + title[f] + "  By：" + by[f];
                            listBoxItem.Tag = f;
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
        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;
            Frame.Visibility = Visibility.Visible;
            SubPage3.id = (int)listBoxItem.Tag;
            Frame.Navigate(new SubPage3());
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));

            if (System.IO.File.Exists(App.Folder_XMCL + "\\Themes.json"))
            {
                try
                {
                    Themes.Children.Clear();
                    JArray jArray = JArray.Parse(System.IO.File.ReadAllText(App.Folder_XMCL + "\\Themes.json",System.Text.Encoding.UTF8));
                    foreach (JObject jObject in jArray)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString(jObject["PrimaryHueMidBrush"].ToString());
                        Color color1 = (Color)ColorConverter.ConvertFromString(jObject["PrimaryHueLightBrush"].ToString());
                        Color color2 = (Color)ColorConverter.ConvertFromString(jObject["PrimaryHueDarkBrush"].ToString());

                        Border border = new Border();
                        border.Margin = new Thickness(10);
                        border.Height = 50;
                        border.BorderBrush = new SolidColorBrush(color1);
                        border.Background = new SolidColorBrush(Colors.White);
                        border.BorderThickness = new Thickness(1);

                        System.Windows.Shapes.Rectangle rectangle1 = new System.Windows.Shapes.Rectangle();
                        rectangle1.Fill = new SolidColorBrush(color1);
                        rectangle1.Margin = new Thickness(0, 0, 580, 0);

                        System.Windows.Shapes.Rectangle rectangle2 = new System.Windows.Shapes.Rectangle();
                        rectangle2.Fill = new SolidColorBrush(color);
                        rectangle2.Margin = new Thickness(8, 0, 430, 0);

                        System.Windows.Shapes.Rectangle rectangle3 = new System.Windows.Shapes.Rectangle();
                        rectangle3.Fill = new SolidColorBrush(color2);
                        rectangle3.Margin = new Thickness(581, 0, 0, 0);

                        Label label = new Label();
                        label.Content = jObject["Name"].ToString();
                        label.HorizontalAlignment = HorizontalAlignment.Left;
                        label.Margin = new Thickness(160, 0, 0, 0);
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.FontSize = 16;

                        RadioButton radioButton = new RadioButton();
                        radioButton.Margin = new Thickness(558, 0, 7, 0);
                        radioButton.GroupName = "A";
                        radioButton.Tag = string.Format("{0};{1};{2}", jObject["PrimaryHueMidBrush"].ToString(), jObject["PrimaryHueLightBrush"].ToString(), jObject["PrimaryHueDarkBrush"].ToString());
                        radioButton.Checked += RadioButton_Checked;
                        if (Settings.PrimaryHueMidBrush == jObject["PrimaryHueMidBrush"].ToString())
                            radioButton.IsChecked = true;

                        Grid grid = new Grid();
                        grid.Children.Add(rectangle1);
                        grid.Children.Add(rectangle2);
                        grid.Children.Add(rectangle3);
                        grid.Children.Add(label);
                        grid.Children.Add(radioButton);

                        border.Child = grid;

                        Themes.Children.Add(border);
                    }
                }
                catch { }
            }

            #region Set1
            if (Settings.IsFullScreen)
                ToggleButton.IsChecked = true;
            if (Settings.Demo)
                ToggleButton1.IsChecked = true;
            if (Settings.AutoHideLaucher)
                ToggleButton2.IsChecked = true;
            if (Settings.AutoMemory)
                ToggleButton3.IsChecked = true;

            path.Text = Settings.GamePath;
            R1.IsChecked = !(bool)Json.ReadPath(Settings.GamePathName, "RelativePath");
            R2.IsChecked = (bool)Json.ReadPath(Settings.GamePathName, "RelativePath");
            TextBox_JavaPath.Text = Settings.JavaPath;
            TextBox_Memory.Text = Settings.Memory.ToString();
            TextBox_Width.Text = (string)Json.Read("Video", "Width");
            TextBox_Height.Text = (string)Json.Read("Video", "Height");
            TextBox_ServerIP.Text = Settings.ServerIP;
            TextBox_JVM_Value.Text = Settings.Value;
            TextBox_Width.IsEnabled = !(bool)ToggleButton.IsChecked;
            #endregion
            #region Set2
            if (Settings.DownloadSource == XL.Core.Tools.DownloadSource.Mojang)
                C1.SelectedIndex = 0;
            else if (Settings.DownloadSource == XL.Core.Tools.DownloadSource.BMCPAPI)
                C1.SelectedIndex = 1;
            else if (Settings.DownloadSource == XL.Core.Tools.DownloadSource.Mcbbs)
                C1.SelectedIndex = 2;
            else
                C1.SelectedIndex = 0;
            if (Settings.CompleteResource)
                ToggleButton4.IsChecked = true;
            else ToggleButton4.IsChecked = false;
            #endregion
            #region Set3
            if (Settings.AcrylicCard)
                ToggleButton5.IsChecked = true;
            else ToggleButton5.IsChecked = false;
            TextBox_BackGround.Text = (string)Json.Read("Individualization", "Background");
            #endregion
            #region Set5
            MySQL_Loaded();
            #endregion
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            #region Set1
            Json.Write("Video", "IsFullScreen", (bool)ToggleButton.IsChecked);
            Json.Write("Game", "Demo", (bool)ToggleButton1.IsChecked);
            Json.Write("Individualization", "AutoHideLaucher", (bool)ToggleButton2.IsChecked);
            Json.Write("JVM", "AutoMemory", (bool)ToggleButton3.IsChecked);
            Json.Write("Files", "UseDefaultDirectory", (bool)R1.IsChecked);

            Json.Write("Files", "JavaPath", TextBox_JavaPath.Text);
            Json.Write("JVM", "Memory", TextBox_Memory.Text);
            Json.Write("Video", "Width", TextBox_Width.Text);
            Json.Write("Video", "Height", TextBox_Height.Text);
            Json.Write("Game", "ServerIP", TextBox_ServerIP.Text);
            Json.Write("JVM", "Value", TextBox_JVM_Value.Text);
            #endregion
            #region Set2
            Json.Write("Files", "DownloadSource", C1.Text);
            Json.Write("Files", "CompleteResource", (bool)ToggleButton4.IsChecked);
            #endregion
            #region Set3
            Json.Write("Individualization", "AcrylicCard", (bool)ToggleButton5.IsChecked);
            Json.Write("Individualization", "Background", TextBox_BackGround.Text);
            #endregion
        }
        private void ListBoxItem_PreviewMouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            Set1.Visibility = Visibility.Visible;
            Set2.Visibility = Set3.Visibility = Set4.Visibility = Set5.Visibility = Visibility.Collapsed;
        }
        private void ListBoxItem_PreviewMouseLeftButton_1(object sender, MouseButtonEventArgs e)
        {
            Set2.Visibility = Visibility.Visible;
            Set1.Visibility = Set3.Visibility = Set4.Visibility = Set5.Visibility = Visibility.Collapsed;
        }
        private void ListBoxItem_PreviewMouseLeftButton_2(object sender, MouseButtonEventArgs e)
        {
            Set5.Visibility = Visibility.Visible;
            Set1.Visibility = Set3.Visibility = Set4.Visibility = Set2.Visibility = Visibility.Collapsed;
        }
        private void ListBoxItem_PreviewMouseLeftButton_3(object sender, MouseButtonEventArgs e)
        {
            Set3.Visibility = Visibility.Visible;
            Set2.Visibility = Set1.Visibility = Set4.Visibility = Set5.Visibility = Visibility.Collapsed;
        }
        private void ListBoxItem_PreviewMouseLeftButton_4(object sender, MouseButtonEventArgs e)
        {
            Set4.Visibility = Visibility.Visible;
            Set2.Visibility = Set1.Visibility = Set3.Visibility = Set5.Visibility = Visibility.Collapsed;
        }
        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Frame.Content == null)
            {
                Frame.Visibility = Visibility.Collapsed;
                MySQL_Loaded();
            }
            try { Frame.RemoveBackEntry(); } catch { }
            GC.Collect();
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            string[] a = radioButton.Tag.ToString().Split(';');
            App.Themes(a[0], a[1], a[2]);
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
        }
        #region
        private async void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            egg += 1;
            if (egg == 5)
            {
                await MainWindow.ShowTip("xuan2006：冒泡！沉！\r\n(日常冒泡...)", 2);
                await MainWindow.ShowTip("xingxing520：快去更新！\r\n(还有不去更新,又有bug!)",2);
                await MainWindow.ShowTip("xuan2006：qwq~\r\n(不存在的,咕咕咕)", 2);
                await MainWindow.ShowTip("gxh2004：吃瓜...\r\n(emmmmmmm)", 2);
                egg = 0;
            }
        }
        #endregion

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Width.IsEnabled = !(bool)ToggleButton.IsChecked;
        }

        private void ToggleButton3_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Memory.IsEnabled = !(bool)ToggleButton3.IsChecked;
        }
    }
}
