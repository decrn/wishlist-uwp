﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ServerApp.Data;
using System;

namespace ServerApp.Migrations
{
    [DbContext(typeof(WishContext))]
    [Migration("20171212173000_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ServerApp.Models.Item", b =>
                {
                    b.Property<int>("ItemId");

                    b.Property<bool>("IsCheckLocked");

                    b.Property<bool>("IsCompleted");

                    b.Property<double>("ItemPriceUsd");

                    b.Property<int>("ListId");

                    b.Property<string>("ProductImageUrl");

                    b.Property<string>("ProductInfoUrl");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("ItemId");

                    b.HasIndex("ListId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("ServerApp.Models.List", b =>
                {
                    b.Property<int>("ListId");

                    b.Property<int>("Color");

                    b.Property<string>("CuratorName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Description");

                    b.Property<string>("EditableHash");

                    b.Property<string>("Icon");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ViewableHash");

                    b.Property<bool>("isHidden");

                    b.Property<bool>("isReadOnly");

                    b.HasKey("ListId");

                    b.ToTable("List");
                });

            modelBuilder.Entity("ServerApp.Models.Item", b =>
                {
                    b.HasOne("ServerApp.Models.List", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}