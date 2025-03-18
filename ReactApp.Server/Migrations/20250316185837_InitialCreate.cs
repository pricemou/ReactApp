using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_creation",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "derniere_connexion",
                table: "Utilisateurs");

            migrationBuilder.RenameColumn(
                name: "telephone",
                table: "Clients",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "nom",
                table: "Clients",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clients",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "date_inscription",
                table: "Clients",
                newName: "Date_Inscription");

            migrationBuilder.RenameColumn(
                name: "adresse",
                table: "Clients",
                newName: "Adresse");

            migrationBuilder.RenameColumn(
                name: "actif",
                table: "Clients",
                newName: "Actif");

            migrationBuilder.RenameColumn(
                name: "id_client",
                table: "Clients",
                newName: "Id_Client");

            migrationBuilder.CreateTable(
                name: "UtilisateursClients",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_client = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateursClients", x => x.id_utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Vue_Utilisateur_Activites",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom_utilisateur = table.Column<string>(type: "TEXT", nullable: false),
                    mot_de_passe = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    nombre_decodeurs = table.Column<int>(type: "INTEGER", nullable: false),
                    nombre_operations = table.Column<int>(type: "INTEGER", nullable: false),
                    est_administrateur = table.Column<bool>(type: "INTEGER", nullable: false),
                    nom_client = table.Column<string>(type: "TEXT", nullable: false),
                    adresse_client = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vue_Utilisateur_Activites", x => x.id_utilisateur);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilisateursClients");

            migrationBuilder.DropTable(
                name: "Vue_Utilisateur_Activites");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Clients",
                newName: "telephone");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Clients",
                newName: "nom");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clients",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Date_Inscription",
                table: "Clients",
                newName: "date_inscription");

            migrationBuilder.RenameColumn(
                name: "Adresse",
                table: "Clients",
                newName: "adresse");

            migrationBuilder.RenameColumn(
                name: "Actif",
                table: "Clients",
                newName: "actif");

            migrationBuilder.RenameColumn(
                name: "Id_Client",
                table: "Clients",
                newName: "id_client");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_creation",
                table: "Utilisateurs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "derniere_connexion",
                table: "Utilisateurs",
                type: "TEXT",
                nullable: true);
        }
    }
}
