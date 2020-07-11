using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using XL.Core;
using XL.Core.Tools;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage1.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage1 : Page
    {
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
        void load()
        {
            try
            {
                pb2.Visibility = pb1.Visibility = Visibility.Visible;
                Latest.Children.Clear();
                All.Children.Clear();
                Task.Run(() =>
                {
                    string[] a = SomethingUseful.GetLatestVersion(Settings.DownloadSource).Split(';');
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ListBoxItem listBoxItem = new ListBoxItem();
                        ListBoxItem listBoxItem1 = new ListBoxItem();
                        listBoxItem.Margin = listBoxItem1.Margin = new Thickness(20, 0, 20, 0);
                        listBoxItem.Content = a[0].Split(',')[0];
                        listBoxItem.Tag = a[0].Split(',')[1];
                        Latest.Children.Add(listBoxItem);
                        listBoxItem.MouseDoubleClick += ListBoxItem_MouseDoubleClick;
                        listBoxItem1.MouseDoubleClick += ListBoxItem_MouseDoubleClick;
                        if (ShowSnapshot.IsChecked == true)
                        {
                            listBoxItem1.Content = a[1].Split(',')[0];
                            listBoxItem1.Tag = a[1].Split(',')[1];
                            Latest.Children.Add(listBoxItem1);
                        }
                        pb1.Visibility = Visibility.Collapsed;
                    }));
                });
                Task.Run(() =>
                {
                    List<string> vs = SomethingUseful.GetVersionsListAll(Settings.DownloadSource);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        for (int i = 0; i < vs.Count; i++)
                        {
                            string a = vs[i].Split(',')[0]; string b = vs[i].Split(',')[2]; string c = vs[i].Split(',')[1];
                            ListBoxItem list = new ListBoxItem();

                            list.Margin = new Thickness(20, 0, 20, 0);
                            list.Content = a;
                            list.Tag = b;
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
                });//此处会导致CPU占用高 因为代码生成的控件太多 所以最好不要在这个页面停留
            }
            catch
            {
                MainWindow.ShowTip("从远程服务器拉取版本失败,请重试", 1);
            }
        }
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
