using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Coinbase.ObjectModel
{
    public class Addres
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resource_path")]
        public string ResourcePath { get; set; }
    }

    public class Addresses : List<Addres>
    {
        
    }
}
