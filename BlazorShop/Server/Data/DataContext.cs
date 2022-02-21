namespace BlazorShop.Server.Data
{
    public class DataContext : DbContext //for context
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        //adds to the database
        public DbSet<Product> Products { get; set; }
    }
}
