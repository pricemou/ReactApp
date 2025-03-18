using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class intiliser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id_client = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    adresse = table.Column<string>(type: "TEXT", nullable: false),
                    telephone = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    date_inscription = table.Column<DateTime>(type: "TEXT", nullable: false),
                    actif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id_client);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom_utilisateur = table.Column<string>(type: "TEXT", nullable: false),
                    mot_de_passe = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    est_administrateur = table.Column<bool>(type: "INTEGER", nullable: false),
                    date_creation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    derniere_connexion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.id_utilisateur);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
