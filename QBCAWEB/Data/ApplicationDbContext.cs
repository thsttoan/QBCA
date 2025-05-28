using Microsoft.EntityFrameworkCore;
using QBCAWEB.Models; // Đảm bảo namespace này đúng

namespace QBCAWEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; } // << THÊM DÒNG NÀY >>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho User Entity (ví dụ)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.HasIndex(e => e.Email).IsUnique(); // Nếu Email là unique

                // Cấu hình mối quan hệ với Role
                entity.HasOne(d => d.UserRole)
                      .WithMany() // Nếu Role không có ICollection<User> Users
                                  // .WithMany(p => p.Users) // Nếu Role có ICollection<User> Users
                      .HasForeignKey(d => d.RoleID)
                      .OnDelete(DeleteBehavior.ClientSetNull); // Hoặc Restrict
            });

            // Cấu hình cho Role Entity (ví dụ)
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleID);
                entity.Property(e => e.RoleID).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.RoleName).IsUnique();
            });
        }
    }
}