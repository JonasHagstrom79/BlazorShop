﻿namespace BlazorShop.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        //we nedd acess to the databas so add the ctor
        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponseDto>>
            {
                Data = new List<CartProductResponseDto>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products //gets Products from DB
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue; //if nothing to add continue to product variants
                }

                var productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == item.ProductId
                    && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                //If we get all information we can create our cartProduct
                var cartProduct = new CartProductResponseDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId
                };
                result.Data.Add(cartProduct); //To the list
            }
            return result;
        }
    }
}
