using System;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void Set1(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new Set1());
        }
        private void Set5(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new Set5());
        }
        private void Set2(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new Set2());
        }
        private void Set3(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new Set3());
        }
        private void About(object sender, MouseButtonEventArgs e)
        {
            Frame.Navigate(new Set4());
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try { Frame.RemoveBackEntry(); } catch { }
            GC.Collect();
        }
    }
}
