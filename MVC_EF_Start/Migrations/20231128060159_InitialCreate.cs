using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_EF_Start.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DRegions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRegions", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "DCounties",
                columns: table => new
                {
                    CountyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCounties", x => x.CountyID);
                    table.ForeignKey(
                        name: "FK_DCounties_DRegions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "DRegions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DMakes",
                columns: table => new
                {
                    MakeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DMakes", x => x.MakeID);
                    table.ForeignKey(
                        name: "FK_DMakes_DCounties_CountyID",
                        column: x => x.CountyID,
                        principalTable: "DCounties",
                        principalColumn: "CountyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DModels",
                columns: table => new
                {
                    ModelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountyID = table.Column<int>(type: "int", nullable: false),
                    DMakeMakeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DModels", x => x.ModelID);
                    table.ForeignKey(
                        name: "FK_DModels_DCounties_CountyID",
                        column: x => x.CountyID,
                        principalTable: "DCounties",
                        principalColumn: "CountyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DModels_DMakes_DMakeMakeID",
                        column: x => x.DMakeMakeID,
                        principalTable: "DMakes",
                        principalColumn: "MakeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DVehicles",
                columns: table => new
                {
                    VIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MakeID = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelID = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    CountyID = table.Column<int>(type: "int", nullable: false),
                    DModelModelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DVehicles", x => x.VIN);
                    table.ForeignKey(
                        name: "FK_DVehicles_DCounties_CountyID",
                        column: x => x.CountyID,
                        principalTable: "DCounties",
                        principalColumn: "CountyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DVehicles_DModels_DModelModelID",
                        column: x => x.DModelModelID,
                        principalTable: "DModels",
                        principalColumn: "ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DCounties_RegionID",
                table: "DCounties",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_DMakes_CountyID",
                table: "DMakes",
                column: "CountyID");

            migrationBuilder.CreateIndex(
                name: "IX_DModels_CountyID",
                table: "DModels",
                column: "CountyID");

            migrationBuilder.CreateIndex(
                name: "IX_DModels_DMakeMakeID",
                table: "DModels",
                column: "DMakeMakeID");

            migrationBuilder.CreateIndex(
                name: "IX_DVehicles_CountyID",
                table: "DVehicles",
                column: "CountyID");

            migrationBuilder.CreateIndex(
                name: "IX_DVehicles_DModelModelID",
                table: "DVehicles",
                column: "DModelModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DVehicles");

            migrationBuilder.DropTable(
                name: "DModels");

            migrationBuilder.DropTable(
                name: "DMakes");

            migrationBuilder.DropTable(
                name: "DCounties");

            migrationBuilder.DropTable(
                name: "DRegions");
        }
    }
}
