using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GoalMicroservice
{
    public partial class GoalDbContext : DbContext
    {
        public GoalDbContext()
        {
        }

        public GoalDbContext(DbContextOptions<GoalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<GoalTool> GoalTools { get; set; }
        public virtual DbSet<Tool> Tools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=15432;Database=postgres;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(e => e.IdGoal)
                    .HasName("goal_pk");

                entity.ToTable("goal");

                entity.Property(e => e.IdGoal)
                    .ValueGeneratedNever()
                    .HasColumnName("idgoal");

                entity.Property(e => e.DeadlineTimeStamp).HasColumnName("deadlinetimestamp");

                entity.Property(e => e.FinishedTimeStamp).HasColumnName("finishedtimestamp");

                entity.Property(e => e.IsAdminOnly).HasColumnName("isadminonly");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("text");
            });

            modelBuilder.Entity<GoalTool>(entity =>
            {
                entity.HasKey(e => new { e.IdGoal, e.IdTool })
                    .HasName("goaltool_pk");

                entity.ToTable("goaltool");

                entity.Property(e => e.IdGoal).HasColumnName("idgoal");

                entity.Property(e => e.IdTool).HasColumnName("idtool");

                entity.HasOne(d => d.IdGoalNavigation)
                    .WithMany(p => p.GoalTools)
                    .HasForeignKey(d => d.IdGoal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("table_4_goal");

                entity.HasOne(d => d.IdToolNavigation)
                    .WithMany(p => p.GoalTools)
                    .HasForeignKey(d => d.IdTool)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("table_4_tool");
            });

            modelBuilder.Entity<Tool>(entity =>
            {
                entity.HasKey(e => e.IdTool)
                    .HasName("tool_pk");

                entity.ToTable("tool");

                entity.Property(e => e.IdTool)
                    .ValueGeneratedNever()
                    .HasColumnName("idtool");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
