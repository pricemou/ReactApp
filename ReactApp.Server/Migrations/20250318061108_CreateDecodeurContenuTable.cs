using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateDecodeurContenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contenus",
                columns: table => new
                {
                    id_contenu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Categorie = table.Column<string>(type: "TEXT", nullable: false),
                    Actif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contenus", x => x.id_contenu);
                });

            migrationBuilder.CreateTable(
                name: "Decodeurs",
                columns: table => new
                {
                    id_decodeur = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numero_serie = table.Column<string>(type: "TEXT", nullable: false),
                    modele = table.Column<string>(type: "TEXT", nullable: false),
                    id_client = table.Column<int>(type: "INTEGER", nullable: false),
                    adresse_ip = table.Column<string>(type: "TEXT", nullable: false),
                    mot_de_passe = table.Column<string>(type: "TEXT", nullable: false),
                    etat = table.Column<string>(type: "TEXT", nullable: false),
                    date_installation = table.Column<DateTime>(type: "TEXT", nullable: true),
                    derniere_mise_a_jour = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decodeurs", x => x.id_decodeur);
                    table.ForeignKey(
                        name: "FK_Decodeurs_Clients_id_client",
                        column: x => x.id_client,
                        principalTable: "Clients",
                        principalColumn: "Id_Client",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalOperations",
                columns: table => new
                {
                    id_operation = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_decodeur = table.Column<int>(type: "INTEGER", nullable: false),
                    id_type_operation = table.Column<int>(type: "INTEGER", nullable: false),
                    id_utilisateur = table.Column<int>(type: "INTEGER", nullable: false),
                    date_debut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    date_fin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    statut = table.Column<string>(type: "TEXT", nullable: false),
                    details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalOperations", x => x.id_operation);
                });

            migrationBuilder.CreateTable(
                name: "TypesOperations",
                columns: table => new
                {
                    id_type_operation = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    temps_execution_moyen = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOperations", x => x.id_type_operation);
                });

            migrationBuilder.CreateTable(
                name: "DecodeurContenus",
                columns: table => new
                {
                    id_decodeur = table.Column<int>(type: "INTEGER", nullable: false),
                    id_contenur = table.Column<int>(type: "INTEGER", nullable: false),
                    Contenuid_contenu = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecodeurContenus", x => new { x.id_decodeur, x.id_contenur });
                    table.ForeignKey(
                        name: "FK_DecodeurContenus_Contenus_Contenuid_contenu",
                        column: x => x.Contenuid_contenu,
                        principalTable: "Contenus",
                        principalColumn: "id_contenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecodeurContenus_Decodeurs_id_decodeur",
                        column: x => x.id_decodeur,
                        principalTable: "Decodeurs",
                        principalColumn: "id_decodeur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DecodeurContenus_Contenuid_contenu",
                table: "DecodeurContenus",
                column: "Contenuid_contenu");

            migrationBuilder.CreateIndex(
                name: "IX_Decodeurs_id_client",
                table: "Decodeurs",
                column: "id_client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecodeurContenus");

            migrationBuilder.DropTable(
                name: "JournalOperations");

            migrationBuilder.DropTable(
                name: "TypesOperations");

            migrationBuilder.DropTable(
                name: "Contenus");

            migrationBuilder.DropTable(
                name: "Decodeurs");
        }
    }
}
