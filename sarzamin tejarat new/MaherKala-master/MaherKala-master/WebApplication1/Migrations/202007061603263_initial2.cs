namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketerUsers", "CanCheckPaymentGoldPlan", c => c.Boolean(nullable: false));
            AddColumn("dbo.MarketerUsers", "CanCheckPaymentSilverPlan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MarketerUsers", "CanCheckPaymentSilverPlan");
            DropColumn("dbo.MarketerUsers", "CanCheckPaymentGoldPlan");
        }
    }
}
