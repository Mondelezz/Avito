﻿// <auto-generated />
using System;
using Avito.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Avito.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230606174108_Favorites")]
    partial class Favorites
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Avito.Models.AdModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryModelId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("NameCategory")
                        .HasColumnType("text");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("integer");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryModelId");

                    b.HasIndex("PersonId");

                    b.ToTable("AdModels");
                });

            modelBuilder.Entity("Avito.Models.CategoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Avito.Models.FavoriteModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AdId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("adModelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AdId");

                    b.HasIndex("adModelId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Avito.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("HashPassword")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Avito.Models.AdModel", b =>
                {
                    b.HasOne("Avito.Models.CategoryModel", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avito.Models.Person", "Person")
                        .WithMany("AdModels")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Avito.Models.FavoriteModel", b =>
                {
                    b.HasOne("Avito.Models.Person", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("AdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avito.Models.AdModel", "adModel")
                        .WithMany("Favorites")
                        .HasForeignKey("adModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("adModel");
                });

            modelBuilder.Entity("Avito.Models.AdModel", b =>
                {
                    b.Navigation("Favorites");
                });

            modelBuilder.Entity("Avito.Models.Person", b =>
                {
                    b.Navigation("AdModels");

                    b.Navigation("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
