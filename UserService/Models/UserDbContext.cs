using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UserService
{
    public partial class UserDbContext : DbContext
    {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=15433;Database=postgres;Username=postgres;Password=postgres");
            }
        }

        internal Role SaveChangesAsync(Func<object, bool> p) {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("role_pk");

                entity.ToTable("role");

                entity.Property(e => e.IdRole)
                    .ValueGeneratedNever()
                    .HasColumnName("idrole");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("user_pk");

                entity.ToTable("User");

                entity.Property(e => e.IdUser)
                    .ValueGeneratedNever()
                    .HasColumnName("iduser");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.IdRole).HasColumnName("idrole");

                entity.Property(e => e.IsReceiveNotifications).HasColumnName("isreceivenotifications");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("registrationdate");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("surname");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
