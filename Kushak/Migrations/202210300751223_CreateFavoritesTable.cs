namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFavoritesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Favorites", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Favorites", "ProductId", "dbo.Products");
            DropIndex("dbo.Favorites", new[] { "UserId" });
            DropIndex("dbo.Favorites", new[] { "ProductId" });
            DropTable("dbo.Favorites");
        }
    }
}
