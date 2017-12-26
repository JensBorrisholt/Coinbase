using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public enum PriceKind { Buy, Sell, Spot };

    public class Price
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        public DateTime Date { get; set; }
        public PriceKind Kind { get; set; }
    }
}
