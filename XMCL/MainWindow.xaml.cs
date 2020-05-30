using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using XMCL.Core;
using XMCL.Pages;
using System.Net;

namespace XMCL
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static Window Window;
        static Snackbar Snackbar;
        Timer timer;
        
        #region 图形/控件
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
            Snackbar = snackbar;
            string[] a = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
            Text_Title.Text += " " + a[0] + "." + a[1] + a[2] + a[3];
            Frame.Visibility = Visibility.Collapsed;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); } catch { }
        }
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private void Button_Mini(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            GC.Collect();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Update
            Task.Run(() =>
            {
                WebClient webClient = new WebClient();
                string[] a;
                if (App.version.Contains("Pre"))
                    a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/debug.html")).Split('#');
                else a = Encoding.UTF8.GetString(webClient.DownloadData("http://api.axing6.cn/api.html")).Split('#');
                if (a[0] != App.version)
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Frame.Visibility = Visibility.Visible;
                        Frame.Navigate(new Page4());
                    }));
                webClient.Dispose();
            });
            #endregion
            #region JSON
            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\XMCL.json"))
            { }
            else
            {
                FileStream fs1 = new FileStream(System.Environment.CurrentDirectory + "\\XMCL.json", FileMode.Create, FileAccess.ReadWrite);
                try
                {
                    fs1.Write(Properties.Resources.XMCL, 0, Properties.Resources.XMCL.Length);
                    fs1.Flush();
                    fs1.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (Json.Read("Files", "JavaPath").Length > 0)
            { }
            else
            {
                List<string> vs = Tools.GetJavaList();
                if (vs != null)
                    Json.Write("Files", "JavaPath", vs[0]);
            }
            try
            {
                if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                    C1.ItemsSource = Tools.GetVersions(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");
                else
                {
                    if (Json.Read("Game", "LatestVerison").Length > 0)
                        if (Json.Read("Game", "LatestVerison").Contains(" "))
                        {
                            string[] vs = Json.Read("Game", "LatestVerison").Split(' ');
                            if (vs[0] == Json.Read("Files", "GamePath"))
                            {
                                C1.ItemsSource = Tools.GetVersions(Json.Read("Files", "GamePath"));
                            }
                        }
                }
            }
            catch { ShowTip("未能正确获取版本列表,请检查设置", 1); }
            #endregion
            #region Login/Iamge
            login();
            Image_Change();
            #endregion
            #region Timer
            Task.Run(() =>
            {
                Tools.Performance();
                timer = new Timer();
                timer.Enabled = true;
                timer.Interval = 1500;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            });
            #endregion
            GC.Collect();
        }
        public static void ShowTip(string text, int second) => Snackbar.MessageQueue.Enqueue(text, null, null, null, false, false, TimeSpan.FromSeconds(second));
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            Laucher game = new Laucher();
            string GamePath;
            if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                GamePath = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");
            else GamePath = Json.Read("Files", "GamePath");
            Value.DownloadSource = Json.Read("Files", "DownloadSource");
            Value.ServerIP = Json.Read("Game", "ServerIP");
            Value.IsDemo = Convert.ToBoolean(Json.Read("Game", "Demo"));
            Value.AutoMemory = Convert.ToBoolean(Json.Read("JVM", "AutoMemory"));
            string uuid = Json.Read("Login", "choose");
            game.Set(Json.ReadUser(uuid, "userName"), Json.Read("JVM", "Memory"), GamePath, Json.Read("Files", "JavaPath"), C1.Text, Json.Read("JVM", "Value"), uuid, Json.ReadUser(uuid, "accessToken"), Convert.ToBoolean(Json.Read("Video", "IsFullScreen")), Convert.ToBoolean(Json.Read("Files", "CompleteResource")));
            game.Owner = this;
            if (Json.ReadUser(uuid, "LoginMode") == "正版")
                game.VerificationToken = true;
            game.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            game.ShowDialog();
            if (game.process == null)
            { }
            else
            {
                game.process.Exited += Process_Exited;
                button.IsEnabled = false;
                button.Content = "游戏已启动";
            }
            Task.Delay(1000);
            try
            {
                if (game.process.HasExited)
                { }
                else
                {
                    if (Convert.ToBoolean(Json.Read("Individualization", "AutoHideLaucher")))
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Visibility = Visibility.Hidden;
                            timer.Stop();
                        }));
                }
            }
            catch { }
            Json.Write("Game", "LatestVerison", Json.Read("Files", "GamePath") + " " + C1.Text);
        }
        private void C1_DropDownClosed(object sender, EventArgs e)
        {
            System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
            C1.Background = new System.Windows.Media.SolidColorBrush(color);
            System.Windows.Media.Color color1 = System.Windows.Media.Color.FromRgb(255, 255, 255);
            C1.Foreground = new System.Windows.Media.SolidColorBrush(color1);
        }
        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            System.Windows.Media.Color color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            C1.Background = new System.Windows.Media.SolidColorBrush(color);
            System.Windows.Media.Color color1 = System.Windows.Media.Color.FromRgb(0, 0, 0);
            C1.Foreground = new System.Windows.Media.SolidColorBrush(color1);
            if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                C1.ItemsSource = Tools.GetVersions(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");
            else C1.ItemsSource = Tools.GetVersions(Json.Read("Files", "GamePath"));
        }
        private void RadioButton1_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Visibility == Visibility.Visible)
            {
                RadioButton1.IsChecked = false;
                Frame.Navigate(null);
            }
            else
            {
                Frame.Visibility = Visibility.Visible;
                Frame.Navigate(new Page1());
            }
        }
        private void RadioButton2_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Visibility == Visibility.Visible)
            {
                RadioButton2.IsChecked = false;
                Frame.Navigate(null);
            }
            else
            {
                Frame.Visibility = Visibility.Visible;
                Frame.Navigate(new Page2());
            }
        }
        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Frame.Content == null)
                Frame.Visibility = Visibility.Collapsed;
            try { Frame.RemoveBackEntry(); } catch { }
            GC.Collect();
        }
        private void Label_Logined_MouseEnter(object sender, MouseEventArgs e)
        {
            Label_Logined.ToolTip = Label_Logined.Content;
            if (Json.ReadUser(Json.Read("Login", "choose"), "LoginMode") == "正版")
                if (Label_Logined.Content.ToString() == "离线登录")
                    Label_Logined.ToolTip = "重新登录";
        }
        private void Label_Logined_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Json.ReadUser(Json.Read("Login", "choose"), "LoginMode") == "正版")
                if (Label_Logined.Content.ToString() == "离线登录")
                {
                    Window1.Window = this;
                    Window1.login();
                    login();
                }
        }
        private void Label_Name2_MouseEnter(object sender, MouseEventArgs e)
        {
            Label_Name2.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215));
        }
        private void Label_Name2_MouseLeave(object sender, MouseEventArgs e)
        {
            Label_Name2.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(225, 0, 0, 0));
        }
        private void Label_Name2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.ShowDialog();
        }
        #endregion
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            float cpu = Tools.CPU_Utilization();
            float allram = Tools.GetTotalPhysicalMemory();
            float available = Tools.GetAvailablePhysicalMemory();
            float ram = (available / allram);
            string a = ((allram - available) / 1024 / 1024).ToString();
            string b = (allram / 1024 / 1024).ToString();
            string c = cpu.ToString();
            StringBuilder stringBuilder = new StringBuilder();
            if (a.Contains("."))
            {
                string[] a1 = a.Split('.');
                stringBuilder.Append(a1[0] + "." + a1[1].Substring(0, 1) + "MB/");
            }
            else stringBuilder.Append(a + "MB/");
            if (b.Contains("."))
            {
                string[] b1 = b.Split('.');
                stringBuilder.Append(b1[0] + "." + b1[1].Substring(0, 1) + "MB");
            }
            else stringBuilder.Append(b + "MB");

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                CPU_PB.Value = cpu;
                RAM_PB.Value = (1 - ram);
                RAM_.Content = stringBuilder.ToString();
                if (c.Contains("."))
                {
                    string[] c1 = c.Split('.');
                    CPU_.Content = c1[0] + "." + c1[1].Substring(0, 1) + "%";
                }
                else CPU_.Content = c + "%";
            }));
        }
        public void login()
        {
            Load.Visibility = Visibility.Visible;
            head1.Source = BitmapToBitmapImage(Properties.Resources.steve);
            string x = Json.Read("Login", "choose");
            if (Json.Read("Login","choose").Length > 0)
            {
                Label_Name2.Content = Json.ReadUser(x, "userName");
                Task.Run(() =>
                {
                    if (Json.ReadUser(x,"LoginMode") == "正版")
                    {
                        Tools.GetSkins(x, App.Folder_XMCL);
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            head1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(x, "userName") + "\\head1.png"));
                            head2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(x, "userName") + "\\head2.png"));
                        }));
                    }
                    if (Authenticate.validate(Json.ReadUser(x, "accessToken")))
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Label_Name2.Content = Json.ReadUser(x, "userName");
                            Label_Logined.Content = "正版登录";
                            Load.Visibility = Visibility.Collapsed;
                        }));
                    }
                    else
                    {
                        if (Authenticate.Refresh(Json.ReadUser(x, "accessToken"),Json.Read("Login", "clientToken")))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Label_Name2.Content = Json.ReadUser(x, "userName");
                                Label_Logined.Content = "正版登录";
                                Load.Visibility = Visibility.Collapsed;
                            }));
                        }
                        else
                        {
                            bool a = false;
                            if (Json.ReadUser(x, "uuid").Length > 0)
                                if (Json.ReadUser(x, "userName").Length > 0)
                                    if (Json.ReadUser(x, "accessToken").Length > 0)
                                        a = true;
                            if (a == false)
                            {
                                System.Windows.MessageBox.Show("当前账户不可用,已删除");
                                Json.ReMoveUsers(x);
                                Json.Write("Login", "choose", "");
                            }
                            else
                            {
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    Label_Logined.Content = "离线登录";
                                    Load.Visibility = Visibility.Collapsed;
                                }));
                            }
                        }
                    }
                });
            }
            else
            {
                Window1.Window = this;
                Window1.login();
                login();
            }
            System.GC.Collect();
        }
        BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }
        private static int intSuijishu(int a, int b)
        {
            Random r = new Random(GetRandomSeed());
            int i = r.Next(a, b + 1);
            return i;
        }
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        private void Process_Exited(object sender, EventArgs e)
        {
            timer.Start();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                button.IsEnabled = true;
                button.Content = "启动游戏";
                if (this.Visibility == Visibility.Hidden)
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }));
        }
        void Image_Change()
        {
            try
            {
                string[] a = null;
                string[] b = null;
                int suiji = intSuijishu(1, 6);
                Task.Run(() =>
                {
                    WebClient webClient = new WebClient();
                    a = Encoding.UTF8.GetString(webClient.DownloadData("http://106.14.64.250/api/" + suiji + ".html")).Split('#');
                    webClient.Dispose();
                    b = a[1].Split('|');
                    System.Net.WebRequest webreq = System.Net.WebRequest.Create("http://106.14.64.250/api/" + b[1]);
                    System.Net.WebResponse webres = webreq.GetResponse();
                    System.IO.Stream stream = webres.GetResponseStream();
                    System.Drawing.Image img1 = System.Drawing.Image.FromStream(stream);
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img1);
                    IntPtr hBitmap = bmp.GetHbitmap();
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ImageLabel1.Content = a[0];
                        ImageLabel.Text = b[0];
                        Image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }));
                    stream.Dispose();
                    webreq.Abort();
                    webres.Dispose();
                    img1.Dispose();
                    bmp.Dispose();
                    GC.Collect();
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Image_Loading.Visibility = Visibility.Collapsed;
                    }));
                });
            }
            catch
            {
                Image_Loading.Visibility = Visibility.Collapsed;
            }
        }

    }
}