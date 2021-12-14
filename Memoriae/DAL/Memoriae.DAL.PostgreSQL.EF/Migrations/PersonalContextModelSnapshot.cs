﻿// <auto-generated />
using System;
using Memoriae.DAL.PostgreSQL.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Memoriae.DAL.PostgreSQL.EF.Migrations
{
    [DbContext(typeof(PersonalContext))]
    partial class PersonalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("pgcrypto")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("character varying");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.ToTable("Post", "memoriae");
                });

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.PostTagLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PostId1")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TagId1")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("PostId1");

                    b.HasIndex("TagId");

                    b.HasIndex("TagId1");

                    b.ToTable("PostTagLink", "memoriae");
                });

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.ToTable("Tag", "memoriae");
                });

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.PostTagLink", b =>
                {
                    b.HasOne("Memoriae.DAL.PostgreSQL.EF.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("PostTagLink_PostId_fkey")
                        .IsRequired();

                    b.HasOne("Memoriae.DAL.PostgreSQL.EF.Models.Post", null)
                        .WithMany("PostTagLink")
                        .HasForeignKey("PostId1");

                    b.HasOne("Memoriae.DAL.PostgreSQL.EF.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("PostTagLink_TagId_fkey")
                        .IsRequired();

                    b.HasOne("Memoriae.DAL.PostgreSQL.EF.Models.Tag", null)
                        .WithMany("PostTagLink")
                        .HasForeignKey("TagId1");

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.Post", b =>
                {
                    b.Navigation("PostTagLink");
                });

            modelBuilder.Entity("Memoriae.DAL.PostgreSQL.EF.Models.Tag", b =>
                {
                    b.Navigation("PostTagLink");
                });
#pragma warning restore 612, 618
        }
    }
}
