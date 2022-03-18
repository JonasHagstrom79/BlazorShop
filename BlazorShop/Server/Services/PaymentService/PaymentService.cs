using Stripe;
using Stripe.Checkout;

namespace BlazorShop.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config; //for the API-KEY IMPORTANT!!!
        

        //The Cart, Order and authentication for stripe
        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService, IConfiguration config)
        {
            //OnGet();
            StripeConfiguration.ApiKey = "";
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _config = config;
        }

        public void OnGet() 
        {
            var key2 = _config.GetSection("Appsettings: StripeApiKey").Value;
            //string key = _config.GetConnectionString("Appsettings:StripeApiKey");
            //String key1 = _config["Appsettings:StripeApiKey"].ToString();
            //var key = _config.GetSection("c").ToString();
            //var key = _config["Appsettings:StripeApiKey"].ToString();
            //StripeConfiguration.ApiKey = key;
            //StripeConfiguration.ApiKey = _config.GetSection("Appsettings:StripeApiKey").ToString();
            //StripeConfiguration.ApiKey = $"{_config["Appsettings:StripeApiKey"]}";
            //var key = 

            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            //    .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            //Create line items=seen on the checkout
            var lineItem = new List<SessionLineItemOptions>();
            //Iterate through the products and add new line-items
            products.ForEach(product => lineItem.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100, //Important(*100) this is how stripe works here
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity,
            }));
            //The checkout-session
            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItem,
                Mode = "payment", //One-time Payment
                SuccessUrl = "https://localhost:7206/order-success", //added /order-success, will have to change when going live
                CancelUrl = "https://localhost:7206/cart" //Go back to our cart
            };

            var service = new SessionService();
            Session session = service.Create(options); //The controller grabs the session and returns the url
            return session;
        }
    }
}
