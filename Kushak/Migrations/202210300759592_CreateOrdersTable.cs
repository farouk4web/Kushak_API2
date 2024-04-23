namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateOrdersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 60),
                        DateOfCreate = c.DateTime(),
                        Phone = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        MoreAboutAddress = c.String(maxLength: 500),
                        Total = c.Decimal(precision: 18, scale: 2),
                        ShippingFee = c.Decimal(precision: 18, scale: 2),
                        GrandTotal = c.Decimal(precision: 18, scale: 2),
                        PaymentMethodId = c.Int(),
                        PaymentMethodFee = c.Decimal(precision: 18, scale: 2),
                        IsConfirmed = c.Boolean(nullable: false),
                        DateOfConfirmation = c.DateTime(),
                        IsShipping = c.Boolean(nullable: false),
                        DateOfShipping = c.DateTime(),
                        IsDelivered = c.Boolean(nullable: false),
                        DateOfDelivery = c.DateTime(),
                        BuyerId = c.String(maxLength: 128),
                        ShoppingCartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerId)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.BuyerId)
                .Index(t => t.ShoppingCartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShoppingCartId", "dbo.ShoppingCarts");
            DropForeignKey("dbo.Orders", "BuyerId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ShoppingCartId" });
            DropIndex("dbo.Orders", new[] { "BuyerId" });
            DropTable("dbo.Orders");
        }
    }
}
