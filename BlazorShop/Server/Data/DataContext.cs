using BlazorShop.Server.Seed;

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

        //adds to the database
        public DbSet<Product> Products { get; set; }
    }
}
