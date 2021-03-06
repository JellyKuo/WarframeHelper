﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                NexusStack.Visibility = Visibility.Collapsed;
                NexusProgress.Visibility = Visibility.Visible;
                WfMarketStack.Visibility = Visibility.Collapsed;
                WfMarketProgress.Visibility = Visibility.Visible;
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
                if (!string.IsNullOrEmpty(warframe.prime_health))
                {
                    PHealthTxt.Text = "Health: " + warframe.prime_health;
                    PHealthTxt.Foreground = Brushes.LightPink;
                }
                else
                    PHealthTxt.Text = HealthTxt.Text;

                if (!string.IsNullOrEmpty(warframe.prime_shield))
                {
                    PShieldTxt.Text = "Shield: " + warframe.prime_shield;
                    PShieldTxt.Foreground = Brushes.LightPink;
                }
                else
                    PShieldTxt.Text = ShieldTxt.Text;

                if (!string.IsNullOrEmpty(warframe.prime_armor))
                {
                    PArmorTxt.Text = "Armor: " + warframe.prime_armor;
                    PArmorTxt.Foreground = Brushes.LightPink;
                }
                else
                    PArmorTxt.Text = ArmorTxt.Text;

                if (!string.IsNullOrEmpty(warframe.prime_power))
                {
                    PPowerTxt.Text = "Power: " + warframe.prime_power;
                    PPowerTxt.Foreground = Brushes.LightPink;
                }
                else
                    PPowerTxt.Text = PowerTxt.Text;

                if (!string.IsNullOrEmpty(warframe.prime_speed))
                {
                    PSpeedTxt.Text = "Speed: " + warframe.prime_speed;
                    PSpeedTxt.Foreground = Brushes.LightPink;
                }
                else
                    PSpeedTxt.Text = SpeedTxt.Text;

                if (!string.IsNullOrEmpty(warframe.prime_conclave))
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
            NexusProgress.Visibility = Visibility.Collapsed;
            NexusStack.Visibility = Visibility.Visible;
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
                    Text = "Med: " + Data.NexusPrice[i].median
                };
                var avgTb = new TextBlock
                {
                    Text = "Avg: " + Math.Round(Data.NexusPrice[i].avg, 2),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                var rangeTb = new TextBlock
                {
                    Text = "Range: " + Data.NexusPrice[i].min + "~" + Data.NexusPrice[i].max
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

            WfMarketProgress.Visibility = Visibility.Collapsed;
            WfMarketStack.Visibility = Visibility.Visible;
            WfMarketStack.Children.Clear();
            for (int i = 0; i < Data.WfMarketPrice.Length; i++)
            {
                var nameTb = new TextBlock
                {
                    FontSize = 20,
                    Text = Data.WfMarketPrice[i].name,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var medTb = new TextBlock
                {
                    Text = "Med: " + Math.Round(Data.WfMarketPrice[i].median, 0)
                };
                var avgTb = new TextBlock
                {
                    Text = "Avg: " + Math.Round(Data.WfMarketPrice[i].avg, 2),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                var rangeTb = new TextBlock
                {
                    Text = "Range: " + Math.Round(Data.WfMarketPrice[i].min, 0) + "~" + Math.Round(Data.WfMarketPrice[i].max, 0)
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
                WfMarketStack.Children.Add(card);
            }
        }
    }
}