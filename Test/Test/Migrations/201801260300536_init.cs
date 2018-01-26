namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CabinetData",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 64),
                        Code = c.String(nullable: false, maxLength: 64),
                        CreateTime = c.DateTime(nullable: false),
                        Humidity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Temperature = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RelayOne = c.Int(nullable: false),
                        RelayTwo = c.Int(nullable: false),
                        HumidityAlarm = c.Int(nullable: false),
                        TemperatureAlarm = c.Int(nullable: false),
                        Dehumidify = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CabinetData");
        }
    }
}
