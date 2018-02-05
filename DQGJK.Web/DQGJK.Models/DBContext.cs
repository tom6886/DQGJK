using Microsoft.EntityFrameworkCore;

namespace DQGJK.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
    }
}
