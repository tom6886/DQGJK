﻿using Microsoft.EntityFrameworkCore;

namespace DQGJK.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        //模型

        public DbSet<Guser> Guser { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Station> Station { get; set; }

        public DbSet<Cabinet> Cabinet { get; set; }

        public DbSet<CabinetData> CabinetData { get; set; }

        public DbSet<Area> Area { get; set; }


        //视图

        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<StationInfo> StationInfo { get; set; }
    }
}
