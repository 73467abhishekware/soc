using Microsoft.EntityFrameworkCore;

namespace SOC.Repository
{
    public class SOC_DBContext : DbContext
    {
        public SOC_DBContext(DbContextOptions<SOC_DBContext> options) : base(options)
        {
        }
    }
}
