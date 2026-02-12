using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosUy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FirstSurname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastSurname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Ci = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Nickname = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "citext", maxLength: 255, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Acronym = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "citext", maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "citext", maxLength: 200, nullable: false),
                    Number = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    Nickname = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "citext", maxLength: 255, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Initials = table.Column<string>(type: "citext", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Categories = table.Column<HashSet<string>>(type: "text[]", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Editions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Initials = table.Column<string>(type: "citext", maxLength: 10, nullable: false),
                    From = table.Column<DateOnly>(type: "date", nullable: false),
                    To = table.Column<DateOnly>(type: "date", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Country = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "citext", maxLength: 200, nullable: false),
                    Number = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Editions_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Editions_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegisterTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quota = table.Column<int>(type: "integer", nullable: false),
                    Used = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EditionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterTypes_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sponsorships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    EditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsorships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sponsorships_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sponsorships_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sponsorships_RegisterTypes_RegisterTypeId",
                        column: x => x.RegisterTypeId,
                        principalTable: "RegisterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: false),
                    Quota = table.Column<int>(type: "integer", nullable: false),
                    Used = table.Column<int>(type: "integer", nullable: false),
                    IsAutoApplied = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    EditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SponsorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_RegisterTypes_RegisterTypeId",
                        column: x => x.RegisterTypeId,
                        principalTable: "RegisterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Sponsorships_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sponsorships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    EditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisterTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_RegisterTypes_RegisterTypeId",
                        column: x => x.RegisterTypeId,
                        principalTable: "RegisterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Ci",
                table: "Clients",
                column: "Ci",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Nickname",
                table: "Clients",
                column: "Nickname",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_Country_City_Street_Number_Floor_To",
                table: "Editions",
                columns: new[] { "Country", "City", "Street", "Number", "Floor", "To" },
                filter: "\"State\" = 'CANCELLED'");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_EventId",
                table: "Editions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_EventId_State",
                table: "Editions",
                columns: new[] { "EventId", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Editions_Initials",
                table: "Editions",
                column: "Initials",
                unique: true,
                filter: "\"State\" = 'CANCELLED'");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_InstitutionId",
                table: "Editions",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_InstitutionId_State",
                table: "Editions",
                columns: new[] { "InstitutionId", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Editions_Name",
                table: "Editions",
                column: "Name",
                unique: true,
                filter: "\"State\" = 'CANCELLED'");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_State",
                table: "Editions",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Active",
                table: "Event",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Initials",
                table: "Event",
                column: "Initials",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Event_InstitutionId",
                table: "Event",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name",
                table: "Event",
                column: "Name",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_Acronym",
                table: "Institutions",
                column: "Acronym",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_Country_City_Street_Number_Floor",
                table: "Institutions",
                columns: new[] { "Country", "City", "Street", "Number", "Floor" },
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_Email",
                table: "Institutions",
                column: "Email",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_Nickname",
                table: "Institutions",
                column: "Nickname",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_Url",
                table: "Institutions",
                column: "Url",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ClientId",
                table: "Registers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ClientId_EditionId",
                table: "Registers",
                columns: new[] { "ClientId", "EditionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Registers_EditionId",
                table: "Registers",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_RegisterTypeId",
                table: "Registers",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_VoucherId",
                table: "Registers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_EditionId",
                table: "RegisterTypes",
                column: "EditionId",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_Name",
                table: "RegisterTypes",
                column: "Name",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_EditionId",
                table: "Sponsorships",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_InstitutionId",
                table: "Sponsorships",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_InstitutionId_EditionId",
                table: "Sponsorships",
                columns: new[] { "InstitutionId", "EditionId" },
                filter: "\"Active\" = 'true'");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_RegisterTypeId",
                table: "Sponsorships",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_Code",
                table: "Vouchers",
                column: "Code",
                filter: "\"State\" = 'AVAILABLE'");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_EditionId",
                table: "Vouchers",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_RegisterTypeId",
                table: "Vouchers",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_SponsorId",
                table: "Vouchers",
                column: "SponsorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Sponsorships");

            migrationBuilder.DropTable(
                name: "RegisterTypes");

            migrationBuilder.DropTable(
                name: "Editions");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Institutions");
        }
    }
}
