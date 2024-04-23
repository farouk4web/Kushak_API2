namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReviewsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StarsCount = c.Byte(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 500),
                        DateOfCreate = c.DateTime(),
                        ProductId = c.Int(nullable: false),
                        BuyerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BuyerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Reviews", "BuyerId", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "BuyerId" });
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropTable("dbo.Reviews");
        }
    }
}
