namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sights", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Sights", "Longtitude", c => c.Double(nullable: false));
            DropColumn("dbo.Sights", "CoordinateX");
            DropColumn("dbo.Sights", "CoordinateY");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sights", "CoordinateY", c => c.Double(nullable: false));
            AddColumn("dbo.Sights", "CoordinateX", c => c.Double(nullable: false));
            DropColumn("dbo.Sights", "Longtitude");
            DropColumn("dbo.Sights", "Latitude");
        }
    }
}
