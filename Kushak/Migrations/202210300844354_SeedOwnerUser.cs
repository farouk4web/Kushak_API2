namespace Kushak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedOwnerUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FullName], [ProfileImageSrc], [JoinDate]) VALUES (N'f79e4da2-278f-439f-8b7f-6d1b9fc49c73', N'farouk@kushak.com', 0, N'ADlh7EG1rBuSRDyMC0HS5n7Uzk7yjor2/dpisK0tNjVwjFVsHhPE6ACbAmubSyjOFA==', N'b6ce3106-a790-462f-bc4f-79ba9e3af816', NULL, 0, 0, NULL, 0, 0, N'farouk@kushak.com', N'Farouk Abdelhamid', N'/Uploads/Users/user.png', N'2022-10-30 08:43:19')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f79e4da2-278f-439f-8b7f-6d1b9fc49c73', N'32fbe1b0-39ea-45d8-9f2d-84e51df3bd4d')
                ");
        }

        public override void Down()
        {
        }
    }
}
