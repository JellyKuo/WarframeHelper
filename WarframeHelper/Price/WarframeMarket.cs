using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeHelper.Price
{
    internal class WarframeMarket : PriceApi
    {
        public WarframeMarket() : base(Apis.WarframeMarket)
        {
        }

        public async Task<WFMarketItem> CheckPrice(string Name)
        {
        }
    }

    internal class WFMarketItem
    {
        public double

        public int closed_price { get; set; }
        public int min_price { get; set; }
        public double avg_price { get; set; }
        public int median { get; set; }
        public DateTime datetime { get; set; }
        public double moving_avg { get; set; }
        public int volume { get; set; }
        public int max_price { get; set; }
        public int open_price { get; set; }
    }
}