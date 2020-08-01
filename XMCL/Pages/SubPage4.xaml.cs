using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                JArray jArray = Json.ReadPaths(); bool a = false;
                foreach (JObject jObject in jArray)
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
            {
                if ((bool)c1.IsChecked)
                    try
                    {
                        TextPath.Text = RelativePath(System.IO.Directory.GetCurrentDirectory(), open.SelectedPath);
                    }
                    catch { MainWindow.ShowTip("这不是一个有效的相对路径", 3); }
                else
                    TextPath.Text = open.SelectedPath;
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            Json.AddPath(NameTextBox.Text, TextPath.Text, "Grass_Block", (bool)c1.IsChecked);
            this.NavigationService.Navigate(new Page1());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }
        private string RelativePath(string absolutePath, string relativeTo)
        {
            string[] absoluteDirectories = absolutePath.Split('\\');
            string[] relativeDirectories = relativeTo.Split('\\');

            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            //Find common root
            for (index = 0; index < length; index++)
                if (absoluteDirectories[index] == relativeDirectories[index])
                    lastCommonRoot = index;
                else
                    break;

            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                throw new ArgumentException("Paths do not have a common base");

            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            //Add on the ..
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
                if (absoluteDirectories[index].Length > 0)
                    relativePath.Append("..\\");

            //Add on the folders
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
                relativePath.Append(relativeDirectories[index] + "\\");
            relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

            return relativePath.ToString();
        }
    }
}
