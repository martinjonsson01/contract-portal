﻿// <auto-generated />
using System;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PostgresContractRepositoryMigrations
{
    [DbContext(typeof(PostgresContractRepository))]
    partial class PostgresContractRepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Contracts.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalDocument")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FAQ")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InspirationalImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupplierContactInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupplierDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupplierLogoImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Domain.Contracts.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Domain.Contracts.Tag", b =>
                {
                    b.HasOne("Domain.Contracts.Contract", null)
                        .WithMany("Tags")
                        .HasForeignKey("ContractId");
                });

            modelBuilder.Entity("Domain.Contracts.Contract", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
