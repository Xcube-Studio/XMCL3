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
using System.Windows.Media.Animation;
using SourceChord.FluentWPF;

namespace XMCL
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {
        public static Window Window;
        public static ColorZone ColorZone;
        public static Page DownloadPage;
        public static Card Tip1;
        public static Frame Frame1;
        public static ComboBox ComboBox;
        public static Performance performance;
        public static Timer timer;
        public static Label Label_1;
        public static Label Label_2;
        public static System.Windows.Controls.Image Image1;
        public static System.Windows.Controls.Image Image2;
        public static Label CPU;
        public static Label RAM;
        public static ProgressBar cpu_PB;
        public static ProgressBar ram_PB;

        #region 图形/控件
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
            ColorZone = CtrlPage;
            ComboBox = C1;
            Frame1 = Frame;
            Tip1 = Tip;
            Label_1 = Label_Name;
            Label_2 = Label_Logined;
            Image1 = head1;
            Image2 = head2;
            CPU = CPU_;
            RAM = RAM_;
            cpu_PB = CPU_PB;
            ram_PB = RAM_PB;
            string[] a = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
            Text_Title.Text += " " + a[0] + "." + a[1] + a[2] + a[3];
            Card_Login.ClipToBounds = true;
            Frame.Navigate(new Page4());
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
            #region Iamge
            Image_Change();
            #endregion
            #region Theme
            #region Acrylic
            if (Settings.AcrylicCard)
            {
                Grid[] grids = new Grid[] { Card_Login, G2, G3, G4 };
                Card[] borders = new Card[] { B1, B2, B3, B4 };
                for (int i = 0; i < grids.Length; i++)
                {
                    AcrylicPanel acrylic = new AcrylicPanel();
                    acrylic.NoiseOpacity = 0.02;
                    borders[i].Background = new SolidColorBrush(Colors.Transparent);
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
            if(File.Exists(Settings.Background))
                MainImage.Source = new BitmapImage(new Uri(Settings.Background));
            #endregion
            GC.Collect();
        }
        public static Task ShowTip(string text, int second) => ShowTip(new TextBlock() { Text = text, Margin = new Thickness(10), TextWrapping = TextWrapping.Wrap }, second);
        public static Task ShowTip(UIElement uIElement, int second)
        {
            Tip1.Visibility = Visibility.Visible;
            Tip1.Content = uIElement;
            return Task.Run(async delegate
            {
                Storyboard sb; DoubleAnimation yd5;
                await Window.Dispatcher.BeginInvoke(new Action(() =>
                {
                    sb = new Storyboard();//首先实例化一个故事板
                    yd5 = new DoubleAnimation(Tip1.ActualWidth, -Tip1.ActualWidth, new Duration(TimeSpan.FromSeconds(1)));//浮点动画定义了开始值和起始值
                    Tip1.RenderTransform = new TranslateTransform();//在二维x-y坐标系统内平移(移动)对象
                    yd5.AutoReverse = false;//设置可以进行反转
                    Storyboard.SetTarget(yd5, Tip1);//绑定动画为这个按钮执行的浮点动画
                    Storyboard.SetTargetProperty(yd5, new PropertyPath("RenderTransform.X"));//依赖的属性
                    sb.Children.Add(yd5);//向故事板中加入此浮点动画
                    sb.Begin();//播放此动画
                }));
                await Task.Delay(second * 1000);
                await Window.Dispatcher.BeginInvoke(new Action(() =>
                {
                    sb = new Storyboard();
                    yd5 = new DoubleAnimation(-Tip1.ActualWidth, Tip1.ActualWidth, new Duration(TimeSpan.FromSeconds(1)));//浮点动画定义了开始值和起始值
                    yd5.AutoReverse = false;//设置可以进行反转
                    Storyboard.SetTarget(yd5, Tip1);//绑定动画为这个按钮执行的浮点动画
                    Storyboard.SetTargetProperty(yd5, new PropertyPath("RenderTransform.X"));//依赖的属性
                    sb.Children.Add(yd5);//向故事板中加入此浮点动画
                    sb.Begin();//播放此动画
                }));
                await Task.Delay(second * 1000);
                await Window.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Tip1.Visibility = Visibility.Collapsed;
                }));

            });
        }
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
            settings.DefaultConnectionLimit = 1000;
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
                    SomethingUseful.DelectDir(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName"));
                    Skin.GetSkins(Settings.UUID, App.Folder_XMCL);
                }
                catch { }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    head1.Visibility = head2.Visibility = Visibility.Visible;
                    head_load.Visibility = Visibility.Collapsed;
                    head1.Source = FileToImageSource(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png");
                    head2.Source = FileToImageSource(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png");
                }));
            });
        }
        #endregion
        public static void Timer_Elapsed(object sender, ElapsedEventArgs e)
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

            Window.Dispatcher.BeginInvoke(new Action(() =>
            {
                cpu_PB.Value = cpu;
                ram_PB.Value = (1 - ram);
                RAM.Content = stringBuilder.ToString();
                if (c.Contains("."))
                {
                    string[] c1 = c.Split('.');
                    CPU.Content = c1[0] + "." + c1[1].Substring(0, 1) + "%";
                }
                else CPU.Content = c + "%";
            }));
        }
        ImageSource FileToImageSource(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            byte[] Mybyte = new byte[fs.Length];
            fs.Read(Mybyte, 0, Mybyte.Length);
            fs.Close();
            MemoryStream MyMemoryStream = new MemoryStream(Mybyte);//将byte[]数组转化为内存流
            System.Drawing.Image MyImage = System.Drawing.Image.FromStream(MyMemoryStream);        //将内存流转化为image类型
            System.Drawing.Bitmap MyBitmap = new System.Drawing.Bitmap(MyImage);                   //将image类型转化为bitmap类型
            IntPtr MyIntPtr = MyBitmap.GetHbitmap();
            ImageSource WPFSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(MyIntPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return WPFSource;
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
        public static async void Login()
        {
            if (Json.ReadUsers().Count == 0)
            {
                Window1.login(Window);
                Login();
            }
            Label_1.Content = Json.ReadUser(Settings.UUID, "userName");
            if (Json.ReadUser(Settings.UUID, "LoginMode") == "正版")
            {
                Label_2.Content = "正版验证";
                try
                {
                    Image1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png"));
                    Image2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png"));
                }
                catch
                {
                    await Task.Run(delegate
                    {
                        SomethingUseful.DelectDir(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName"));
                        try { Skin.GetSkins(Settings.UUID, App.Folder_XMCL); } catch { }
                        Window.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                Image1.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head1.png"));
                                Image2.Source = new BitmapImage(new Uri(App.Folder_XMCL + "\\user\\" + Json.ReadUser(Settings.UUID, "userName") + "\\head2.png"));
                            } catch { }
                        }));
                    });
                }
            }
            else
            {
                Label_2.Content = "离线验证";
                Image1.Source = new BitmapImage(new Uri(@"pack://application:,,,/XMCL;component/Resources/steve.png"));
                Image2.Source = null;
            }
        }

    }
}