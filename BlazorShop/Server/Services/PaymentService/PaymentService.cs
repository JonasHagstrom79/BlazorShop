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

        const string secret2 = ""; //Emtpy as for now while not needing the api webhook 
        //string secret = Test(_config);

        //The Cart, Order and authentication for stripe
        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService, IConfiguration config /*, PaymentService moviesConfig, WebApiOptions webApiOptions*/)
        {
            //Test(_config);
            StripeConfiguration.ApiKey = config["Appsettings:StripeApiKey"];         
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _config = config;            
        }

        public string Test(IConfiguration _config) 
        {
            string secret = $"{_config["Appsettings:StripeWeebHook"]}";
            //var webHook = config["Appsettings:StripeWeebHook"];
            return secret;
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
                        Name = product.Title, //The email NOT WORKING
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
                    //"card",
                    "bancontact",
                    "card",
                    "eps",
                    "giropay",
                    "ideal",
                    "p24",
                    "sepa_debit",
                    "sofort",
                    "klarna",
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

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request/*, IConfiguration config*/)
        {
            //New stream reader, this comes from stripe
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                //First we get a stripe event
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret2//Test(config) //The webhook original was secret
                    );
                //We want the checkout session completet event from stripe
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    //We create a new session
                    var session = stripeEvent.Data.Object as Session;
                    //This gives us the custom user e-mail-adress
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }
                return new ServiceResponse<bool> { Data = true };
                
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };                
            }

        }
    }
}
