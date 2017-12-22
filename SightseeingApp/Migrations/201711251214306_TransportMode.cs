namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportMode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportModes", "Name", c => c.String());
            AddColumn("dbo.UserDatas", "TranspModeId", c => c.Byte(nullable: false));
            AddColumn("dbo.UserDatas", "TransportMode_Id", c => c.Int());
            CreateIndex("dbo.UserDatas", "TransportMode_Id");
            AddForeignKey("dbo.UserDatas", "TransportMode_Id", "dbo.TransportModes", "Id");
            DropColumn("dbo.TransportModes", "TransportModeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportModes", "TransportModeName", c => c.String());
            DropForeignKey("dbo.UserDatas", "TransportMode_Id", "dbo.TransportModes");
            DropIndex("dbo.UserDatas", new[] { "TransportMode_Id" });
            DropColumn("dbo.UserDatas", "TransportMode_Id");
            DropColumn("dbo.UserDatas", "TranspModeId");
            DropColumn("dbo.TransportModes", "Name");
        }
    }
}
