using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    class ExchangeRates
    {
        [JsonProperty("currency")]
        public string CurrencyName { get; set; }

        [JsonProperty("rates")]
        public NameValueCollection Rates { get; set; }
    }
}
