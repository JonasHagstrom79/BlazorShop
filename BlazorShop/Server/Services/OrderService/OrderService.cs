using System.Security.Claims;

namespace BlazorShop.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;


        //implement the datacontext, the cartservice and the http-context accessor
        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponseDto>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponseDto>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems) //include the order items
                .ThenInclude(oi => oi.Product) //from that include the products
                .Where(o => o.UserId == _authService.GetUserId()) //we filter this by the userId
                .OrderByDescending(o => o.OrderDate) //latest order on top
                .ToListAsync();

            //the order response
            var orderResponse = new List<OrderOverviewResponseDto>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponseDto //iterate throug our orders, for every order ve add a new orderOverViewResponse
            { 
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $"{o.OrderItems.Count -1} more.." :
                    o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            })); 

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            //Get the products that is in the cart of the auth. user
            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>(); //New list and add the products
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));
            //creates a new order
            var order = new Order
            {
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice, //from above
                OrderItems = orderItems //from above
            };

            _context.Orders.Add(order);
            //removes the items in the cart
            _context.CartItems.RemoveRange(_context.CartItems
                .Where(ci => ci.UserId == _authService.GetUserId()));
            
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
