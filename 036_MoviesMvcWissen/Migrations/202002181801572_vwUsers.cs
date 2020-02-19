namespace _036_MoviesMvcWissen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vwUsers : DbMigration
    {
        public override void Up()
        {
            string sql = "";
            sql += "DROP VIEW IF EXISTS dbo.vwUsers; ";
            Sql(sql);
            sql = "CREATE VIEW dbo.vwUsers ";
            sql += "AS ";
            sql += "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY u.[Id]), 0) as Id, u.[Id] as [UserId], u.[UserName], u.[Password], u.[Active], u.[RoleId], r.[Name] as [RoleName] ";
            sql += "FROM Users u INNER JOIN Roles r ON u.[RoleId] = r.[Id] ";
            Sql(sql);

        }
        
        public override void Down()
        {
        }
    }
}
