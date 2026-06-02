using System;
using IT04Solution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            // DateTime converter to ensure all DateTime values are treated as UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

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

                entity.Property(e => e.BirthDay)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(dateTimeConverter);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(dateTimeConverter);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(dateTimeConverter);

                // Indexes
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Phone);
            });
        }
    }
}
