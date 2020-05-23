using System.Windows.Controls;
using System.Windows.Input;

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
            //SubPage2.choose("原版", Frame);
        }
    }
}
