using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Data
{
    public class GatewayDbContext : DbContext
    {

        public GatewayDbContext(DbContextOptions<GatewayDbContext> options)
            : base(options)
        {
        }

    }
}
