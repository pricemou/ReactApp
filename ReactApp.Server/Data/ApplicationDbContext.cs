using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasKey(c => c.id_client);

            modelBuilder.Entity<Utilisateur>()
                .HasKey(u => u.id_utilisateur);

            // Configurations supplémentaires pour les relations et les index...
        }
    }
}
