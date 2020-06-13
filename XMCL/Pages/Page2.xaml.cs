using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForm = System.Windows.Forms;
using System.Threading.Tasks;
using XMCL.Core;
using MySql.Data.MySqlClient;
using System.IO;

namespace XMCL.Pages
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        public static Page Page;
        public Page2()
        {
            InitializeComponent();
            Page = this;
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            try { this.NavigationService.Navigate(null); } catch { }
        }
        private void R1_Checked(object sender, RoutedEventArgs e)
        {
            TextBox_GamePath.IsEnabled = Button_OpenGame.IsEnabled = false;
        }
        private void R2_Checked(object sender, RoutedEventArgs e)
        {
            TextBox_GamePath.IsEnabled = Button_OpenGame.IsEnabled = true;
        }
        private void Button_OpenGame_Click(object sender, RoutedEventArgs e)
        {
            WinForm.FolderBrowserDialog dialog = new WinForm.FolderBrowserDialog();
            dialog.ShowDialog();
            TextBox_GamePath.Text = dialog.SelectedPath;
            dialog.Dispose();
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
        private void ToggleButton3_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleButton3.IsChecked == true)
                TextBox_Memory.IsEnabled = false;
            else TextBox_Memory.IsEnabled = true;
        }
        private void ToggleButton3_Checked(object sender, RoutedEventArgs e)
        {
            TextBox_Memory.IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Thems("#2196f3", "#6ec6ff", "#0069c0");
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            App.Thems("#ff8f00", "#ffc406", "#c56000");
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region Set1
            if (Convert.ToBoolean(Json.Read("Video", "IsFullScreen")))
                ToggleButton.IsChecked = true;
            if (Convert.ToBoolean(Json.Read("Game", "Demo")))
                ToggleButton1.IsChecked = true;
            if (Convert.ToBoolean(Json.Read("Individualization", "AutoHideLaucher")))
                ToggleButton2.IsChecked = true;
            if (Convert.ToBoolean(Json.Read("JVM", "AutoMemory")))
                ToggleButton3.IsChecked = true;
            if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                R1.IsChecked = true;
            else R2.IsChecked = true;
            TextBox_GamePath.Text = Json.Read("Files", "GamePath");
            TextBox_JavaPath.Text = Json.Read("Files", "JavaPath");
            TextBox_Memory.Text = Json.Read("JVM", "Memory");
            TextBox_Width.Text = Json.Read("Video", "Width");
            TextBox_Height.Text = Json.Read("Video", "Height");
            TextBox_ServerIP.Text = Json.Read("Game", "ServerIP");
            TextBox_JVM_Value.Text = Json.Read("JVM", "Value");
            #endregion
            #region Set2
            if (Json.Read("Files", "DownloadSource") == "Mojang")
                C1.SelectedIndex = 0;
            else if (Json.Read("Files", "DownloadSource") == "BMCLAPI")
                C1.SelectedIndex = 1;
            else if (Json.Read("Files", "DownloadSource") == "McbbsAPI")
                C1.SelectedIndex = 2;
            else
                C1.SelectedIndex = 0;
            if (Convert.ToBoolean(Json.Read("Files", "CompleteResource")))
                ToggleButton4.IsChecked = true;
            else ToggleButton4.IsChecked = false;
            #endregion
            #region Set5
            MySQL_Loaded();
            #endregion
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            #region Set1
            if (ToggleButton.IsChecked == true)
                Json.Write("Video", "IsFullScreen", "true");
            else Json.Write("Video", "IsFullScreen", "false");
            if (ToggleButton1.IsChecked == true)
                Json.Write("Game", "Demo", "true");
            else Json.Write("Game", "Demo", "false");
            if (ToggleButton2.IsChecked == true)
                Json.Write("Individualization", "AutoHideLaucher", "true");
            else Json.Write("Individualization", "AutoHideLaucher", "false");
            if (ToggleButton3.IsChecked == true)
                Json.Write("JVM", "AutoMemory", "true");
            else Json.Write("JVM", "AutoMemory", "false");
            if (R1.IsChecked == true)
                Json.Write("Files", "UseDefaultDirectory", "true");
            if (R2.IsChecked == true)
                Json.Write("Files", "UseDefaultDirectory", "false");
            Json.Write("Files", "GamePath", TextBox_GamePath.Text);
            Json.Write("Files", "JavaPath", TextBox_JavaPath.Text);
            Json.Write("JVM", "Memory", TextBox_Memory.Text);
            Json.Write("Video", "Width", TextBox_Width.Text);
            Json.Write("Video", "Height", TextBox_Height.Text);
            Json.Write("Game", "ServerIP", TextBox_ServerIP.Text);
            Json.Write("JVM", "Value", TextBox_JVM_Value.Text);
            #endregion
            #region Set2
            Json.Write("Files", "DownloadSource", C1.Text);
            if (ToggleButton4.IsChecked == true)
                Json.Write("Files", "CompleteResource", "true");
            else Json.Write("Files", "CompleteResource", "false");
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
    }
}
