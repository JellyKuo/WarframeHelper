using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using WarframeHelper.Game;

namespace WarframeHelper
{
    /// <summary>
    /// SearchWindow.xaml 的互動邏輯
    /// </summary>
    public partial class SearchWindow : Window
    {
        private GameItem[] Items;
        private List<GameItem> MatchedItems;
        private const int MaxSugCount = 10;
        private int SugCount = 0;
        private int SugIndex = 0;
        private TextBlock[] SugTxts;
        private WarframeWindow wfWindow;
        private WeaponWindow wpWindow;
        private ArcaneWindow acWindow;

        public SearchWindow()
        {
            InitializeComponent();
            Items = GameItem.ParseAllItems();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
#if DEBUG
                Environment.Exit(0);
#else
                Close();
#endif
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SugTxts != null)
                foreach (var SugTxt in SugTxts)
                    MainGrid.Children.Remove(SugTxt);

            MatchedItems = new List<GameItem>();
            var Query = from Item in Items where (Item.name.ToLower().Contains(SearchBox.Text.ToLower())) select Item;
            foreach (var Item in Query.Take(MaxSugCount + 1))
                MatchedItems.Add(Item);
            SugCount = MatchedItems.Count;
            if (SugCount == 0 || SearchBox.Text == "")
            {
                Height = 50;
                return;
            }
            SugTxts = new TextBlock[SugCount];
            for (int i = 0; i < SugCount; i++)
            {
                SugTxts[i] = new TextBlock()
                {
                    Text = MatchedItems[i].name,
                    Margin = new Thickness(5, 50 + i * 40, 5, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 24,
                    Foreground = Brushes.White,
                    Background = Brushes.Transparent
                };
                MainGrid.Children.Add(SugTxts[i]);
            }
            SugIndex = 0;
            SugTxts[0].Background = Brushes.Gray;
            Height = 50 + (SugCount) * 40;
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (SugIndex - 1 < 0)
                        break;
                    SugTxts[SugIndex].Background = Brushes.Transparent;
                    SugIndex -= 1;
                    SugTxts[SugIndex].Background = Brushes.Gray;
                    break;

                case Key.Down:
                    if (SugIndex + 1 > SugTxts.Length - 1)
                        break;
                    SugTxts[SugIndex].Background = Brushes.Transparent;
                    SugIndex += 1;
                    SugTxts[SugIndex].Background = Brushes.Gray;
                    break;

                case Key.Enter:
                    switch (MatchedItems[SugIndex].type)
                    {
                        case ItemType.Warframe:
                            var warframe = (Warframe)MatchedItems[SugIndex].item;
                            if (wfWindow == null)
                                wfWindow = new WarframeWindow(warframe);
                            else
                                wfWindow.UpdateData(ref warframe);
                            wfWindow.Show();
                            break;

                        case ItemType.Weapon:
                            var weapon = (Weapon)MatchedItems[SugIndex].item;
                            if (wpWindow == null)
                                wpWindow = new WeaponWindow(weapon);
                            else
                                wpWindow.UpdateData(ref weapon);
                            wpWindow.Show();
                            break;

                        case ItemType.Arcane:
                            var arcane = (Arcane)MatchedItems[SugIndex].item;
                            if (acWindow == null)
                                acWindow = new ArcaneWindow(ref arcane);
                            else
                                acWindow.UpdateData(ref arcane);
                            acWindow.Show();
                            break;

                        default:
                            break;
                    }

                    Activate();
                    SearchBox.Focus();
                    break;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Activate();
            SearchBox.Focus();
        }
    }
}