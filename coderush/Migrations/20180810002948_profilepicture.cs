using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace coderush.Migrations
{
    public partial class profilepicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "UserProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "UserProfile");
        }
    }
}
