namespace SightseeingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateCategories : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Categories (Id, CategoryName) VALUES (1, 'History')");
            Sql("INSERT INTO Categories (Id, CategoryName) VALUES (2, 'Art')");
            Sql("INSERT INTO Categories (Id, CategoryName) VALUES (3, 'Cuiscine')");
            Sql("INSERT INTO Categories (Id, CategoryName) VALUES (4, 'Nature')");
            Sql("INSERT INTO Categories (Id, CategoryName) VALUES (5, 'Entertainment')");
        }
        
        public override void Down()
        {
        }
    }
}
