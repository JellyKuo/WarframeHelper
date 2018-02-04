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
    /// ArcaneWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ArcaneWindow : Window
    {
        private Price.PriceChecker checker = new Price.PriceChecker();

        public ArcaneWindow(Arcane Item)
        {
            InitializeComponent();
#if DEBUG
            Topmost = false;
#endif

            UpdateData(ref Item);
        }

        public ArcaneWindow(ref Arcane Item)
        {
            InitializeComponent();
            UpdateData(ref Item);
        }

        public void UpdateData(ref Arcane item)
        {
            NameTxt.Text = item.name.Remove(0, 7);
            TypeTxt.Text = "Arcane";
            RarityTxt.Text = "Rarity: " + item.rarity;
            LocationTxt.Text = "Location: " + item.location;
            EffectTxt.Text = item.effect;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(item.thumbnail, UriKind.Absolute);
            bitmap.EndInit();
            Img.ImageSource = bitmap;

            NexusStack.Visibility = Visibility.Collapsed;
            NexusProgress.Visibility = Visibility.Visible;
            WfMarketStack.Visibility = Visibility.Collapsed;
            WfMarketProgress.Visibility = Visibility.Visible;
            GetPrice(item.name);
        }

        private async void GetPrice(string Name)
        {
            var PriceCheckName = Name;
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