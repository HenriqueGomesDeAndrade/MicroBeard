﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MicroBeard.Entities.Models
{
    public partial class MicroBeardContext : DbContext
    {
        public MicroBeardContext()
        {
        }

        public MicroBeardContext(DbContextOptions<MicroBeardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collaborator> Collaborators { get; set; } = null!;
        public virtual DbSet<CollaboratorService> CollaboratorServices { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<LicencedCollaborator> LicencedCollaborators { get; set; } = null!;
        public virtual DbSet<License> Licenses { get; set; } = null!;
        public virtual DbSet<Scheduling> Schedulings { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost;Database=MicroBeard;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Collaborator");

                entity.Property(e => e.Commision).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.Function)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<CollaboratorService>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Collaborator_Service");

                entity.HasOne(d => d.CollaboratorCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CollaboratorCode)
                    .HasConstraintName("FK__Collabora__Colla__787EE5A0");

                entity.HasOne(d => d.ServiceCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.ServiceCode)
                    .HasConstraintName("FK__Collabora__Servi__797309D9");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Contact");

                entity.HasIndex(e => e.Cpf, "IDX_CPF_Contact")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "IDX_Email_Contact")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LicencedCollaborator>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Licenced_Collaborator");

                entity.HasOne(d => d.CollaboratorCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CollaboratorCode)
                    .HasConstraintName("FK__Licenced___Colla__7B5B524B");

                entity.HasOne(d => d.LicenceCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.LicenceCode)
                    .HasConstraintName("FK__Licenced___Licen__7C4F7684");
            });

            modelBuilder.Entity<License>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("License");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Scheduling>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Scheduling");

                entity.HasOne(d => d.ContactCodeNavigation)
                    .WithMany(p => p.Schedulings)
                    .HasForeignKey(d => d.ContactCode)
                    .HasConstraintName("FK_Scheduling_Contact_Code");

                entity.HasOne(d => d.ServiceCodeNavigation)
                    .WithMany(p => p.Schedulings)
                    .HasForeignKey(d => d.ServiceCode)
                    .HasConstraintName("FK_Scheduling_Service_Code");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Service");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}