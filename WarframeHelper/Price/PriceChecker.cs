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
                var pc = new PriceData.PriceComponent();
                pc.max = item.combined.max.Value;
                pc.median = item.combined.median.Value;
                pc.min = item.combined.min.Value;
                pc.avg = item.combined.avg.Value;
                pc.name = item.name;
                NList.Add(pc);
            }
            Result.NexusPrice = NList.ToArray();

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