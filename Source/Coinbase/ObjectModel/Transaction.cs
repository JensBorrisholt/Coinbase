using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Coinbase.Helpers;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public class Transactions : List<Transaction>
    {
        private List<Buy> buys;

        public List<Buy> Buys
        {
            get
            {
                if (buys == null)
                {
                    buys = new List<Buy>();
                    foreach (var element in this.Where(e => e.Buy != null))
                        buys.Add(element.Buy);
                }
                return buys;
            }
        }
    }

    public class Trade
    {
        public Currency Currency { get; }
        public float Amount { get; }
        public float NativeAmount { get; }
        public Trade(Currency currency, string amount, string nativeAmount)
        {
            Currency = currency;
            Amount = float.Parse(amount, CultureInfo.InvariantCulture);
            NativeAmount = float.Parse(nativeAmount, CultureInfo.InvariantCulture);
        }

        public Trade(Transaction transaction)
            : this(transaction.Amount.Currency.ToEnum<Currency>(), transaction.Amount.PurpleAmount,
                transaction.NativeAmount.PurpleAmount)
        {

        }
    }

    public class Transaction
    {
        private Trade trade;

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("native_amount")]
        public Amount NativeAmount { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }

        [JsonProperty("buy")]
        public Buy Buy { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }

        public Trade Trade => trade ?? (trade = new Trade(this));
    }

    public class Details
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }
    }

    public class Buy
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }
    }

    public class Amount
    {
        [JsonProperty("amount")]
        public string PurpleAmount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
