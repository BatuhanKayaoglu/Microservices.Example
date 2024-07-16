using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entities;

namespace Order.API.Models
{
    public class OrderAPIDbContext:DbContext
    {
        public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options)
               : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connStr = "Server=BATUHAN\\SQLEXPRESS;Initial Catalog=MicroservicesExample;integrated security=True;multipleactiveresultsets=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connStr);
            }

        }


        public DbSet<API.Models.Entities.Order> Orders { get; set; }        
        public DbSet<OrderItem> OrderItems { get; set; } 
    }



}
