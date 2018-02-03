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
using System.Windows.Shapes;

using WarframeHelper.Game;

namespace WarframeHelper
{
    /// <summary>
    /// WarframeWindow.xaml 的互動邏輯
    /// </summary>
    public partial class WarframeWindow : Window
    {
        private ImageProvider provider = new ImageProvider();
        private Price.PriceChecker checker = new Price.PriceChecker();

        public WarframeWindow(Warframe Item)
        {
            InitializeComponent();
#if DEBUG
            Topmost = false;
#endif

            UpdateData(ref Item);
        }

        public WarframeWindow(ref Warframe Item)
        {
            InitializeComponent();
            UpdateData(ref Item);
        }

        public void UpdateData(ref Warframe warframe)
        {
            NameTxt.Text = warframe.name;
            TypeTxt.Text = "Warframe";

            if (warframe.prime_health != null)
            {
                PriceGBox.Visibility = Visibility.Visible;
                GetPrice(warframe.name);
            }
            else
                PriceGBox.Visibility = Visibility.Collapsed;

            //TODO: Image Provider to cache image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(warframe.thumbnail, UriKind.Absolute);
            bitmap.EndInit();
            Img.ImageSource = bitmap;

            //Img.ImageSource = provider.GetImage(warframe.thumbnail);

            DescTxt.Text = warframe.description;

            HealthTxt.Text = "Health: " + warframe.health;
            ShieldTxt.Text = "Sheld: " + warframe.shield;
            ArmorTxt.Text = "Armor: " + warframe.armor;
            PowerTxt.Text = "Power: " + warframe.power;
            SpeedTxt.Text = "Speed: " + warframe.speed;
            ConclaveTxt.Text = "Conclave: " + warframe.conclave;

            if (warframe.prime_health != null)
            {
                if (warframe.prime_health != "" && warframe.prime_health != null)
                {
                    PHealthTxt.Text = "Health: " + warframe.prime_health;
                    PHealthTxt.Foreground = Brushes.LightPink;
                }
                else
                    PHealthTxt.Text = HealthTxt.Text;

                if (warframe.prime_shield != "" && warframe.prime_shield != null)
                {
                    PShieldTxt.Text = "Shield: " + warframe.prime_shield;
                    PShieldTxt.Foreground = Brushes.LightPink;
                }
                else
                    PShieldTxt.Text = ShieldTxt.Text;

                if (warframe.prime_armor != "" && warframe.prime_armor != null)
                {
                    PArmorTxt.Text = "Armor: " + warframe.prime_armor;
                    PArmorTxt.Foreground = Brushes.LightPink;
                }
                else
                    PArmorTxt.Text = ArmorTxt.Text;

                if (warframe.prime_power != "" && warframe.prime_power != null)
                {
                    PPowerTxt.Text = "Power: " + warframe.prime_power;
                    PPowerTxt.Foreground = Brushes.LightPink;
                }
                else
                    PPowerTxt.Text = PowerTxt.Text;

                if (warframe.prime_speed != "" && warframe.prime_speed != null)
                {
                    PSpeedTxt.Text = "Speed: " + warframe.prime_speed;
                    PSpeedTxt.Foreground = Brushes.LightPink;
                }
                else
                    PSpeedTxt.Text = SpeedTxt.Text;

                if (warframe.prime_conclave != "" && warframe.prime_conclave != null)
                {
                    PConclaveTxt.Text = "Conclave: " + warframe.prime_conclave;
                    PConclaveTxt.Foreground = Brushes.LightPink;
                }
                else
                    PConclaveTxt.Text = ConclaveTxt.Text;

                Grid.SetColumnSpan(AttrGBox, 1);
                PrimedGBox.Visibility = Visibility.Visible;
            }
            else
            {
                PrimedGBox.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(AttrGBox, 2);
            }
        }

        private async void GetPrice(string Name)
        {
            var PriceCheckName = Name.Split(' ')[0] + " Prime";
            var Data = await checker.GetPriceAsync(PriceCheckName);

            NexusStack.Children.Clear();
            for (int i = 0; i < Data.NexusPrice.Length; i++)
            {
                var nameTb = new TextBlock
                {
                    FontSize = 20,
                    Text = Data.NexusPrice[i].name,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var medTb = new TextBlock
                {
                    Text = "Med: " + Data.NexusPrice[i].median.ToString()
                };
                var avgTb = new TextBlock
                {
                    Text = "Avg: " + Math.Round(Data.NexusPrice[i].avg, 2).ToString(),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                var rangeTb = new TextBlock
                {
                    Text = "Range: " + Data.NexusPrice[i].min.ToString() + "~" + Data.NexusPrice[i].max.ToString()
                };
                Grid.SetColumnSpan(nameTb, 2);
                Grid.SetRow(medTb, 1);
                Grid.SetRow(avgTb, 1);
                Grid.SetColumn(avgTb, 1);
                Grid.SetRow(rangeTb, 2);
                Grid.SetColumnSpan(rangeTb, 2);

                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                grid.Children.Add(nameTb);
                grid.Children.Add(medTb);
                grid.Children.Add(avgTb);
                grid.Children.Add(rangeTb);
                var card = new MaterialDesignThemes.Wpf.Card()
                {
                    Padding = new Thickness(4),
                    Margin = new Thickness(2)
                };
                card.Content = grid;
                NexusStack.Children.Add(card);
            }

            WfMarketStack.Children.Clear();
            //for (int i = 0; i < Data.WfMarketPrice.Length; i++)
            //{
            //}
        }
    }
}