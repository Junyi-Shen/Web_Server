using System.Collections.Generic;
using System.Data.Entity;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Data
{
    public class ProjectDbContext : DbContext
    {
        // Specify the name of the connection string from Web.config
        public ProjectDbContext() : base("DefaultConnection")
        {
        }

        // DbSet for each table
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users Table
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasColumnType("nvarchar")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasColumnType("nvarchar")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Buildings Table
            modelBuilder.Entity<Building>()
                .HasRequired(b => b.PropertyManager)
                .WithMany(u => u.ManagedBuildings)
                .HasForeignKey(b => b.PropertyManagerId)
                .WillCascadeOnDelete(true); // ON DELETE CASCADE

            modelBuilder.Entity<Building>()
                .Property(b => b.ImageURL)
                .HasColumnType("nvarchar(max)"); // Optional, stores the URL for the building image

            // Apartments Table
            modelBuilder.Entity<Apartment>()
                .HasRequired(a => a.Building)
                .WithMany(b => b.Apartments)
                .HasForeignKey(a => a.BuildingId)
                .WillCascadeOnDelete(true); // ON DELETE CASCADE

            modelBuilder.Entity<Apartment>()
                .Property(a => a.LivingRoom)
                .IsRequired(); // Number of living rooms, required

            modelBuilder.Entity<Apartment>()
                .Property(a => a.ElevatorAccess)
                .IsRequired(); // Whether the apartment has elevator access, required

            // Appointments Table
            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Tenant)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.TenantId)
                .WillCascadeOnDelete(false); // ON DELETE NO ACTION

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Apartment)
                .WithMany(ap => ap.Appointments)
                .HasForeignKey(a => a.ApartmentId)
                .WillCascadeOnDelete(false); // ON DELETE NO ACTION

            // Messages Table
            modelBuilder.Entity<Message>()
                .HasOptional(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false); // ON DELETE NO ACTION

            modelBuilder.Entity<Message>()
                .HasOptional(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false); // ON DELETE NO ACTION

            // Reports Table
            modelBuilder.Entity<Report>()
                .HasRequired(r => r.PropertyManager)
                .WithMany(u => u.ManagedReports)
                .HasForeignKey(r => r.PropertyManagerId)
                .WillCascadeOnDelete(false); // ON DELETE NO ACTION

            modelBuilder.Entity<Report>()
                .HasRequired(r => r.PropertyOwner)
                .WithMany(u => u.OwnedReports)
                .HasForeignKey(r => r.PropertyOwnerId)
                .WillCascadeOnDelete(true); // ON DELETE CASCADE
        }
    }
}
