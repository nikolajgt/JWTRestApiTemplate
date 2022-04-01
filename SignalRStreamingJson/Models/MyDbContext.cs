using Microsoft.EntityFrameworkCore;

namespace SignalRStreamingJson.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MockDataTable> MockTable { get; set; }

        public MyDbContext(DbContextOptions options) : base(options) { }
    }
}
