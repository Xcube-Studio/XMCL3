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
using Newtonsoft.Json.Linq;

namespace XMCL.Pages
{
    /// <summary>
    /// SubPage4.xaml 的交互逻辑
    /// </summary>
    public partial class SubPage4 : Page
    {
        public SubPage4()
        {
            InitializeComponent();
            TextPath.Text = Settings.GamePath;
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Length == 0)
            {
                NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(NameTextBox, new SolidColorBrush(Colors.Red));
                Done.IsEnabled = false;
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length == 0)
            {
                NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(NameTextBox, new SolidColorBrush(Colors.Red));
                Done.IsEnabled = false;
            }
            else
            {
                JArray jArray = Json.ReadPaths();bool a = false;
                foreach(JObject jObject in jArray)
                {
                    if ((string)jObject["Name"] == NameTextBox.Text)
                    {
                        a = true;
                    }
                }
                if (a)
                {
                    NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(NameTextBox, new SolidColorBrush(Colors.Red));
                    MaterialDesignThemes.Wpf.HintAssist.SetHelperText(NameTextBox, "该名称已存在");
                    Done.IsEnabled = false;
                }
                else
                {
                    MaterialDesignThemes.Wpf.TextFieldAssist.SetUnderlineBrush(NameTextBox, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#89000000")));
                    NameTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#89000000"));
                    MaterialDesignThemes.Wpf.HintAssist.SetHelperText(NameTextBox, "");
                    Done.IsEnabled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var open = new System.Windows.Forms.FolderBrowserDialog();
            open.ShowNewFolderButton = true;
            open.Description = "选择.minecraft文件夹";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                TextPath.Text = open.SelectedPath;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            Json.AddPath(NameTextBox.Text, TextPath.Text, "Grass_Block", (bool)c1.IsChecked);
            this.NavigationService.Navigate(new Page1());
        }
    }
}
