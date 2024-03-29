﻿using BlazorShop.Server.Seed;

namespace BlazorShop.Server.Data
{
    public class DataContext : DbContext //for context
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //Seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Seed();            
        }

        //adds to the database(POCO)
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; } //Add migration whnever we add something here
    }
}
