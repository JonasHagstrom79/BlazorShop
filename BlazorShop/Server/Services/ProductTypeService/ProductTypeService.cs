﻿namespace BlazorShop.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;
        //Need the datacontext
        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false; //For instant update on the webbpage!
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            //get the product type that shall be updated from database
            var dbProductType = await _context.ProductTypes.FindAsync(productType.Id);
            if (dbProductType == null) 
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found"
                };
            }
            
            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypes(); //return product
        }
    }
}
