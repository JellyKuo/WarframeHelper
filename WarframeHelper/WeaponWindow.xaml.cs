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
    /// WeaponWindow.xaml 的互動邏輯
    /// </summary>
    public partial class WeaponWindow : Window
    {
        private Price.PriceChecker checker = new Price.PriceChecker();

        public WeaponWindow(Weapon Item)
        {
            InitializeComponent();
            UpdateData(ref Item);
        }

        public WeaponWindow(ref Weapon Item)
        {
            InitializeComponent();
            UpdateData(ref Item);
        }

        public void UpdateData(ref Weapon weapon)
        {
            NameTxt.Text = weapon.name;
            TypeTxt.Text = weapon.type;

            if (weapon.subtype != null && weapon.subtype != "")
                SubtypeTxt.Text = weapon.subtype;
            else
                SubtypeTxt.Visibility = Visibility.Hidden;

            //TODO: Get ALL Image (SUPER HARD PART)
            if (weapon.thumbnail != null && weapon.thumbnail != "")
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(weapon.thumbnail, UriKind.Absolute);
                bitmap.EndInit();
                Img.ImageSource = bitmap;
            }
            else
                Img.ImageSource = null;

            DamageTxt.Text = "Damage: " + weapon.damage;
            ImpactTxt.Text = "Impact: " + weapon.impact;
            PunctureTxt.Text = "Puncture: " + weapon.puncture;
            SlashTxt.Text = "Slash: " + weapon.slash;
            CritChanceTxt.Text = "Crit Chance: " + weapon.crit_chance;
            CritMultiTxt.Text = "Crit Multiplier: " + weapon.crit_mult;
            StatusChanceTxt.Text = "Status Chance: " + weapon.status_chance;

            if (weapon.trigger != null && weapon.trigger != "")
            {
                MeleeStack.Visibility = Visibility.Collapsed;
                PrimaryStack.Visibility = Visibility.Visible;

                TriggerTxt.Text = "Trigger: " + weapon.trigger;
                ProjectileTxt.Text = "Projectile: " + weapon.projectile;
                RateTxt.Text = "Rate: " + weapon.rate;
                AccuracyTxt.Text = "Accuracy: " + weapon.accuracy;
                MagazineTxt.Text = "Magazine: " + weapon.magazine;
                AmmoTxt.Text = "Ammo: " + weapon.ammo;
                ReloadTxt.Text = "Reload: " + weapon.reload;
                FlightTxt.Text = "Flight: " + weapon.flight;
                NoiseTxt.Text = "Noise: " + weapon.noise;
                RivenDispositionTxt.Text = "Riven Disposition: " + weapon.riven_disposition;
            }
            else
            {
                PrimaryStack.Visibility = Visibility.Collapsed;
                MeleeStack.Visibility = Visibility.Visible;

                SlideTxt.Text = "Slide: " + weapon.slide;
                JumpTxt.Text = "Jump: " + weapon.jump;
                WallTxt.Text = "Wall: " + weapon.wall;
                SpeedTxt.Text = "Speed: " + weapon.speed;
                ChannelingTxt.Text = "Channeling: " + weapon.channeling;
            }

            if (weapon.name.Contains("Prime"))
            {
                NexusStack.Visibility = Visibility.Collapsed;
                NexusProgress.Visibility = Visibility.Visible;
                WfMarketStack.Visibility = Visibility.Collapsed;
                WfMarketProgress.Visibility = Visibility.Visible;
                PriceGBox.Visibility = Visibility.Visible;
                GetPrice(weapon.name);
            }
            else
            {
                PriceGBox.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetPrice(string Name)
        {
            var Data = await checker.GetPriceAsync(Name);

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