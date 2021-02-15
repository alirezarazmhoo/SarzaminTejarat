namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FactorType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketerFactors", "FactorType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MarketerFactors", "FactorType");
        }
    }
}
