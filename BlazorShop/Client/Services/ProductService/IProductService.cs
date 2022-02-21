﻿namespace BlazorShop.Client.Services.ProductService
{
    public interface IProductService
    {
        //The productlist itself
        List<Product> Products { get; set; }
        Task GetProducts();
        Task<ServiceResponse<Product>> GetProduct(int productId);
    }
}
