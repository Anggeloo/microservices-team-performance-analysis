using Microsoft.EntityFrameworkCore;

namespace microservices_raw_material_management.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Models.RawMaterialManagement> RawMaterialManagement { get; set; }
    }
}
