using Microsoft.EntityFrameworkCore;

namespace DQGJK.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Guser> Guser { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Station> Station { get; set; }

        public DbSet<Cabinet> Cabinet { get; set; }

        public DbSet<CabinetData> CabinetData { get; set; }

        public DbSet<Area> Area { get; set; }

        public DbSet<UserInfo> UserInfo { get; set; }   //视图
    }
}
