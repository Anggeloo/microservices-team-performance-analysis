using Microsoft.EntityFrameworkCore;

namespace microservices_team_performance_analysis.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Models.TeamPerformanceAnalysis> TeamPerformanceAnalysis { get; set; }
    }
}
