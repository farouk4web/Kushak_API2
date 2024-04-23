namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileImageSrcToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfileImageSrc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfileImageSrc");
        }
    }
}
