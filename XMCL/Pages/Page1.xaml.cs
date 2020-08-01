using Ionic.Zip;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private void Back(object sender, RoutedEventArgs e)
        {
            try { this.NavigationService.Navigate(null); } catch { }
        }
        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            G1.Visibility = Visibility.Visible; G2.Visibility = G3.Visibility = Visibility.Collapsed;
            GetList();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Resources.Remove("PrimaryHueMidBrush");
            Resources.Add("PrimaryHueMidBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueMidBrush)));
            Resources.Remove("PrimaryHueLightBrush");
            Resources.Add("PrimaryHueLightBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueLightBrush)));
            Resources.Remove("PrimaryHueDarkBrush");
            Resources.Add("PrimaryHueDarkBrush", new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Settings.PrimaryHueDarkBrush)));
            GetList();
            GetList2();

            GPN.Text = Settings.GamePathName;
            string temp = (string)Json.ReadPath(Settings.GamePathName, "Path");
            if (temp.Length > 15)
                GP.Text = temp.Substring(0, 15) + "..";
            else GP.Text = temp;
        }
        void GetList()
        {
            List.Children.Clear();
            Task.Run(() =>
            {
                try
                {
                    if (Directory.Exists(Settings.GamePath + "\\versions"))
                    { }
                    else { Directory.CreateDirectory(Settings.GamePath + "\\versions"); }
                    string[] b = Directory.GetDirectories(Settings.GamePath + "\\versions");
                    for (int i = 0; i < b.Length; i++)
                    {
                        int c = (Settings.GamePath + "\\versions\\").Length;
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
                                radioButton.Checked += Checked1;
                                radioButton.Tag = d;
                                if (MainWindow.ComboBox.Text == d)
                                    radioButton.IsChecked = true;

                                Label label = new Label();
                                label.Content = d;
                                label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                                label.FontFamily = new FontFamily("Microsoft YaHei UI Light");
                                label.FontSize = 14;
                                label.Margin = new System.Windows.Thickness(75, 0, 90, 0);

                                Grid grid = new Grid();

                                Card card = new Card();
                                card.Height = 50;
                                card.Margin = new System.Windows.Thickness(0, 8, 0, 0);
                                if (i == b.Length - 1)
                                    card.Margin = new System.Windows.Thickness(0, 8, 0, 8);

                                grid.Children.Add(image);
                                grid.Children.Add(button);
                                grid.Children.Add(radioButton);
                                grid.Children.Add(label);

                                card.Content = grid;

                                List.Children.Add(card);
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        await MainWindow.ShowTip(ex.Message, 3);
                        await MainWindow.ShowTip("请重新设置游戏目录", 3);
                    }));
                }
            });
        }
        void GetList1()
        {
            List1.Children.Clear();
            if (Directory.Exists(Settings.GamePath + "\\mods"))
            {
                string[] mods = Directory.GetFiles(Settings.GamePath + "\\mods");
                foreach (string mod in mods)
                {
                    string a = Path.GetExtension(mod).ToLower();
                    if (Path.GetExtension(mod).ToLower() == ".jar")
                    { }
                    else if (Path.GetExtension(mod).ToLower() == ".disabled")
                    { }
                    else continue;
                    try
                    {
                        JObject jObject = new JObject(); string info;
                        ZipFile zipEntries = ZipFile.Read(mod);
                        if (zipEntries.ContainsEntry("fabric.mod.json"))
                        {
                            Stream stream = zipEntries["fabric.mod.json"].OpenReader();
                            StreamReader streamReader = new StreamReader(stream);
                            jObject = JObject.Parse(streamReader.ReadToEnd()); stream.Close(); streamReader.Close();
                            info = "fabric.mod.json";
                        }
                        else if (zipEntries.ContainsEntry("pack.mcmeta"))
                        {
                            Stream stream = zipEntries["pack.mcmeta"].OpenReader();
                            StreamReader streamReader = new StreamReader(stream);
                            jObject = JObject.Parse(streamReader.ReadToEnd()); stream.Close(); streamReader.Close();
                            info = "pack.mcmeta";
                        }
                        else info = "none";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Card card = new Card();
                            card.Height = 50;
                            card.Margin = new Thickness(0, 8, 0, 0);

                            Grid grid = new Grid();

                            CheckBox checkBox = new CheckBox();
                            checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                            checkBox.VerticalAlignment = VerticalAlignment.Center;
                            checkBox.Margin = new Thickness(15, 0, 0, 0);
                            checkBox.Tag = mod;
                            if (Path.GetExtension(mod).ToLower() == ".jar")
                                checkBox.IsChecked = true;
                            checkBox.Click += Checked2;

                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Margin = new Thickness(50, 8, 50, 8);

                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 15;
                            textBlock.Text = Path.GetFileNameWithoutExtension(mod);

                            TextBlock textBlock1 = new TextBlock();
                            textBlock1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF939393"));
                            if (info != "none")
                            {
                                if (info == "fabric.mod.json")
                                {
                                    textBlock1.Text = (string)jObject["name"] + ",版本:" + (string)jObject["version"] + ",作者:";
                                    try
                                    {
                                        JArray jArray = JArray.Parse(jObject["authors"].ToString());
                                        foreach (JToken jToken in jArray)
                                            textBlock1.Text += jToken + " ";
                                    }
                                    catch { textBlock1.Text += "null"; }
                                }
                                else if (info == "pack.mcmeta")
                                    textBlock1.Text = (string)jObject["pack"]["description"];
                            }
                            else textBlock1.Text = textBlock.Text;

                            stackPanel.Children.Add(textBlock);
                            stackPanel.Children.Add(textBlock1);
                            grid.Children.Add(stackPanel);
                            grid.Children.Add(checkBox);
                            card.Content = grid;
                            List1.Children.Add(card);
                        }));
                    }
                    catch { continue; }
                }
            }
        }
        void GetList2()
        {
            List2.Children.Clear();
            JArray jArray = Json.ReadPaths();
            foreach (JObject jObject in jArray)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Card card = new Card();
                    card.Height = 50;
                    card.Margin = new Thickness(0, 8, 0, 0);

                    Grid grid = new Grid();

                    Image image = new Image();
                    image.SnapsToDevicePixels = true;
                    image.UseLayoutRounding = true;
                    image.Width = image.Height = 35;
                    image.Margin = new System.Windows.Thickness(35, 5, 0, 5);
                    image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                    if ((string)jObject["Icon"] != "Grass_Block")
                        image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Furnace.png", UriKind.Relative));
                    else image.Source = new BitmapImage(new Uri("/XMCL;component/Resources/Grass_Block.png", UriKind.Relative));

                    RadioButton checkBox = new RadioButton();
                    checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                    checkBox.VerticalAlignment = VerticalAlignment.Center;
                    checkBox.Margin = new Thickness(10, 0, 0, 0);
                    checkBox.Tag = (string)jObject["Name"];
                    if ((string)jObject["Name"] == Settings.GamePathName)
                        checkBox.IsChecked = true;
                    checkBox.GroupName = "GP";
                    checkBox.Checked += Checked3;

                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Margin = new Thickness(75, 8, 10, 8);

                    TextBlock textBlock = new TextBlock();
                    textBlock.FontSize = 15;
                    textBlock.Text = (string)jObject["Name"];

                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF939393"));
                    textBlock1.Text = (string)jObject["Path"];

                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(textBlock1);
                    grid.Children.Add(image);
                    grid.Children.Add(stackPanel);
                    grid.Children.Add(checkBox);
                    card.Content = grid;
                    List2.Children.Add(card);
                }));
            }
        }
        private void Checked3(object sender, RoutedEventArgs e)
        {
            Json.Write("Files", "GamePathName", (string)((RadioButton)sender).Tag);
            GPN.Text = Settings.GamePathName;
            string temp = (string)Json.ReadPath(Settings.GamePathName, "Path");
            if (temp.Length > 15)
                GP.Text = temp.Substring(0, 15) + "..";
            else GP.Text = temp;
            GetList1();
        }
        private void Checked2(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (!(bool)checkBox.IsChecked)
                File.Move((string)checkBox.Tag, Path.ChangeExtension((string)checkBox.Tag, ".disabled"));
            else File.Move((string)checkBox.Tag, Path.ChangeExtension((string)checkBox.Tag, ".jar"));
        }
        private void Checked1(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            foreach (string text in MainWindow.ComboBox.Items)
                if (text == radioButton.Tag.ToString())
                    MainWindow.ComboBox.SelectedItem = text;
        }
        private void ToSubPage1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame1.Navigate(new SubPage1());
        }
        private void R1_Checked(object sender, RoutedEventArgs e)
        {
            GetList1();
        }
        private void ListBoxItem_PreviewMouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            GetList1();
            G2.Visibility = Visibility.Visible; G1.Visibility = G3.Visibility = Visibility.Collapsed;
        }
        private void ListBoxItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            GetList2();
            G1.Visibility = G2.Visibility = Visibility.Collapsed; G3.Visibility = Visibility.Visible;
        }
        private void RfGP_Click(object sender, RoutedEventArgs e)
        {
            GetList2();
        }
        private void ToSubPage4_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame1.Navigate(new SubPage4());
        }
    }
}
