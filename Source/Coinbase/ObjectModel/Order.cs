using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public class Metadata
    {
    }

    public class OrderTransaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }
    }

    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("payout_amount")]
        public object PayoutAmount { get; set; }

        [JsonProperty("bitcoin_address")]
        public string BitcoinAddress { get; set; }

        [JsonProperty("bitcoin_amount")]
        public Amount BitcoinAmount { get; set; }

        [JsonProperty("bitcoin_uri")]
        public string BitcoinUri { get; set; }

        [JsonProperty("receipt_url")]
        public string ReceiptUrl { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty("mispaid_at")]
        public object MispaidAt { get; set; }

        [JsonProperty("paid_at")]
        public DateTime PaidAt { get; set; }

        [JsonProperty("refund_address")]
        public string RefundAddress { get; set; }

        [JsonProperty("transaction")]
        public OrderTransaction Transaction { get; set; }

        [JsonProperty("refunds")]
        public object[] Refunds { get; set; }

        [JsonProperty("mispayments")]
        public object[] Mispayments { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }
    }

    public class Orders : List<Order>
    {

    }

}
