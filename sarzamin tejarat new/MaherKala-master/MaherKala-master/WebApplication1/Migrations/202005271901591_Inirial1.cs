namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inirial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditPaymentRequestAttempPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(),
                        creditPaymentRequestAttempId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditPaymentRequestAttemps", t => t.creditPaymentRequestAttempId, cascadeDelete: true)
                .Index(t => t.creditPaymentRequestAttempId);
            
            CreateTable(
                "dbo.CreditPaymentRequestAttemps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreditPaymentRequestAttempStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        AdminComment = c.String(),
                        MarketerUserId = c.Int(nullable: false),
                        CreditPayConditationsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditPayConditations", t => t.CreditPayConditationsId, cascadeDelete: true)
                .ForeignKey("dbo.MarketerUsers", t => t.MarketerUserId, cascadeDelete: true)
                .Index(t => t.MarketerUserId)
                .Index(t => t.CreditPayConditationsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditPaymentRequestAttemps", "MarketerUserId", "dbo.MarketerUsers");
            DropForeignKey("dbo.CreditPaymentRequestAttempPictures", "creditPaymentRequestAttempId", "dbo.CreditPaymentRequestAttemps");
            DropForeignKey("dbo.CreditPaymentRequestAttemps", "CreditPayConditationsId", "dbo.CreditPayConditations");
            DropIndex("dbo.CreditPaymentRequestAttemps", new[] { "CreditPayConditationsId" });
            DropIndex("dbo.CreditPaymentRequestAttemps", new[] { "MarketerUserId" });
            DropIndex("dbo.CreditPaymentRequestAttempPictures", new[] { "creditPaymentRequestAttempId" });
            DropTable("dbo.CreditPaymentRequestAttemps");
            DropTable("dbo.CreditPaymentRequestAttempPictures");
        }
    }
}
