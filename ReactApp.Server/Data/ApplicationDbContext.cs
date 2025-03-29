using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Models;
using System.Text;

namespace ReactApp.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
            {
            }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<VueUtilisateurActivites> Vue_Utilisateur_Activites { get; set; }
        public DbSet<UtilisateursClients> UtilisateursClients { get; set; } 
        public DbSet<Decodeur> Decodeurs {get ; set;}
        public DbSet<JournalOperation> JournalOperations {get ; set;}
        public DbSet<TypesOperation> TypesOperations {get ; set;}
        public DbSet<DecodeurContenu> DecodeurContenu {get ; set;}
        public DbSet<Contenu> Contenus {get ; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // Définir la clé primaire composite pour la table UtilisateursClients
        modelBuilder.Entity<UtilisateursClients>()
            .HasKey(uc => new { uc.id_utilisateur, uc.id_client });

        // Définir les relations
        

            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id_Client);

            modelBuilder.Entity<Utilisateur>()
                .HasKey(u => u.id_utilisateur);

            modelBuilder.Entity<VueUtilisateurActivites>()
                .HasKey(u => u.Id_Utilisateur);

            modelBuilder.Entity<Decodeur>()
            .HasOne(d => d.Client)  
            .WithMany() 
            .HasForeignKey(d => d.id_client);

            modelBuilder.Entity<TypesOperation>()
                .HasKey(u => u.id_type_operation);

            modelBuilder.Entity<JournalOperation>()
                .HasKey(u => u.id_operation);
            
            modelBuilder.Entity<DecodeurContenu>()
            .HasKey(dc => new { dc.id_decodeur, dc.id_contenu });
            
            modelBuilder.Entity<Contenu>()
                .HasKey(u => u.id_contenu);

            // Configurations supplémentaires pour les relations et les index...
        }

       
    }
}
