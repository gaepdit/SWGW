using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWGW.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class SWGW_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermitActions_Permits_PermitId1",
                table: "PermitActions");

            migrationBuilder.DropIndex(
                name: "IX_PermitActions_PermitId1",
                table: "PermitActions");

            migrationBuilder.DropColumn(
                name: "PermitId1",
                table: "PermitActions");

            migrationBuilder.RenameColumn(
                name: "ClosedDate",
                table: "Permits",
                newName: "PermitClosedDate");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Permits",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "PermitClosed",
                table: "Permits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermitId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(245)", maxLength: 245, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UploadedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsImage = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_AspNetUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_Permits_PermitId",
                        column: x => x.PermitId,
                        principalTable: "Permits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_PermitId",
                table: "PermitActions",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_PermitId",
                table: "Attachment",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UploadedById",
                table: "Attachment",
                column: "UploadedById");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitActions_Permits_PermitId",
                table: "PermitActions",
                column: "PermitId",
                principalTable: "Permits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermitActions_Permits_PermitId",
                table: "PermitActions");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_PermitActions_PermitId",
                table: "PermitActions");

            migrationBuilder.DropColumn(
                name: "PermitClosed",
                table: "Permits");

            migrationBuilder.RenameColumn(
                name: "PermitClosedDate",
                table: "Permits",
                newName: "ClosedDate");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Permits",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "PermitId1",
                table: "PermitActions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PermitActions_PermitId1",
                table: "PermitActions",
                column: "PermitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PermitActions_Permits_PermitId1",
                table: "PermitActions",
                column: "PermitId1",
                principalTable: "Permits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
