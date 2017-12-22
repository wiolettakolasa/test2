namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cost = c.Double(nullable: false),
                        Time = c.Double(nullable: false),
                        CategoryId = c.Byte(nullable: false),
                        TrasportModeId = c.Byte(nullable: false),
                        TransportMode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserDatas", "TransportMode_Id");
            CreateIndex("dbo.UserDatas", "CategoryId");
            AddForeignKey("dbo.UserDatas", "TransportMode_Id", "dbo.TransportModes", "Id");
            AddForeignKey("dbo.UserDatas", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
