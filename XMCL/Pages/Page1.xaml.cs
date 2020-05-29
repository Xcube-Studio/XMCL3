using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System;

namespace XMCL.Pages
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        public static Page Page;
        public Page1()
        {
            InitializeComponent();
            Page = this;
        }

        private void SubPage1(object sender, MouseButtonEventArgs e)
        {
            //Frame.Navigate(new SubPage1());
        }

        private void TreeViewItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //SubPage2.choose("forge", Frame);
        }

        private void TreeViewItem_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            //SubPage2.choose("optifine", Frame);
        }

        private void TreeViewItem_PreviewMouseUp_2(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new SubPage1());
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Frame.Content == null)
                Frame.Visibility = Visibility.Collapsed;
            try { Frame.RemoveBackEntry(); } catch { }
            GC.Collect();
        }
    }
}
