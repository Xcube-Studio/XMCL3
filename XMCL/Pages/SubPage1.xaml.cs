﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XMCL.Core;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage1.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage1 : Page
    {
        public SubPage1()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
                    string[] a = Tools.GetLatestVersion(Json.Read("Files", "DownloadSource")).Split(';');
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ListBoxItem listBoxItem = new ListBoxItem();
                        ListBoxItem listBoxItem1 = new ListBoxItem();
                        listBoxItem.Margin = listBoxItem1.Margin = new Thickness(20, 0, 20, 0);
                        listBoxItem.Content = a[0].Split(',')[0];
                        listBoxItem.Tag = a[0].Split(',')[1];
                        Latest.Children.Add(listBoxItem);
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
                    List<string> vs = Tools.GetVersionsListAll(Json.Read("Files", "DownloadSource"));
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
    }
}