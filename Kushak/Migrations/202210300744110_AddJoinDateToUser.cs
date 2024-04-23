namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJoinDateToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "JoinDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "JoinDate");
        }
    }
}
