﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using infrastructure.sqlserver.Data;

#nullable disable

namespace infrastructure.sqlserver.Migrations
{
    [DbContext(typeof(HealthSpanMdDbContext))]
    [Migration("20220127002207_m0013")]
    partial class m0013
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ActiveMetric", b =>
                {
                    b.Property<long>("ActiveMetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ActiveMetricId"), 1L, 1);

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
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AuthorizedRoleId"), 1L, 1);

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MetricId"), 1L, 1);

                    b.Property<bool>("AllowMultipleValues")
                        .HasColumnType("bit");

                    b.Property<int>("DataType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<double?>("MaxValue")
                        .HasColumnType("float");

                    b.Property<double?>("MinValue")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UseSlider")
                        .HasColumnType("bit");

                    b.HasKey("MetricId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricSelectItem", b =>
                {
                    b.Property<int>("MetricSelectItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MetricSelectItemId"), 1L, 1);

                    b.Property<string>("ItemDisplayText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("MetricSelectItemId");

                    b.HasIndex("MetricId");

                    b.ToTable("MetricsSelectItems");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntry", b =>
                {
                    b.Property<long>("MetricTrackingEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MetricTrackingEntryId"), 1L, 1);

                    b.Property<DateTime>("EntryDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("MetricTrackingEntryId");

                    b.HasIndex("MetricId");

                    b.HasIndex("UserId");

                    b.ToTable("MetricTrackingEntries");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntryValue", b =>
                {
                    b.Property<long>("MetricTrackingEntryValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MetricTrackingEntryValueId"), 1L, 1);

                    b.Property<long>("MetricTrackingEntryId")
                        .HasColumnType("bigint");

                    b.Property<bool?>("ValueAsBoolean")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ValueAsDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("ValueAsNumber")
                        .HasColumnType("float");

                    b.Property<string>("ValueAsString")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetricTrackingEntryValueId");

                    b.HasIndex("MetricTrackingEntryId");

                    b.ToTable("MetricTrackingEntriesValues");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IdentityUserId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("NotificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneCountryCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("ProgramEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ProgramStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TimeZoneId")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserRoleId"), 1L, 1);

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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricSelectItem", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.Metric", null)
                        .WithMany("SelectItems")
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntryValue", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.MetricTrackingEntry", null)
                        .WithMany("EntryValues")
                        .HasForeignKey("MetricTrackingEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.Metric", b =>
                {
                    b.Navigation("SelectItems");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.MetricTrackingEntry", b =>
                {
                    b.Navigation("EntryValues");
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
