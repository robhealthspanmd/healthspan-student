﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using infrastructure.sqlserver.Data;

namespace infrastructure.sqlserver.Migrations
{
    [DbContext(typeof(HealthSpanMdDbContext))]
    [Migration("20211025141753_m0004")]
    partial class m0004
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ActiveMetric", b =>
                {
                    b.Property<long>("ActiveMetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ActiveMetricId");

                    b.HasIndex("MetricId");

                    b.HasIndex("UserId");

                    b.ToTable("ActiveMetrics");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.AuthorizedRole", b =>
                {
                    b.Property<long>("AuthorizedRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("AuthorizedRoleId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("AuthorizedRoles");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.Metric", b =>
                {
                    b.Property<int>("MetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetricTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetricId");

                    b.HasIndex("MetricTypeId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntry", b =>
                {
                    b.Property<long>("MetricTrackingEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EntryDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<bool?>("ValueAsBoolean")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ValueAsDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("ValueAsNumber")
                        .HasColumnType("float");

                    b.Property<string>("ValueAsString")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetricTrackingEntryId");

                    b.HasIndex("MetricId");

                    b.HasIndex("UserId");

                    b.ToTable("MetricTrackingEntries");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricType", b =>
                {
                    b.Property<int>("MetricTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetricTypeId");

                    b.ToTable("MetricTypes");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserRoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ActiveMetric", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("infrastructure.sqlserver.Data.Entities.User", null)
                        .WithMany("ActiveMetrics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metric");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.AuthorizedRole", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.User", null)
                        .WithMany("AuthorizedRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("infrastructure.sqlserver.Data.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.Metric", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.MetricType", "MetricType")
                        .WithMany()
                        .HasForeignKey("MetricTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MetricType");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntry", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("infrastructure.sqlserver.Data.Entities.User", null)
                        .WithMany("MetricTrackingEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metric");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.User", b =>
                {
                    b.Navigation("ActiveMetrics");

                    b.Navigation("AuthorizedRoles");

                    b.Navigation("MetricTrackingEntries");
                });
#pragma warning restore 612, 618
        }
    }
}