using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public class Balance
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

}
