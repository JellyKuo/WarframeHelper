using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WarframeHelper.Price
{
    internal class WarframeMarket : PriceApi
    {
        private HttpClient client { get; }

        public WarframeMarket() : base(Apis.WarframeMarket)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(ApiUrl)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WfMarketStatistics> CheckPrice(string Name)
        {
            string resultJson;
            resultJson = await client.GetStringAsync(string.Format("/v1/items/{0}/statistics", Name).Replace(' ', '_').ToLower());
            var jsonType = new { payload = new { statistics = new WfMarketStatistics() } };
            var parsedResult = JsonConvert.DeserializeAnonymousType(resultJson, jsonType);

            return parsedResult.payload.statistics;
        }
    }

    internal class WfMarketItem
    {
        //public int closed_price { get; set; }
        public int min_price { get; set; }

        public double avg_price { get; set; }
        public double median { get; set; }

        //public string datetime { get; set; }
        //public double moving_avg { get; set; }
        //public int volume { get; set; }
        public int max_price { get; set; }

        //public int open_price { get; set; }
    }

    internal class WfMarketStatistics
    {
        [JsonProperty(PropertyName = "48hours")]
        public WfMarketItem[] twoDays;

        //[JsonProperty(PropertyName = "90days")]
        //public WfMarketItem[] nintyDays;
    }
}