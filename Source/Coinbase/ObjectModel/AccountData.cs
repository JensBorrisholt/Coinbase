using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public class AccountData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("balance")]
        public Balance Balance { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }

        [JsonProperty("native_balance")]
        public Balance NativeBalance { get; set; }
    }

    public class AccountsData : List<AccountData>
    {

    }
}
