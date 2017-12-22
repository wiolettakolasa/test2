namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressToSight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sights", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sights", "Address");
        }
    }
}
