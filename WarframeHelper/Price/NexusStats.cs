using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WarframeHelper.Price
{
    internal class NexusStats : PriceApi
    {
        public DateTime CacheTime { get; set; }
        public NexusItem[] Items { get; set; }

        public NexusStats() : base(Apis.NexusStats)
        {
        }

        public async Task RefreshCacheAsync()
        {
            string resultJson = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                resultJson = await client.GetStringAsync("?data=prices");
            }
            Items = JsonConvert.DeserializeObject<NexusItem[]>(resultJson);
            CacheTime = DateTime.Now;
        }

        public async Task<NexusItem> CheckPrice(string Name)
        {
            if (Items == null)
                await RefreshCacheAsync();
            if (Items != null && DateTime.Now - CacheTime > new TimeSpan(0, 1, 30))
                await RefreshCacheAsync();

            NexusItem Result = null;
            var Query = from Item in Items where (Item.name.ToLower().Contains(Name.ToLower())) select Item;
            Result = Query.First();
            if (Result == null)
                throw new Exception();
            return Result;
        }
    }

    internal class NexusItem
    {
        public string name { get; set; }
        public NexusComponent[] components { get; set; }

        internal class NexusComponent
        {
            public string name { get; set; }

            //public NexusPrice selling { get; set; }
            //public NexusPrice buying { get; set; }
            public NexusPrice combined { get; set; }
        }

        internal class NexusPrice
        {
            public double? avg { get; set; }
            public double? median { get; set; }
            public double? min { get; set; }
            public double? max { get; set; }
        }
    }
}