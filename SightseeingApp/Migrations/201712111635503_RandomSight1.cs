namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RandomSight1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RandomSights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Cost = c.Double(nullable: false),
                        Time = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longtitude = c.Double(nullable: false),
                        CategoryId = c.Byte(nullable: false),
                        Attractivenes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RandomSights", "CategoryId", "dbo.Categories");
            DropIndex("dbo.RandomSights", new[] { "CategoryId" });
            DropTable("dbo.RandomSights");
        }
    }
}
