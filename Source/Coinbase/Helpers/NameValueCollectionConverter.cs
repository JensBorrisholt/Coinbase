using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Coinbase.Helpers
{
    /// <summary>
    /// Custom converter for (de)serializing NameValueCollection
    /// Add an instance to the settings Converters collection
    /// </summary>
    public class NameValueCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is NameValueCollection collection))
                return;

            writer.WriteStartObject();

            foreach (var key in collection.AllKeys)
            {
                writer.WritePropertyName(key);
                writer.WriteValue(collection.Get(key));
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var nameValueCollection = new NameValueCollection();
            var key = "";
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        nameValueCollection = new NameValueCollection();
                        break;
                    case JsonToken.EndObject:
                        return nameValueCollection;
                    case JsonToken.PropertyName:
                        key = reader.Value.ToString();
                        break;
                    case JsonToken.String:
                        nameValueCollection.Add(key, reader.Value.ToString());
                        break;
                }
            }
            return nameValueCollection;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(NameValueCollection);
    }
}
