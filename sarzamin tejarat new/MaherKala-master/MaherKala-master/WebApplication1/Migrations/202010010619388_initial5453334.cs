namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5453334 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ShowonWebSite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ShowonWebSite");
        }
    }
}
