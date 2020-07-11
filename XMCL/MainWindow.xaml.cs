using MaterialDesignThemes.Wpf;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XL.Core.Tools;
using XL.Core;
using XMCL.Pages;
using SourceChord.FluentWPF;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace XMCL
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {
        public static Window Window;
        static Snackbar Snackbar;
        public static ColorZone ColorZone;
        public static Page DownloadPage;
        Performance performance;
        Timer timer;

        #region 图形/控件
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
            ColorZone = CtrlPage;
            Snackbar = snackbar;
            string[] a = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
            Text_Title.Text += " " + a[0] + "." + a[1] + a[2] + a[3];
            Card_Login.ClipToBounds = true;
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
            this.Activate();
            #region hwid
/*
            if (!File.Exists("C:\\Users\\xmcl主题.txt"))
                {
                
                
                    FileStream fs = new FileStream("C:\\Users\\xmcl主题.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                    StreamWriter sw = new StreamWriter(fs);
                    char a1=(char) intSuijishu(1, 96);
                    char b2 = (char)intSuijishu(1, 96);
                    char c3 = (char)intSuijishu(1, 96);
                    char d4 = (char)intSuijishu(1, 96);
                    char e5 = (char)intSuijishu(1, 96);
                    char f6 = (char)intSuijishu(1, 96);
                    sw.WriteLine(a1.ToString()+b2.ToString()+c3.ToString()+d4.ToString()+e5.ToString()+f6.ToString()); 
                    sw.Close();
                    fs.Close();
                   
                    string ConString = "server=106.14.64.250;User Id=User;password=User20202020server;Database=User";
                    MySqlConnection conn = new MySqlConnection(ConString);//连接数据库 
                    
                    conn.Open();   //open的时候可以套个try防止boom 
                    string hwid = System.IO.File.ReadAllText("C:\\Users\\xmcl主题.txt");
                System.Windows.MessageBox.Show(hwid);
                    string sql = "INSERT INTO `主题` (`hwid`, `主题1`, `主题2`, `主题3`) VALUES ('"+hwid+"', '0', '0', '0');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Close();
                }
                else
                {
                    
                }

  */
            #endregion
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
                        Frame.Navigate(new Page3());
                    }));
                webClient.Dispose();
            });
            #endregion
            #region JSON
            C1.ItemsSource = SomethingUseful.GetVersions(Settings.GamePath);
            if (Settings.LatestVerison.Contains(" "))
            {
                string[] vs = Settings.LatestVerison.Split(' ');
                if (vs[0] == Settings.GamePath)
                {
                    for (int i = 0; i < C1.Items.Count; i++)
                    {
                        if (C1.Items[i].ToString() == vs[1])
                            C1.SelectedIndex = i;
                    }
                }
            }
            #endregion
            #region Login/Iamge
            Login();
            Image_Change();
            #endregion
            #region Timer
            Task.Run(() =>
            {
                performance = new Performance();
                timer = new Timer();
                timer.Enabled = true;
                timer.Interval = 1500;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            });
            #endregion
            #region Theme
            #region Acrylic
            if (Settings.AcrylicCard)
            {
                Grid[] grids = new Grid[] { Card_Login, G2, G3, G4 };
                for (int i = 0; i < grids.Length; i++)
                {
                    AcrylicPanel acrylic = new AcrylicPanel();
                    acrylic.NoiseOpacity = 0.02;
                    acrylic.SetBinding(AcrylicPanel.TargetProperty, new Binding() { ElementName = "MainImage" });
                    grids[i].Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#26FFFFFF"));
                    grids[i].Children.Add(acrylic);
                    Panel.SetZIndex(acrylic, 0);
                    Label_Name.Foreground = Label_Logined.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CCFFFFFF"));
                    for (int i1 = 0; i1 < grids[i].Children.Count; i1++)
                    {
                        if (!(grids[i].Children[i1] == acrylic))
                            Panel.SetZIndex(grids[i].Children[i1], 1);
                    }
                }

            }
            #endregion
            #region Color
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
            #endregion
            #endregion
            GC.Collect();
        }
        public static void ShowTip(string text, int second) => Snackbar.MessageQueue.Enqueue(text, null, null, null, false, false, TimeSpan.FromSeconds(second));
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            LaunchInfo launchInfo = new LaunchInfo();
            launchInfo.Game.ComplementaryResources = true;
            launchInfo.Game.GamePath = Settings.GamePath;
            launchInfo.Game.Selected_Version = C1.Text;
            launchInfo.Game.ServerIP = Settings.ServerIP;
            launchInfo.Game.IsDemo = Settings.Demo;
            launchInfo.Game.IsFullScreen = Settings.IsFullScreen;

            launchInfo.Java.AutoMemory = Settings.AutoMemory;
            launchInfo.Java.Memory = Settings.Memory;
            launchInfo.Java.MoreValue = Settings.Value;
            launchInfo.Java.JavaPath = Settings.JavaPath;

            launchInfo.Player.LoginName = Json.ReadUser(Settings.UUID, "Email");
            launchInfo.Player.AccessToken = Json.ReadUser(Settings.UUID, "accessToken");
            launchInfo.Player.ClientToken = Settings.ClientToken;
            launchInfo.Player.Name = Json.ReadUser(Settings.UUID, "userName");
            launchInfo.Player.UUID = Settings.UUID;
            if (Json.ReadUser(Settings.UUID, "LoginMode") == "正版")
                launchInfo.Player.Mode = Player.Authentication.Validate;
            else launchInfo.Player.Mode = Player.Authentication.Offline;

            XMCLSettings settings = new XMCLSettings();
            settings.DefaultConnectionLimit = 1024;
            settings.DownloadSource = Settings.DownloadSource;
            settings.AfterLaunchAction = new Action(delegate
            {
                Json.AddUsers(launchInfo.Player.Name, launchInfo.Player.UUID, launchInfo.Player.AccessToken, Json.ReadUser(Settings.UUID, "LoginMode"), launchInfo.Player.LoginName);
                Json.Write("Login", "ClientToken", launchInfo.Player.ClientToken);
                Json.Write("Login", "Choose", launchInfo.Player.UUID);
                Json.Write("Game", "LatestVerison", Settings.GamePath + " " + C1.Text);
                if (Launcher.process != null)
                {
                    Launcher.process.Exited += Process_Exited;
                    button.IsEnabled = false;
                    button.Content = "游戏已启动";
                }
                try
                {
                    if (Launcher.process.HasExited)
                    { }
                    else
                    {
                        if (Settings.AutoHideLaucher)
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                this.Visibility = Visibility.Hidden;
                                timer.Stop();
                            }));
                    }
                }
                catch { }
            });

            Launcher launcher = new Launcher(launchInfo,settings);
            launcher.Start(this);
            
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
            C1.ItemsSource = SomethingUseful.GetVersions(Settings.GamePath);
        }
        private void FrameButton1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page1());
        }
        private void FrameButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page2());
        }
        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Frame.Content == null)
            {
                Frame.Visibility = Visibility.Collapsed;
                Main.Visibility = Visibility.Visible;
            }
            else
            {
                Frame.Visibility = Visibility.Visible;
                Main.Visibility = Visibility.Collapsed;
            }
            try { Frame.RemoveBackEntry(); } catch { }
            GC.Collect();
        }
        private void Label_Logined_MouseEnter(object sender, MouseEventArgs e)
        {
            Label_Logined.ToolTip = Label_Logined.Content;
            if (Json.ReadUser(Settings.UUID, "LoginMode") == "正版")
                if (Label_Logined.Content.ToString() == "离线登录")
                    Label_Logined.ToolTip = "重新登录";
        }
        private void Label_Logined_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window2.Show();
            Login();
        }
        private void Label_Name2_MouseEnter(object sender, MouseEventArgs e)
        {
            Label_Name.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 120, 215));
        }
        private void Label_Name2_MouseLeave(object sender, MouseEventArgs e)
        {
            Label_Name.Foreground = Label_Logined.Foreground;
        }
        private void Label_Name2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window2.Show();
            Login();
        }
        private void Head2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Task.Run(delegate
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    head1.Visibility = head2.Visibility = Visibility.Collapsed;
                    head_load.Visibility = Visibility.Visible;
                }));
                try
                {
                    Skin.GetSkins(Settings.UUID, App.Folder_XMCL);
                }
                catch { }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    head1.Visibility = head2.Visibility = Visibility.Visible;
                    head_load.Visibility = Visibility.Collapsed;
                    head1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png"));
                    head2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png"));
                }));
            });
        }
        #endregion
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            float cpu = performance.CPU_Utilization();
            float allram = performance.GetTotalPhysicalMemory();
            float available = performance.GetAvailablePhysicalMemory();
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
        void Login()
        {
            if (Json.ReadUsers().Count == 0)
            {
                Window1.login(this);
                Login();
            }
            Label_Name.Content = Json.ReadUser(Settings.UUID, "userName");
            if (Json.ReadUser(Settings.UUID, "LoginMode") == "正版")
            {
                Label_Logined.Content = "正版验证";
                try
                {
                    head1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png"));
                    head2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png"));
                }
                catch
                {
                    Task.Run(delegate
                    {
                        try { Skin.GetSkins(Settings.UUID, App.Folder_XMCL); } catch { }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            head1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png"));
                            head2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png"));
                        }));
                    });
                }
            }
            else
            {
                Label_Logined.Content = "离线验证";
                head1.Source = new BitmapImage(new Uri(@"pack://application:,,,/XMCL;component/Resources/steve.png"));

            }
        }

    }
}