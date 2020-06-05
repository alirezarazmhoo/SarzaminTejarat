namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsOnlyForMultipation", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "IsOnlyForRetailer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsOnlyForRetailer");
            DropColumn("dbo.Products", "IsOnlyForMultipation");
        }
    }
}
