﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineClipboard.Data;

#nullable disable

namespace OnlineClipboard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230826152943_AddMaxAndMinLengthToDb")]
    partial class AddMaxAndMinLengthToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("OnlineClipboard.Models.Entry", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("created_at")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("custom_id")
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("custom_id")
                        .IsUnique();

                    b.ToTable("Entry");
                });
#pragma warning restore 612, 618
        }
    }
}
