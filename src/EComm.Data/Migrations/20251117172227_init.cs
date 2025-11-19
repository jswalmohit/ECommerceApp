using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create UserDetails table
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'UserDetails' AND type = 'U')
        BEGIN
            CREATE TABLE [UserDetails](
                [Id] INT IDENTITY(1,1) NOT NULL,
                [LoginId] NVARCHAR(50) NOT NULL,
                [Email] NVARCHAR(MAX) NOT NULL,
                [FullName] NVARCHAR(MAX) NOT NULL,
                CONSTRAINT [PK_UserDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
            )
        END
    ");

            // Create UserCred table
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'UserCred' AND type = 'U')
        BEGIN
            CREATE TABLE [UserCred](
                [Id] INT NOT NULL,
                [Password] NVARCHAR(MAX) NOT NULL,
                CONSTRAINT [PK_UserCred] PRIMARY KEY CLUSTERED ([Id] ASC),
                CONSTRAINT [FK_UserCred_UserDetails_Id] FOREIGN KEY ([Id])
                    REFERENCES [UserDetails]([Id]) ON DELETE CASCADE
            )
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF OBJECT_ID('UserCred', 'U') IS NOT NULL
            DROP TABLE [UserCred];
    ");

            migrationBuilder.Sql(@"
        IF OBJECT_ID('UserDetails', 'U') IS NOT NULL
            DROP TABLE [UserDetails];
    ");
        }
    }
}
