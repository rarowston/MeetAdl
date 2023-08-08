using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetAdl.Migrations
{
    /// <inheritdoc />
    public partial class grouppermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserGroupPermissions",
                table: "GroupMembers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGroupPermissions",
                table: "GroupMembers");
        }
    }
}
