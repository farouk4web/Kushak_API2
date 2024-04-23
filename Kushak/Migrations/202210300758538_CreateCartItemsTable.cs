namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCartItemsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ShoppingCartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ShoppingCartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "ShoppingCartId", "dbo.ShoppingCarts");
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropIndex("dbo.CartItems", new[] { "ShoppingCartId" });
            DropIndex("dbo.CartItems", new[] { "ProductId" });
            DropTable("dbo.CartItems");
        }
    }
}
