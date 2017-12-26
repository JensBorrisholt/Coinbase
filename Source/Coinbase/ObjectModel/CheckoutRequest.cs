using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Coinbase.ObjectModel
{
    public enum OrderType
    {
        Order,
        Donation
    }

    public enum OrderStyle
    {
        buy_now_large,
        buy_now_small,
        donation_large,
        donation_small,
        custom_large,
        custom_small
    }

    public class CheckoutRequest
    {
        public CheckoutRequest()
        {
            Metadata = new Dictionary<string, object>();
        }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType Type { get; set; }

        public string Style { get; set; }

        [JsonProperty("customer_defined_amount")]
        public bool? CustomerDefinedAmount { get; set; }

        [JsonProperty("amount_presets")]
        public decimal[] AmountPresets { get; set; }

        [JsonProperty("success_url")]
        public string SuccessUrl { get; set; }

        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }

        [JsonProperty("notifications_url")]
        public string NotificationsUrl { get; set; }

        [JsonProperty("auto_redirect")]
        public bool? AutoRedirect { get; set; }

        [JsonProperty("collect_shipping_address")]
        public bool? CollectShippingAddress { get; set; }

        [JsonProperty("collect_email")]
        public bool? CollectEmail { get; set; }

        [JsonProperty("collect_phone_number")]
        public bool? CollectPhoneNumber { get; set; }

        [JsonProperty("collect_country")]
        public bool? CollectCountry { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
