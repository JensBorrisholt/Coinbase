using System;
using Coinbase.ObjectModel;
using RestSharp;

namespace Coinbase
{
    public enum Currency { BCH, EUR, LTC, ETH, BTC };

    public class Account 
    {
        private readonly AccountData data;

        public string Id => data.Id;
        public string Name => data.Name;
        public bool IsPrimary => data.Primary;
        public string Type => data.Type;
        public Currency Currency => (Currency)Enum.Parse(typeof(Currency), data.Currency);
        public Balance Balance => data.Balance;
        public DateTime Created => data.CreatedAt;
        public DateTime Updated => data.UpdatedAt;
        public Balance NativeBalance => data.NativeBalance;
        public Transactions Transactions { get; }
        public Addresses Addresses { get; }
        private T SendRequest<T>(string endpoint) => CoinbaseApi.Instance.SendRequest<T>($"accounts/{Id}/{endpoint}", null, Method.GET).Data;

        public Account(AccountData data)
        {
            this.data = data;
            Transactions = SendRequest<Transactions>("transactions");
            Addresses = SendRequest<Addresses>("addresses");
        }
    }
}
