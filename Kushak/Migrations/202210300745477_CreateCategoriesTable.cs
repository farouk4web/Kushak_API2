namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 1000),
                        SellerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SellerId)
                .Index(t => t.SellerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "SellerId", "dbo.AspNetUsers");
            DropIndex("dbo.Categories", new[] { "SellerId" });
            DropTable("dbo.Categories");
        }
    }
}
