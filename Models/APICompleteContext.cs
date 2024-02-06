using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_Complete_ASP.Models
{
    public partial class APICompleteContext : DbContext
    {
        public APICompleteContext()
        {
        }

        public APICompleteContext(DbContextOptions<APICompleteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<DataUser> DataUsers { get; set; } = null!;
        public virtual DbSet<ResetPass> ResetPasses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=API-Complete;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.IdContact)
                    .HasName("PK__Contact__CDC036FFEA68AB50");

                entity.ToTable("Contact");

                entity.Property(e => e.IdContact).HasColumnName("ID_Contact");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Reference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DataUser>(entity =>
            {
                entity.HasKey(e => e.IdData)
                    .HasName("PK__DataUser__AA50859155EC5A1E");

                entity.ToTable("DataUser");

                entity.Property(e => e.IdData).HasColumnName("ID_Data");

                entity.Property(e => e.City)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Genero)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ResetPass>(entity =>
            {
                entity.HasKey(e => e.IdResetPass)
                    .HasName("PK__ResetPas__F50915C7A3CEA92E");

                entity.ToTable("ResetPass");

                entity.Property(e => e.IdResetPass).HasColumnName("ID_ResetPass");

                entity.Property(e => e.ConfirmPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Users__ED4DE442F763962C");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
