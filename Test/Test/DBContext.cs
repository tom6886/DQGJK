using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("Name=DBContext")
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<CabinetData> CabinetData { get; set; }
    }
}
