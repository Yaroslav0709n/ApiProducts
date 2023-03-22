using Microsoft.EntityFrameworkCore;

namespace WebApiProducts.Data
{
    public class ApiDbContext:DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<ProductItem> Products { get; set; }
    }
}
