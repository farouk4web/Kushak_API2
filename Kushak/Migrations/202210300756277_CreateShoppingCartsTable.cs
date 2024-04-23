namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateShoppingCartsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        BuyerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerId)
                .Index(t => t.BuyerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingCarts", "BuyerId", "dbo.AspNetUsers");
            DropIndex("dbo.ShoppingCarts", new[] { "BuyerId" });
            DropTable("dbo.ShoppingCarts");
        }
    }
}
