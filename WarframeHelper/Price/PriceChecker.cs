using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeHelper.Price
{
    internal class PriceChecker
    {
        private NexusStats ns;
        private WarframeMarket wmarket;

        public PriceChecker()
        {
            ns = new NexusStats();
            wmarket = new WarframeMarket();
        }

        public async Task<PriceData> GetPriceAsync(string Name)
        {
            var NItem = await ns.CheckPrice(Name);
            var Result = new PriceData();
            Result.name = NItem.name;
            var NList = new List<PriceData.PriceComponent>();
            var MList = new List<PriceData.PriceComponent>();
            foreach (var item in NItem.components)
            {
                var nexusPC = new PriceData.PriceComponent();
                nexusPC.max = item.combined.max.Value;
                nexusPC.median = item.combined.median.Value;
                nexusPC.min = item.combined.min.Value;
                nexusPC.avg = item.combined.avg.Value;
                nexusPC.name = item.name;
                NList.Add(nexusPC);

                var wmarketPC = new PriceData.PriceComponent();
                double wmTwoDayMax = 0, wmTwoDayMin = 0, wmTwoDayAvg = 0, wmTwoDayMed = 0;
                WfMarketStatistics MPart;
                if (NItem.components.Length == 1)
                    MPart = await wmarket.CheckPrice(Name);
                else
                    MPart = await wmarket.CheckPrice(Name + " " + item.name);
                foreach (var wmData in MPart.twoDays)
                {
                    wmTwoDayMax += wmData.max_price;
                    wmTwoDayAvg += wmData.avg_price;
                    wmTwoDayMed += wmData.median;
                    wmTwoDayMin += wmData.min_price;
                }

                wmarketPC.name = item.name;
                wmarketPC.max = wmTwoDayMax / MPart.twoDays.Length;
                wmarketPC.avg = wmTwoDayAvg / MPart.twoDays.Length;
                wmarketPC.median = wmTwoDayMed / MPart.twoDays.Length;
                wmarketPC.min = wmTwoDayMin / MPart.twoDays.Length;

                MList.Add(wmarketPC);
            }
            Result.NexusPrice = NList.ToArray();
            Result.WfMarketPrice = MList.ToArray();
            return Result;
        }
    }

    internal class PriceData
    {
        public string name { get; set; }
        public PriceComponent[] NexusPrice { get; set; }
        public PriceComponent[] WfMarketPrice { get; set; }

        public class PriceComponent
        {
            public string name { get; set; }
            public double avg { get; set; }
            public double median { get; set; }
            public double min { get; set; }
            public double max { get; set; }
        }
    }
}