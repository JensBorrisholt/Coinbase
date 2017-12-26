using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coinbase;


namespace Coinbase_Demoproject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching Coinbase data ....");
            var coinbase = new Coinbase.Coinbase();
            Console.WriteLine("");

            Console.WriteLine("=== Accounts ===");
            foreach (var account in coinbase.Accounts)
            {
                Console.WriteLine($"Name : {account.Name}");
                Console.WriteLine($"Crypto Balance: {account.Balance.Amount}");
                Console.WriteLine($"Native Balance : {account.NativeBalance.Amount}");
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("=== Exchange Rates ===");
            var rates = coinbase.ExchangeRates[Currency.BCH];
            foreach (var element in rates.Keys.Cast<string>())
                Console.WriteLine(element + " " + rates[element]);
            Console.WriteLine("");

            Console.WriteLine("");
            Console.WriteLine("=== User Information ===");
            Console.WriteLine($"Name: {coinbase.UserInformation.Name}");
            Console.WriteLine($"Country Name: {coinbase.UserInformation.Country.Name}");
            Console.WriteLine($"Native Currency: {coinbase.UserInformation.NativeCurrency}");

            var cutoffDate = new DateTime(2017, 12, 3);
            Console.WriteLine("");
            Console.WriteLine("=== PnL ===");
            Console.WriteLine($"PnL for {cutoffDate}");

            
            foreach(Currency currency in Enum.GetValues(typeof(Currency)) )
            {
                if( currency == Currency.EUR )
                    continue;

                var account = coinbase.Accounts.FirstOrDefault(a => a.Currency == currency);

                if ( account == null )
                    continue;

                Console.WriteLine($"Name : {account.Name}");
                Console.WriteLine($"PnL : {coinbase.PnL(currency, cutoffDate)}");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
