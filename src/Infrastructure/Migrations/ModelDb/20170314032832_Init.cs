using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Migrations.ModelDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    ModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassName = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => x.ModelId);
                });

            migrationBuilder.CreateTable(
                name: "ModelPropertyMeta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRequired = table.Column<bool>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true),
                    RuntimeModelMetaModelId = table.Column<int>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPropertyMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelPropertyMeta_Metas_RuntimeModelMetaModelId",
                        column: x => x.RuntimeModelMetaModelId,
                        principalTable: "Metas",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelPropertyMeta_RuntimeModelMetaModelId",
                table: "ModelPropertyMeta",
                column: "RuntimeModelMetaModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelPropertyMeta");

            migrationBuilder.DropTable(
                name: "Metas");
        }
    }
}
