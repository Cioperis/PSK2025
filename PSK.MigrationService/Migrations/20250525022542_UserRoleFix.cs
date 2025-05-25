using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSK.MigrationService.Migrations
{
    public partial class UserRoleFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the default before changing type
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Role\" DROP DEFAULT;");
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Role\" TYPE integer USING \"Role\"::integer;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to text and optionally add default back
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Role\" TYPE text USING \"Role\"::text;");
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Role\" SET DEFAULT '';");
        }
    }
}