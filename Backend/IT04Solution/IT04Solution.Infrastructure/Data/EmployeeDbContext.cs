using IT04Solution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IT04Solution.Infrastructure.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee entity configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Occupation)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProfileImage)
                    .HasColumnType("bytea");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false);

                // Indexes
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Phone);
            });
        }
    }
}
