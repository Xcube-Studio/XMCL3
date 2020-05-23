using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace XMCL.Pages
{
    /// <summary>
    /// Set3.xaml 的交互逻辑
    /// </summary>
    public partial class Set3 : Page
    {
        public Set3()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Thems("#2196f3", "#6ec6ff", "#0069c0");
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            App.Thems("#ff8f00", "#ffc406", "#c56000");
        }

    }
}
