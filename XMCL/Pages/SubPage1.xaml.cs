using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using XL.Core.Tools;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage1.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage1 : Page
    {
        List<Thread> Threads = new List<Thread>();
        public static Page Page;
        public SubPage1()
        {
            InitializeComponent();
            Page = this;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));

            load();
        }
        private void ShowSnapshot_Click(object sender, RoutedEventArgs e)
        {
            load();
        }
        string version;string kind;
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

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mod.Children.Clear();
            kind = ((ListBoxItem)sender).Tag.ToString();
            S3.Visibility = Visibility.Visible; S2.Visibility = Visibility.Collapsed;
            Thread thread;
            Threads.Add(thread = new Thread(() =>
            {
                List<JObject> jObjects = SomethingUseful.GetVersionsMod(kind,version);
                foreach(JObject jObject in jObjects)
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
                            label.Content = jObject["info"]["version"].ToString(); listBoxItem.Tag = jObject["info"]["build"].ToString();
                        }
                        else if (kind == "optifine")
                        {
                            image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Furnace.png", UriKind.Relative));
                            listBoxItem.Tag = (string)jObject["info"]["type"] + "_" + (string)jObject["info"]["patch"];
                            label.Content = new TextBlock() { Text = (string)jObject["info"]["type"] + "_" + (string)jObject["info"]["patch"] };
                        }
                        /*else if (kind == "liteloader")
                        {
                            image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Chicken.png", UriKind.Relative));
                            label.Content = listBoxItem.Tag = version;
                        }*/
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
}
