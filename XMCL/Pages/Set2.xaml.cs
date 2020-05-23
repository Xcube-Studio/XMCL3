using System;
using System.Windows;
using System.Windows.Controls;
using XMCL.Core;

namespace XMCL.Pages
{
    /// <summary>
    /// Set2.xaml 的交互逻辑
    /// </summary>
    public partial class Set2 : Page
    {
        public static Page Page;
        public Set2()
        {
            InitializeComponent();
            Page = this;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Json.Read("Files", "DownloadSource") == "Mojang")
                C1.SelectedIndex = 0;
            else if (Json.Read("Files", "DownloadSource") == "BMCLAPI")
                C1.SelectedIndex = 1;
            else if (Json.Read("Files", "DownloadSource") == "McbbsAPI")
                C1.SelectedIndex = 2;
            else
                C1.SelectedIndex = 0;
            if (Convert.ToBoolean(Json.Read("Files", "CompleteResource")))
                ToggleButton.IsChecked = true;
            else ToggleButton.IsChecked = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Json.Write("Files", "DownloadSource", C1.Text);
            if (ToggleButton.IsChecked == true)
                Json.Write("Files", "CompleteResource", "true");
            else Json.Write("Files", "CompleteResource", "false");
        }
    }
}
