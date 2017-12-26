using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Coinbase.Helpers;
using Coinbase.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace Coinbase
{

    public sealed class Coinbase
    {
        private static CoinbaseApi Api => CoinbaseApi.Instance;
        private Dictionary<Currency, NameValueCollection> exchangeRates;

        public List<Account> Accounts { get; }

        public Orders Orders { get; private set; }
        public User UserInformation { get; private set; }
        public Dictionary<Currency, NameValueCollection> ExchangeRates => exchangeRates;
        public NameValueCollection GetExchangeRates(Currency currency)
        {
            var options = new
            {
                currency = currency.ToString(),
            };

            var rates = Api.SendRequest<ExchangeRates>("exchange-rates", options, Method.GET)?.Data?.Rates;
            if (rates != null)
                exchangeRates[currency] = rates;
            return rates;
        }


        public Price SpotPrices(string currencyPair, DateTime? asOff = null)
        {
            if (asOff == null)
                asOff = DateTime.Now;

            asOff = asOff.Value.Date;
            var endPoint = $"prices/{currencyPair.ToUpper()}/spot";
            var request = WebRequest.Create(CoinbaseConstants.LiveApiUrl + endPoint + "?date=" + asOff.Value.ToString("yyyy-MM-dd"));
            request.Headers.Add("CB-VERSION", CoinbaseConstants.ApiVersionDate);
            var reader = new StreamReader(request.GetResponse().GetResponseStream());

            var result = JsonConvert.DeserializeObject<CoinbaseResponse<Price>>(reader.ReadToEnd()).Data;
            if (result != null)
            {
                result.Date = asOff.Value;
                result.Kind = PriceKind.Spot;
            }

            /*  
            var endPoint = $"prices/{currencyPair.ToUpper()}/spot";
            var result = Api.SendGetRequest<Price>(endPoint, new KeyValuePair<string, string>("date", asOff.Value.Date.ToString("yyyy-MM-dd")))?.Data;
            */

            return result;
        }

        public float PnL(Currency currency, DateTime? cutoffDate)
        {
            var account = Accounts.FirstOrDefault(a => a.Currency == currency);
            var amount = 0f;
            var nativeAmount = 0f;
            foreach (var transaction in account.Transactions.Where(t => t.CreatedAt <= cutoffDate))
            {
                amount += transaction.Trade.Amount;
                nativeAmount += transaction.Trade.NativeAmount;
            }

            var spotprice = float.Parse(SpotPrices(currency, cutoffDate).Amount, CultureInfo.InvariantCulture);
            return spotprice * amount - nativeAmount;
        }

        public Price SpotPrices(Currency currency, DateTime? asOff = null) => SpotPrices($"{currency}-{UserInformation.NativeCurrency}", asOff);

        public void RefreshExchangeRates()
        {
            exchangeRates = new Dictionary<Currency, NameValueCollection>();

            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
                Task.Run(() => GetExchangeRates(currency));
        }

        public Coinbase()
        {

            if ( string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApiKey"]) || string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApiSecret"]))
                throw  new ArgumentNullException($"Don't forget to update ApiKey and  ApiSecret in AppSettings");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };

            settings.Converters.Add(new NameValueCollectionConverter());
            JsonConvert.DefaultSettings = () => settings;
            Accounts = new List<Account>();
            var taskList = new List<Task>
            {
                Task.Run(() => RefreshExchangeRates()),
                Task.Run(() => UserInformation = Api.SendRequest<User>("user", null, Method.GET)?.Data),
                Task.Run(() => Orders = Api.SendRequest<Orders>("orders", null, Method.GET)?.Data)
            };

            var task = Task.Run(() =>
            {
                var data = Api.SendRequest<AccountsData>("accounts", null, Method.GET).Data;
                return Parallel.ForEach(data, element => { Accounts.Add(new Account(element)); });
            });

            taskList.Add(task);
            Task.WaitAll(taskList.ToArray());
        }

        /// <summary>
        ///     Get the API server time.
        /// </summary>
        public Time GetTime() => Api.SendRequest<Time>("time", null, Method.GET).Data;

    }
}
