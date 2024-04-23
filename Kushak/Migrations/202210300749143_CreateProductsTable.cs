namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitsInStore = c.Int(nullable: false),
                        AvailableToSale = c.Boolean(nullable: false),
                        ShowInSlider = c.Boolean(nullable: false),
                        ImageSrc = c.String(),
                        CountOfSale = c.Int(),
                        StarsCount = c.Double(),
                        CategoryId = c.Int(nullable: false),
                        SellerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SellerId)
                .Index(t => t.CategoryId)
                .Index(t => t.SellerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SellerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "SellerId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.Products");
        }
    }
}
