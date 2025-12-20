using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class TurnDbContext : DbContext
    {

        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }

        public TurnDbContext(DbContextOptions<TurnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Professional>(entity =>
            {
                entity.ToTable("professionals");

                entity.HasKey(x => x.ProfessionalId);

                entity.Property(x => x.FullName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.Specialty)
                      .HasMaxLength(80)
                      .IsRequired();

                entity.Property(x => x.Email)
                      .HasMaxLength(100);

                entity.Property(x => x.Phone)
                      .HasMaxLength(20);

                entity.Property(x => x.IsActive)
                      .IsRequired();

                entity.Property(x => x.CreatedAt)
                      .IsRequired();

                entity.HasIndex(x => x.Email);
            });


            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.HasKey(x => x.ClientId);

                entity.Property(x => x.FullName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.Phone)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(x => x.Email)
                      .HasMaxLength(100);

                entity.Property(x => x.Notes)
                      .HasMaxLength(250);

                entity.Property(x => x.IsActive)
                      .IsRequired();

                entity.Property(x => x.CreatedAt)
                      .IsRequired();

                entity.HasIndex(x => x.Email);
                entity.HasIndex(x => x.Phone);
            });


            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointments");

                entity.HasKey(x => x.AppointmentId);

                entity.Property(x => x.ProfessionalId)
                      .IsRequired();

                entity.Property(x => x.ClientId)
                      .IsRequired();

                entity.Property(x => x.StartAt)
                      .IsRequired();

                entity.Property(x => x.EndAt)
                      .IsRequired();

                entity.Property(x => x.Status)
                      .IsRequired()
                      .HasConversion<string>(); 

                entity.Property(x => x.Notes)
                      .HasMaxLength(250);

                entity.Property(x => x.CreatedAt)
                      .IsRequired();

                // Relaciones
                entity.HasOne(x => x.Professional)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(x => x.ProfessionalId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Client)
                      .WithMany(c => c.Appointments)
                      .HasForeignKey(x => x.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Índices para performance de agenda
                entity.HasIndex(x => new { x.ProfessionalId, x.StartAt });
                entity.HasIndex(x => new { x.ClientId, x.StartAt });
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.UserId);

                entity.Property(x => x.FullName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.Email)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasIndex(x => x.Email)
                      .IsUnique();

                entity.Property(x => x.Role)
                      .IsRequired()
                      .HasConversion<string>();

                entity.Property(x => x.PasswordHash)
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(x => x.IsActive)
                      .IsRequired();

                entity.Property(x => x.CreatedAt)
                      .IsRequired();
            });
        }
    }
}
