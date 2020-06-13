using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XMCL.Core;

namespace XMCL.Pages
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        public static Page Page;
        public Page1()
        {
            InitializeComponent();
            Page = this;
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            try { this.NavigationService.Navigate(null); } catch { }
        }
        private void ListBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List.Children.Clear();
            Task.Run(() =>
            {
                string GamePath;
                if (Convert.ToBoolean(Json.Read("Files", "UseDefaultDirectory")))
                    GamePath = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft");
                else GamePath = Json.Read("Files", "GamePath");

                if (Directory.Exists(GamePath + "\\versions"))
                { }
                else { Directory.CreateDirectory(GamePath + "\\versions"); }
                string[] b = Directory.GetDirectories(GamePath + "\\versions");
                for (int i = 0; i < b.Length; i++)
                {
                    int c = (GamePath + "\\versions\\").Length;
                    string d = b[i].Substring(c, b[i].Length - c);
                    string json = b[i] + "\\" + d + ".json";
                    if (File.Exists(json))
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Image image = new Image();
                            image.SnapsToDevicePixels = true;
                            image.UseLayoutRounding = true;
                            image.Width = image.Height = 35;
                            image.Margin = new System.Windows.Thickness(35, 5, 0, 5);
                            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                            if (File.ReadAllText(json).Contains("inheritsFrom"))
                                image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Furnace.png", UriKind.Relative));
                            else image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Grass_Block.png", UriKind.Relative));

                            Button button = new Button();
                            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            button.Margin = new System.Windows.Thickness(0, 10, 10, 10);
                            button.Width = 45;
                            button.Height = 25;
                            button.Style = (Style)this.FindResource("MaterialDesignFlatButton");
                            button.Background = button.BorderBrush = new SolidColorBrush(Colors.White);
                            ShadowAssist.SetShadowDepth(button, ShadowDepth.Depth0);
                            button.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#DD000000");
                            button.Padding = new System.Windows.Thickness(0);

                            PackIcon packIcon = new PackIcon();
                            packIcon.Kind = PackIconKind.DotsVertical;
                            packIcon.Width = packIcon.Height = 22;
                            button.Content = packIcon;

                            RadioButton radioButton = new RadioButton();
                            radioButton.Margin = new System.Windows.Thickness(10, 0, 0, 0);
                            radioButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            radioButton.Width = 25;
                            radioButton.GroupName = "A";

                            Label label = new Label();
                            label.Content = d;
                            label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            label.FontFamily = new FontFamily("Microsoft YaHei UI Light");
                            label.FontSize = 14;
                            label.Margin = new System.Windows.Thickness(75, 0, 90, 0);

                            Grid grid = new Grid();
                            grid.Height = 45;
                            grid.Background = new SolidColorBrush(Colors.White);
                            grid.Margin = new System.Windows.Thickness(5, 5, 5, 0);
                            if (i == b.Length - 1)
                                grid.Margin = new System.Windows.Thickness(5, 5, 5, 5);

                            grid.Children.Add(image);
                            grid.Children.Add(button);
                            grid.Children.Add(radioButton);
                            grid.Children.Add(label);

                            List.Children.Add(grid);
                        }));

                    }
                }
            });
        }
    }
}
