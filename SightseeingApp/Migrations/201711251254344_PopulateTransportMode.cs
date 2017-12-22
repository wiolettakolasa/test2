namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTransportMode : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT TransportModes ON");
            Sql("INSERT INTO TransportModes (Id, TransportModeName) VALUES (1, 'Driving')");
            Sql("INSERT INTO TransportModes (Id, TransportModeName) VALUES (2, 'Walking')");
            Sql("INSERT INTO TransportModes (Id, TransportModeName) VALUES (3, 'Transit')");
            Sql("SET IDENTITY_INSERT TransportModes OFF");
        }
        
        public override void Down()
        {
        }
    }
}
