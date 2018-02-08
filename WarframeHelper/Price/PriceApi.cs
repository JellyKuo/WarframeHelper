using System;

namespace WarframeHelper.Price
{
    internal class PriceApi
    {
        public string ApiUrl { get; }

        public PriceApi(Apis api)
        {
            switch (api)
            {
                case Apis.NexusStats:
                    ApiUrl = (string)Properties.Settings.Default["NexusApi"] + @"/warframe/v1/items";
                    break;

                case Apis.WarframeMarket:
                    ApiUrl = (string)Properties.Settings.Default["WarframeMarketApi"] + @"/v1/items/{0}/statistics";
                    break;

                case Apis.WarframeStatus:
                    ApiUrl = (string)Properties.Settings.Default["WarframeStatusApi"] + @"/pricecheck/find";
                    break;

                default:
                    throw new InvalidOperationException("Selected Api does not confront with PriceApi parent class");
            }
        }
    }

    internal enum Apis { NexusStats, WarframeMarket, WarframeStatus }
}