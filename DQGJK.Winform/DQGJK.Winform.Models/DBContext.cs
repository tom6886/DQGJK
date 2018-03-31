using System.Data.Entity;

namespace DQGJK.Winform.Models
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("Name=DBContext")
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Station> Station { get; set; }

        public DbSet<Cabinet> Cabinet { get; set; }

        public DbSet<CabinetData> CabinetData { get; set; }

        public DbSet<Operate> Operate { get; set; }

        public DbSet<DeviceOperate> DeviceOperate { get; set; }
    }
}
