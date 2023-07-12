﻿// <auto-generated />
using System;
using Bird_Box.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bird_Box.Migrations
{
    [DbContext(typeof(BirdBoxContext))]
    [Migration("20230712123607_AlphaMigration")]
    partial class AlphaMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bird_Box.Models.IdentifiedBird", b =>
                {
                    b.Property<Guid>("objId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("birdName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("detectionThreshold")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("objId");

                    b.ToTable("BirdRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
