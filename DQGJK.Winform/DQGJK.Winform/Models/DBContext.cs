using System.Data.Entity;

namespace DQGJK.Winform
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
    }
}
