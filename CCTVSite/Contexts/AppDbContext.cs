using Microsoft.EntityFrameworkCore;

namespace CCTVSite.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        
    }
}
