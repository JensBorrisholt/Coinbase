using System;
using System.Diagnostics;
using System.Net;
using Coinbase.ObjectModel;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Coinbase.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
		private const string ApiKey = "EGVDDCTWDcYsQKlI";
		private const string ApiSecretKey = "OyRpgMk41FxdVzwMaM2ZvxL61nvfWoiX";
		private WebProxy proxy = new WebProxy("http://localhost.:8888", false);
 
        [Test]
        [Explicit]
        public void integration_test_create_button()
        {
            var api = new CoinbaseApi(ApiKey, ApiSecretKey, useSandbox: true, proxy: proxy );

            var paymenRequest = new ButtonRequest
                {
                    Name = "Order Name",
                    Currency = Currency.USD,
                    Price = 79.99m,
                    Type = ButtonType.BuyNow,
                    Custom = "Custom_Order_Id",
                    Description = "Order Description",
                    Style = ButtonStyle.CustomLarge,
                    CallbackUrl = "http://www.bitarmory.com/callback",
                    CancelUrl = "http://www.bitarmory.com/cancel",
                    InfoUrl = "http://www.bitarmory.com/info",
                    SuccessUrl = "http://www.bitarmory.com/success",
                    ChoosePrice = false,
                    IncludeAddress = false,
                    IncludeEmail = false,
                    VariablePrice = false
                };

            var buttonResponse = api.RegisterButton( paymenRequest );

            if( buttonResponse.Success )
            {
                var redirectUrl = buttonResponse.GetCheckoutUrl();
                //Redirect the user to the URL to complete the
                //the purchase
                Console.WriteLine( redirectUrl );
            }
            else
            {
                //Something went wrong. Check errors and fix any issues.
                Debug.WriteLine( string.Join( ",", buttonResponse.Errors ) );
            }
            
            var o = api.CreateOrder( buttonResponse );
            o.Should().NotBeNull();

            buttonResponse.Button.Code.Should().NotBeNullOrEmpty();

            buttonResponse.Button.Price.Cents.Should().Be( 7999 );
                    
            buttonResponse.Success.Should().BeTrue();
            //http://www.bitarmory.com/cancel?order%5Bbutton%5D%5Bdescription%5D=Order+Description&order%5Bbutton%5D%5Bid%5D=ea607b144c6fc28ec289eea5acaaaf86&order%5Bbutton%5D%5Bname%5D=Order+Name&order%5Bbutton%5D%5Btype%5D=buy_now&order%5Bcreated_at%5D=2013-12-01+19%3A04%3A24+-0800&order%5Bcustom%5D=Custom_Order_Id&order%5Bid%5D=3LP5XUP7&order%5Breceive_address%5D=13uwaYfphxs51eN2DuhBJqimRJJ3UrYjSX&order%5Bstatus%5D=new&order%5Btotal_btc%5D%5Bcents%5D=7767029&order%5Btotal_btc%5D%5Bcurrency_iso%5D=BTC&order%5Btotal_native%5D%5Bcents%5D=7999&order%5Btotal_native%5D%5Bcurrency_iso%5D=USD&order%5Btransaction%5D=
            //http://www.bitarmory.com/cancel?
            //order[button][description]=Order Description
            //&order[button][id]=ea607b144c6fc28ec289eea5acaaaf86
            //&order[button][name]=Order Name
            //&order[button][type]=buy_now&order[created_at]=2013-12-01 19:04:24 -0800
            //&order[custom]=Custom_Order_Id
            //&order[id]=3LP5XUP7
            //&order[receive_address]=13uwaYfphxs51eN2DuhBJqimRJJ3UrYjSX
            //&order[status]=new&order[total_btc][cents]=7767029
            //&order[total_btc][currency_iso]=BTC
            //&order[total_native][cents]=7999
            //&order[total_native][currency_iso]=USD
            //&order[transaction]=
        }

        [Test]
        [Explicit]
        public void create_order_test()
        {
            var json =
                @"{""success"":true,""button"":{""code"":""d4d26483141bec5f551d4d8822fab6fa"",""type"":""buy_now"",""subscription?"":false,""repeat"":null,""style"":""custom_large"",""text"":""Pay With Bitcoin"",""name"":""Order Name"",""description"":""Order Description"",""custom"":""Custom_Order_Id"",""callback_url"":""http://www.bitarmory.com/callback"",""success_url"":""http://www.bitarmory.com/success"",""cancel_url"":""http://www.bitarmory.com/cancel"",""info_url"":""http://www.bitarmory.com/info"",""auto_redirect"":false,""auto_redirect_success"":false,""auto_redirect_cancel"":false,""price"":{""cents"":7999.0,""currency_iso"":""USD""},""variable_price"":false,""choose_price"":false,""include_address"":false,""include_email"":false}}";

            var b = JsonConvert.DeserializeObject<ButtonResponse>(json);
            var api = new CoinbaseApi();

            var o = api.CreateOrder(b);
            o.Should().NotBeNull();
        }

        [Test]
        [Explicit]
        public void create_refund_test()
        {
            // arrange
            var api = new CoinbaseApi(apiKey: ApiKey, apiSecret: ApiSecretKey, useSandbox:true, proxy: proxy);

            var refundOptions = new RefundOptions
                {
                    RefundIsoCurrency = Currency.BTC,
                    
                    //By default, refunds will be issued to the refund_address
                    //that is set on the order.
                    //Additionally, if you want to send the refund to a different
                    //bitcoin address other than the one that was in the original order
                    //set ExteranlRefundAddress property.  
                    //OPTIONAL:
                    ExternalRefundAddress = "BITCOIN_REFUND_ADDRESS"
                };

            var orderIdToRefund = "COINBASE_ORDER_ID";

            // act
            var refundResult = api.Refund(orderIdToRefund, refundOptions);

            if(refundResult.Success)
            {
                //The refund was successful
                var refundTxn = refundResult.Order.RefundTransaction;
            }
            
            // assert
            refundResult.Should().NotBeNull();
        }

        [Test]
        [Explicit]
        public void send_money_test()
        {
            // arrange
            var api = new CoinbaseApi(apiKey: ApiKey, apiSecret: ApiSecretKey, useSandbox:true, proxy: proxy);

            //Make a direct payment of BTC to another
            //bit coin address
            var pmtInBtc = new Payment()
                {
                    To = "BITCOIN_ADDRESS_OR_EMAIL",
                    Amount = 0.0m, // IN BTC
                    Notes = "MY_MESSAGE"
                };

            //Optionally, make an equivalent payment of $20 in USD in BTC
            //to the recipient.
            var pmtInUSD = new Payment()
                {
                    To = "BITCOIN_ADDRESS_OR_EMAIL",

                    Amount = null, //Don't use when using currency other than BTC

                    AmountString = 20.00m, // IN USD
                    AmountCurrencyIso = Currency.USD,
                    //InstantBuy parameter signals that if your account does 
                    //not currently have enough funds to cover the 
                    //amount, first purchase the difference with
                    //an instant buy, then send the bitcoin.
                    InstantBuy = true,
                };

            // act
            var response = api.SendMoney(pmtInBtc);

            if ( response.Errors != null)
            {
                //Some send money error
            }
            else if (response.Success)
            {
                //The send was successful
                var sendTxn = response.Transaction;
            }

            // assert
            response.Should().NotBeNull();
        }

        [Test]
        [Explicit]
        public void get_order_test()
        {
            // arrange
            var api = new CoinbaseApi(apiKey: ApiKey, apiSecret: ApiSecretKey, useSandbox: true, proxy: proxy);

            // act
            var orderResult = api.GetOrder("ORDER_ID_OR_CUSTOM");

            if (orderResult.Error != null)
            {
                //Some Error
            }
            else if (orderResult.Order.Status == Status.Completed)
            {
                //The request was successful
                var orderTxn = orderResult.Order;
            }

            // assert
            orderResult.Order.Should().NotBeNull();
        }

        [Test]
        [Explicit]
        public void get_order_with_refund_test()
        {
            // arrange
            var api = new CoinbaseApi(apiKey: ApiKey, apiSecret: ApiSecretKey, useSandbox: true, proxy: proxy);

            // act
            var orderResult = api.GetOrder("ORDER_ID_OR_CUSTOM");

            if (orderResult.Error != null)
            {
                //Some Error
            }
            else if (orderResult.Order.Status == Status.Completed)
            {
                //The request was successful
                var orderTxn = orderResult.Order;
            }

            // assert
            orderResult.Order.RefundTransaction.Should().NotBeNull();
        }
    }
}