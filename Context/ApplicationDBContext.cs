using ASPNETAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETAuthAPI.Context {
    public class ApplicationDBContext: DbContext {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options) {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
