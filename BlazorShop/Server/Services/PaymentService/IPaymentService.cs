using Stripe.Checkout;

namespace BlazorShop.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        //sesson is of typ stripe checkout
        Task<Session> CreateCheckoutSession();
        //for stripe webhook
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}
