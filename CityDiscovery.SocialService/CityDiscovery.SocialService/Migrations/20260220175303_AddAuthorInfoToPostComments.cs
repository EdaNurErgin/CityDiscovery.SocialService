using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityDiscovery.SocialService.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorInfoToPostComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorAvatarUrl",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorUserName",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorAvatarUrl",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "AuthorUserName",
                table: "PostComments");
        }
    }
}
