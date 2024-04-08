using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.MSSQL.Migrations
{
    public partial class UserCountTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_UserAdded
ON dbo.AspNetUsers
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserCount INT;

    -- Get the current count
    SELECT @UserCount = Count FROM dbo.UserCounts;

    -- Increment the count
    SET @UserCount = @UserCount + 1;

    -- Update the count
    UPDATE dbo.UserCounts
    SET Count = @UserCount;
END;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_UserAdded;");
        }
    }
}
