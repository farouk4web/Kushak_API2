namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedMainRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'd963758b-69bd-42a0-b0fa-18de3f31e37c', N'Admins')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'32fbe1b0-39ea-45d8-9f2d-84e51df3bd4d', N'Owners')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'13a9c618-68e3-49c6-8909-667ab0a04741', N'Sellers')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c870d9e4-094c-486f-8e57-29f5de8fb3c2', N'ShippingStaff')
                ");
        }

        public override void Down()
        {
        }
    }
}
