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
    [Migration("20220915161320_m0030")]
    partial class m0030
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ActiveMetric", b =>
                {
                    b.Property<long>("ActiveMetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ActiveMetricId"), 1L, 1);

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<double?>("Goal")
                        .HasColumnType("float");

                    b.Property<double?>("Goal2")
                        .HasColumnType("float");

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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCard", b =>
                {
                    b.Property<int>("ContentCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentCardId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ImageFileId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotificationMessage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContentCardId");

                    b.ToTable("ContentCards");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCardAssignment", b =>
                {
                    b.Property<int>("ContentCardAssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentCardAssignmentId"), 1L, 1);

                    b.Property<long>("AssignedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CompletedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("ContentCardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<long?>("NotificationId")
                        .HasColumnType("bigint");

                    b.Property<string>("NotificationMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ContentCardAssignmentId");

                    b.HasIndex("ContentCardId");

                    b.HasIndex("UserId");

                    b.ToTable("ContentCardsAssignments");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCardMember", b =>
                {
                    b.Property<int>("ContentCardMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentCardMemberId"), 1L, 1);

                    b.Property<int>("ContentCardId")
                        .HasColumnType("int");

                    b.Property<int>("ContentItemId")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("ContentCardMemberId");

                    b.HasIndex("ContentCardId");

                    b.HasIndex("ContentItemId");

                    b.ToTable("ContentCardsMembers");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentFile", b =>
                {
                    b.Property<int>("ContentFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentFileId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContentFileId");

                    b.ToTable("ContentFiles");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentItem", b =>
                {
                    b.Property<int>("ContentItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentItemId"), 1L, 1);

                    b.Property<int?>("ContentFileId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCaption")
                        .HasColumnType("bit");

                    b.Property<int>("ItemType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContentItemId");

                    b.HasIndex("ContentFileId");

                    b.ToTable("ContentItems");
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

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAlphaSelectNumeric")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPositivePolarity")
                        .HasColumnType("bit");

                    b.Property<double?>("MaxValue")
                        .HasColumnType("float");

                    b.Property<double?>("MinValue")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Threshold")
                        .HasColumnType("float");

                    b.Property<double?>("Threshold2")
                        .HasColumnType("float");

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

                    b.Property<DateTime>("EntryForDate")
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

                    b.Property<double?>("Value2AsNumber")
                        .HasColumnType("float");

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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.Notification", b =>
                {
                    b.Property<long>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("NotificationId"), 1L, 1);

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentDateTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.NotificationTemplate", b =>
                {
                    b.Property<int>("NotificationTemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationTemplateId"), 1L, 1);

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<string>("Template")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationTemplateId");

                    b.ToTable("NotificationTemplates");
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

                    b.Property<bool>("NotificationByEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationBySMS")
                        .HasColumnType("bit");

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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCardAssignment", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.ContentCard", "ContentCard")
                        .WithMany()
                        .HasForeignKey("ContentCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("infrastructure.sqlserver.Data.Entities.User", null)
                        .WithMany("ContentCardAssignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentCard");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCardMember", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.ContentCard", null)
                        .WithMany("ContentCardMembers")
                        .HasForeignKey("ContentCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("infrastructure.sqlserver.Data.Entities.ContentItem", "ContentItem")
                        .WithMany()
                        .HasForeignKey("ContentItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentItem");
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentItem", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.ContentFile", "ContentFile")
                        .WithMany()
                        .HasForeignKey("ContentFileId");

                    b.Navigation("ContentFile");
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

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.Notification", b =>
                {
                    b.HasOne("infrastructure.sqlserver.Data.Entities.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("infrastructure.sqlserver.Data.Entities.ContentCard", b =>
                {
                    b.Navigation("ContentCardMembers");
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

                    b.Navigation("ContentCardAssignments");

                    b.Navigation("MetricTrackingEntries");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}