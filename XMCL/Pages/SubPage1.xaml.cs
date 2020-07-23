using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using XL.Core;
using XL.Core.Tools;
using XL.Core.LaunchCore;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using System.Net;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage1.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage1 : Page
    {
        ListBoxItem[] listBoxItems = new ListBoxItem[0];
        List<Thread> Threads = new List<Thread>();
        public static Page Page;
        public SubPage1()
        {
            InitializeComponent();
            ThreadPool.SetMinThreads(100, 100); ThreadPool.SetMaxThreads(100, 100);
            ServicePointManager.DefaultConnectionLimit = 100;
            listBoxItems = new ListBoxItem[] { Minecraft, Forge, Optifine, LiteLoader };
            Page = this;
            this.Minecraft.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_MouseLeftButtonDown), true);
            this.Forge.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_MouseLeftButtonDown), true);
            this.Fabric.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_MouseLeftButtonDown), true);
            this.Optifine.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_MouseLeftButtonDown), true);
            this.LiteLoader.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_MouseLeftButtonDown), true);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
            Fabric.IsEnabled = false;
            load();
        }
        private void ShowSnapshot_Click(object sender, RoutedEventArgs e)
        {
            load();
        }
        string version;string kind;JObject downloadinfo;
        void load()
        {
            foreach (Thread thread in Threads.ToArray())
                try { thread.Abort(); } catch { }
            Threads.Clear();
            try
            {
                Threads.Add(new Thread(() =>
                {
                    string[] a = SomethingUseful.GetLatestVersion(Settings.DownloadSource).Split(';');
                    ListBoxItem listBoxItem; ListBoxItem listBoxItem1;
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        pb2.Visibility = pb1.Visibility = Visibility.Visible;
                        Latest.Children.Clear();
                        All.Children.Clear();
                        listBoxItem = new ListBoxItem();
                        listBoxItem1 = new ListBoxItem();
                        listBoxItem.Margin = listBoxItem1.Margin = new Thickness(20, 0, 20, 0);
                        listBoxItem.Content = a[0].Split(',')[0];
                        listBoxItem.Tag = a[0].Split(',')[1];
                        Latest.Children.Add(listBoxItem);
                        listBoxItem.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown1; 
                        listBoxItem1.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown1;
                        if (ShowSnapshot.IsChecked == true)
                        {
                            listBoxItem1.Content = a[1].Split(',')[0];
                            listBoxItem1.Tag = a[1].Split(',')[1];
                            Latest.Children.Add(listBoxItem1);
                        }
                        pb1.Visibility = Visibility.Collapsed;
                    }));
                }));
                Threads.Add(new Thread(() =>
                {
                    List<string> vs = SomethingUseful.GetVersionsListAll(Settings.DownloadSource);
                    ListBoxItem list;
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        for (int i = 0; i < vs.Count; i++)
                        {
                            string a = vs[i].Split(',')[0]; string b = vs[i].Split(',')[2]; string c = vs[i].Split(',')[1];
                            list = new ListBoxItem();

                            list.Margin = new Thickness(20, 0, 20, 0);
                            list.Content = a;
                            list.Tag = b;
                            list.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown1;
                            if (ShowSnapshot.IsChecked == true)
                                All.Children.Add(list);
                            else
                            {
                                if (c == "release")
                                    All.Children.Add(list);
                            }
                        }
                        pb2.Visibility = Visibility.Collapsed;
                    }));
                }));
                foreach (Thread thread in Threads.ToArray())
                    thread.Start();
            }
            catch
            {
                MainWindow.ShowTip("从远程服务器拉取版本失败,请重试", 1);
            }
        }
        private void ListBoxItem_PreviewMouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)sender;
            version = listBoxItem.Content.ToString();
            S1.Visibility = Visibility.Collapsed;
            S2.Visibility = Visibility.Visible;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            if (S1.Visibility == Visibility.Visible)
                try { this.NavigationService.Navigate(null); version = null; kind = null; } catch { }
            if (S2.Visibility == Visibility.Visible)
                { S1.Visibility = Visibility.Visible; S2.Visibility = Visibility.Collapsed; S3.Visibility = Visibility.Collapsed; }
            if (S3.Visibility == Visibility.Visible)
                { S1.Visibility = Visibility.Collapsed; S2.Visibility = Visibility.Visible; S3.Visibility = Visibility.Collapsed; }
        }
        private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            kind = ((ListBoxItem)sender).Tag.ToString();
            if (((ListBoxItem)sender).Tag.ToString() == "minecraft")
            {
                ListItemEnable(((ListBoxItem)sender));
            }
            else
            {
                mod.Children.Clear();
                S3.Visibility = Visibility.Visible; S2.Visibility = Visibility.Collapsed;
                Thread thread;
                Threads.Add(thread = new Thread(() =>
                {
                    List<JObject> jObjects = SomethingUseful.GetVersionsMod(kind, version);
                    foreach (JObject jObject in jObjects)
                    {
                        Card card;
                        ListBoxItem listBoxItem;
                        StackPanel stackPanel;
                        Label label;
                        Image image;
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            card = new Card();
                            card.Height = 48;
                            card.VerticalAlignment = VerticalAlignment.Top;
                            card.Margin = new Thickness(0, 10, 0, 0);

                            listBoxItem = new ListBoxItem();
                            listBoxItem.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown;

                            stackPanel = new StackPanel();
                            stackPanel.Orientation = Orientation.Horizontal;

                            label = new Label();
                            label.VerticalContentAlignment = VerticalAlignment.Center;
                            label.HorizontalAlignment = HorizontalAlignment.Left;
                            label.Margin = new Thickness(10, 0, 0, 0);

                            image = new Image();
                            image.SnapsToDevicePixels = true;
                            image.UseLayoutRounding = true;
                            image.Width = image.Height = 32;
                            image.HorizontalAlignment = HorizontalAlignment.Left;
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

                            if (kind == "forge")
                            {
                                image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Anvil.png", UriKind.Relative));
                                label.Content = jObject["info"]["version"].ToString(); listBoxItem.Tag = jObject;
                            }
                            else if (kind == "optifine")
                            {
                                image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Furnace.png", UriKind.Relative));
                                listBoxItem.Tag = jObject;
                                label.Content = new TextBlock() { Text = (string)jObject["info"]["type"] + "_" + (string)jObject["info"]["patch"] };
                            }
                            else if (kind == "liteloader")
                            {
                                image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Chicken.png", UriKind.Relative));
                                label.Content = jObject["info"]["version"]; listBoxItem.Tag = jObject;
                            }
                            stackPanel.Children.Add(image);
                            stackPanel.Children.Add(label);
                            listBoxItem.Content = stackPanel;
                            card.Content = listBoxItem;
                            mod.Children.Add(card);
                            pb2.Visibility = Visibility.Collapsed;
                        }));
                    }
                })); thread.Start();
            }
        }
        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            S3.Visibility = Visibility.Collapsed; S2.Visibility = Visibility.Visible;
            foreach (ListBoxItem listBoxItem1 in listBoxItems)
                if (listBoxItem1.Tag.ToString() == kind)
                    ListItemEnable(listBoxItem1);
            downloadinfo = (JObject)((ListBoxItem)sender).Tag;
        }
        Button button;
        void ListItemEnable(ListBoxItem listBoxItem)
        {
            foreach (ListBoxItem listBoxItem1 in listBoxItems)
                if (listBoxItem1 != listBoxItem)
                    listBoxItem1.IsEnabled = false;
            Grid grid = (Grid)listBoxItem.Parent;
            if (button == null)
            {
                button = new Button();
                button.Content = "取消";
                button.Style = (Style)this.FindResource("MaterialDesignOutlinedButton");
                button.Width = 68;
                button.Margin = new Thickness(0, 0, 5, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.Click += Button_Click;
                grid.Children.Add(button);
                setup.IsEnabled = true;
            }
            else if (!grid.Children.Contains(button))
            {
                button = new Button();
                button.Content = "取消";
                button.Style = (Style)this.FindResource("MaterialDesignOutlinedButton");
                button.Width = 68;
                button.Margin = new Thickness(0, 0, 5, 0);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.Click += Button_Click;
                grid.Children.Add(button);
                setup.IsEnabled = true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            kind = null; downloadinfo = null;
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            grid.Children.Remove(button);
            foreach (ListBoxItem listBoxItem1 in listBoxItems)
                listBoxItem1.IsEnabled = true;
            setup.IsEnabled = false;
        }
        private void Setup_Click(object sender, RoutedEventArgs e)
        {
            S1.Visibility = S2.Visibility = S3.Visibility = GridBottom.Visibility = Visibility.Collapsed;
            setupGrid.Visibility = Visibility.Visible;
        }
        private async void SetupGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (setupGrid.Visibility == Visibility)
            {
                if (kind != "minecraft")
                    model.Content = "安装" + kind;
                else Second.Visibility = Visibility.Collapsed;
                TaskFactory taskFactory = new TaskFactory();
                string filename = string.Format(Settings.GamePath + "\\versions\\{0}\\{1}.json", version, version);
                string replaceurl = "";
                if (Settings.DownloadSource == DownloadSource.BMCPAPI)
                    replaceurl = "https://bmclapi2.bangbang93.com/";
                else if (Settings.DownloadSource == DownloadSource.Mcbbs)
                    replaceurl = "https://download.mcbbs.net/";
                await taskFactory.StartNew(() =>
                {
                    if (!Directory.Exists(string.Format(Settings.GamePath + "\\versions\\{0}\\", version)))
                        Directory.CreateDirectory(string.Format(Settings.GamePath + "\\versions\\{0}\\", version));
                    if (!File.Exists(filename))
                    {
                        ProgressBar progressBar = null; Label label = null; Grid grid = null;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            progressBar = new ProgressBar();
                            progressBar.IsIndeterminate = true;
                            progressBar.Height = 4;
                            progressBar.VerticalAlignment = VerticalAlignment.Bottom;

                            label = new Label();
                            label.Margin = new Thickness(0, 0, 0, 4);
                            label.FontFamily = new FontFamily("宋体");
                            label.Content = "连接中...";

                            grid = new Grid();
                            grid.Margin = new Thickness(0, 0, 0, 2);

                            grid.Children.Add(progressBar);
                            grid.Children.Add(label);
                            StackPanel.Children.Add(grid);
                        }));
                        DownloadFile(SomethingUseful.JsonUrl(version, Settings.DownloadSource), filename, progressBar, label, grid, System.IO.Path.GetFileName(filename));
                    }
                });
                JsonInfo jsonInfo = new JsonInfo(filename);
                Launcher.JsonInfo = jsonInfo;
                LaunchInfo launchInfo = new LaunchInfo();
                launchInfo.Game.Selected_Version = version;
                launchInfo.Game.ComplementaryResources = true;
                launchInfo.Game.GamePath = Settings.GamePath;
                Launcher.LaunchInfo = launchInfo;
                await taskFactory.StartNew(() =>
                {
                    string assetsFile = String.Format(Settings.GamePath + "\\assets\\indexes\\{0}.json", jsonInfo.Assets);
                    string assetsDir = String.Format(Settings.GamePath + "\\assets\\indexes\\");
                    if (!Directory.Exists(assetsDir))
                        Directory.CreateDirectory(assetsDir);
                    ProgressBar progressBar = null; Label label = null; Grid grid = null;
                    string url = jsonInfo.AssetIndex["url"].ToString();
                    if (Settings.DownloadSource != DownloadSource.Mojang)
                        url = url.Replace("https://launchermeta.mojang.com/", replaceurl);
                    bool need = false;
                    if (!File.Exists(assetsFile))
                        need = true;
                    else if (new FileInfo(assetsFile).Length != (int)jsonInfo.AssetIndex["size"])
                        need = true;
                    if (need)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            progressBar = new ProgressBar();
                            progressBar.IsIndeterminate = true;
                            progressBar.Height = 4;
                            progressBar.VerticalAlignment = VerticalAlignment.Bottom;

                            label = new Label();
                            label.Margin = new Thickness(0, 0, 0, 4);
                            label.FontFamily = new FontFamily("宋体");
                            label.Content = "连接中...";

                            grid = new Grid();
                            grid.Margin = new Thickness(0, 0, 0, 2);

                            grid.Children.Add(progressBar);
                            grid.Children.Add(label);
                            StackPanel.Children.Add(grid);
                        }));
                        DownloadFile(url, assetsFile, progressBar, label, grid, System.IO.Path.GetFileName(assetsFile));
                    }
                });

                List<DownloadFile> downloadFiles = new List<DownloadFile>();
                await taskFactory.StartNew(() =>
                {
                    downloadFiles = new LaunchFileVerifier().Verifier(Settings.DownloadSource, Libraries.Get(filename));
                });
                List<Task> tasks = new List<Task>();
                foreach (DownloadFile downloadFile in downloadFiles)
                    tasks.Add(taskFactory.StartNew(() =>
                    {
                        ProgressBar progressBar = null; Label label = null; Grid grid = null;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            progressBar = new ProgressBar();
                            progressBar.IsIndeterminate = true;
                            progressBar.Height = 4;
                            progressBar.VerticalAlignment = VerticalAlignment.Bottom;

                            label = new Label();
                            label.Margin = new Thickness(0, 0, 0, 4);
                            label.FontFamily = new FontFamily("宋体");
                            label.Content = "连接中...";

                            grid = new Grid();
                            grid.Margin = new Thickness(0, 0, 0, 2);

                            grid.Children.Add(progressBar);
                            grid.Children.Add(label);
                            StackPanel.Children.Add(grid);
                        }));
                        DownloadFile(downloadFile.Url, downloadFile.Path, progressBar, label, grid, downloadFile.Name);
                    }));

                await Task.WhenAll(tasks.ToArray());
                this.Dispatcher.Invoke(new Action(() =>
                {
                    StackPanel.Children.Remove(check);
                    icon1.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                    icon2.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowRight;
                }));
                if (kind != "minecraft")
                {
                    string url = replaceurl;
                    if (kind == "forge")
                        url += "forge/download/" + (string)downloadinfo["info"]["build"];
                    else if (kind == "optifine")
                        url += "optifine/" + version + "/" + (string)downloadinfo["info"]["type"] + "/" + (string)downloadinfo["info"]["patch"];
                    else if (kind == "liteloader")
                        url += "/maven/com/mumfrey/liteloader/${version}/liteloader-${version}.jar".Replace("${version}", (string)downloadinfo["info"]["version"]);
                    await taskFactory.StartNew(() =>
                    {
                        Grid grid = null; ProgressBar progressBar = null; Label label = null;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            progressBar = new ProgressBar();
                            progressBar.IsIndeterminate = true;
                            progressBar.Height = 4;
                            progressBar.VerticalAlignment = VerticalAlignment.Bottom;

                            label = new Label();
                            label.Margin = new Thickness(0, 0, 0, 4);
                            label.FontFamily = new FontFamily("宋体");
                            label.Content = "连接中...";

                            grid = new Grid();
                            grid.Margin = new Thickness(0, 0, 0, 2);

                            grid.Children.Add(progressBar);
                            grid.Children.Add(label);
                            StackPanel1.Children.Add(grid);
                        }));
                        DownloadFile(url, Settings.GamePath + "\\" + (string)downloadinfo["FileName"], progressBar, label, grid, (string)downloadinfo["FileName"]);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            StackPanel1.Children.Remove(grid);
                        }));
                    });
                    Grid g = null; ProgressBar p = null; Label l = null;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        p = new ProgressBar();
                        p.IsIndeterminate = true;
                        p.Height = 4;
                        p.VerticalAlignment = VerticalAlignment.Bottom;
                        p.IsIndeterminate = true;

                        l = new Label();
                        l.Margin = new Thickness(0, 0, 0, 4);
                        l.FontFamily = new FontFamily("宋体");
                        l.Content = "运行" + kind;

                        g = new Grid();
                        g.Margin = new Thickness(0, 0, 0, 2);

                        g.Children.Add(p);
                        g.Children.Add(l);
                        StackPanel1.Children.Add(g);
                    }));

                    Process process = new Process();
                    process.StartInfo.FileName = Settings.JavaPath;
                    process.StartInfo.Arguments = "-jar " + Settings.GamePath + "\\" + (string)downloadinfo["FileName"];
                    await taskFactory.StartNew(() =>
                    {
                        process.Start();
                        process.WaitForExit();
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StackPanel1.Children.Remove(g);
                            icon2.Kind = icon3.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                            SetupDone.Visibility = Visibility.Visible;
                        }));
                    });
                }
                else
                {
                    await this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        icon2.Kind = icon3.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                        SetupDone.Visibility = Visibility.Visible;
                    }));
                }
                Launcher.JsonInfo = null;
                Launcher.LaunchInfo = null;
            }
        }
        public void DownloadFile(string URL, string filename, ProgressBar prog, Label label, Grid grid, string text)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Timeout = 30000;
            try
            {
                request.KeepAlive = false;
                request.ServicePoint.Expect100Continue = false;

                request.ServicePoint.UseNagleAlgorithm = false;
                request.AllowWriteStreamBuffering = false;
                request.Proxy = null;
                /*Mystate mystate = new Mystate();
                mystate.FileName = filename; mystate.Label = label; mystate.ProgressBar = prog; mystate.WebRequestObject = Myrq; mystate.Grid = grid;
                mystate.text = text;*/
                this.Dispatcher.Invoke(new Action(() =>
                {
                    prog.IsIndeterminate = false;
                }));
                //Myrq.BeginGetResponse(DownloadFinished, mystate);
                WebResponse response = request.GetResponse();
                long totalBytes = response.ContentLength;
                Stream st = response.GetResponseStream();
                Stream so = File.Create(filename);

                byte[] by = new byte[1024];
                this.Dispatcher.Invoke(new Action(() =>
                {
                    label.Content = text;
                }));
                try
                {
                    float percent = 0;
                    long totalDownloadedByte = 0;
                    int osize = st.Read(by, 0, (int)by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        so.Write(by, 0, osize);
                        osize = st.Read(by, 0, (int)by.Length);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                            prog.Value = percent;
                        }));
                    }
                }
                finally
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        StackPanel.Children.Remove(grid);
                    }));
                    request.Abort(); response.Close();
                    if (so != null) so.Close();
                    if (st != null) st.Close();
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    StackPanel.Children.Remove(grid);
                }));
                Console.WriteLine(ex.Message);
                request.Abort();
            }
        }

        private void SetupDone_Click(object sender, RoutedEventArgs e)
        {
            setupGrid.Visibility = Visibility.Collapsed;
            this.NavigationService.Navigate(new Page1());
        }
    }
}
