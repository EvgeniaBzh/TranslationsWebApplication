﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TranslationsWebApplication.Models;

#nullable disable

namespace TranslationsWebApplication.Migrations
{
    [DbContext(typeof(DbtranslationAgencyContext))]
    partial class DbtranslationAgencyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TranslationsWebApplication.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("AdminEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AdminPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AdminId");

                    b.HasIndex("AccountId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CustomerPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CustomerId");

                    b.HasIndex("AccountId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Language", b =>
                {
                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LanguageId"));

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LanguageId");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("TextMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TranslatorId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TranslatorId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("OrderName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("OrderPrice")
                        .HasColumnType("float");

                    b.Property<int>("OrderScope")
                        .HasColumnType("int");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.Property<DateTime>("OrderSubmissionDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int");

                    b.Property<int?>("TypeId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("TopicId");

                    b.HasIndex("TypeId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.PersonalAccount", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("AccountPhoto")
                        .HasColumnType("image");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime");

                    b.HasKey("AccountId");

                    b.ToTable("PersonalAccount", (string)null);
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"));

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.HasKey("TopicId");

                    b.ToTable("Topic", (string)null);
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<double>("AmountOfMoney")
                        .HasColumnType("float");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime");

                    b.HasKey("TransactionId")
                        .HasName("PK_Payment");

                    b.HasIndex("OrderId");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Translator", b =>
                {
                    b.Property<int>("TranslatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TranslatorId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("AddedAdminId")
                        .HasColumnType("int");

                    b.Property<string>("TranslatorEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TranslatorExperience")
                        .HasColumnType("int");

                    b.Property<string>("TranslatorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TranslatorPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TranslatorId");

                    b.HasIndex("AccountId");

                    b.HasIndex("AddedAdminId");

                    b.ToTable("Translators");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Type", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeId"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TypeId");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Admin", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.PersonalAccount", "Account")
                        .WithMany("Admins")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK_Admins_Account");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Customer", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.PersonalAccount", "Account")
                        .WithMany("Customers")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK_Customers_Account");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Message", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.Customer", "Customer")
                        .WithMany("Messages")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TranslationsWebApplication.Models.Translator", "Translator")
                        .WithMany("Messages")
                        .HasForeignKey("TranslatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Translator");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Order", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.Topic", "Topic")
                        .WithMany("Orders")
                        .HasForeignKey("TopicId")
                        .HasConstraintName("FK_Orders_Topic");

                    b.HasOne("TranslationsWebApplication.Models.Type", "Type")
                        .WithMany("Orders")
                        .HasForeignKey("TypeId")
                        .IsRequired()
                        .HasConstraintName("FK_Orders_Types");

                    b.Navigation("Topic");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Transaction", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Translator", b =>
                {
                    b.HasOne("TranslationsWebApplication.Models.PersonalAccount", "Account")
                        .WithMany("Translators")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK_Translators_Account");

                    b.HasOne("TranslationsWebApplication.Models.Admin", "AddedAdmin")
                        .WithMany("Translators")
                        .HasForeignKey("AddedAdminId")
                        .IsRequired()
                        .HasConstraintName("FK_Translators_Admins");

                    b.Navigation("Account");

                    b.Navigation("AddedAdmin");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Admin", b =>
                {
                    b.Navigation("Translators");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Customer", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.PersonalAccount", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Customers");

                    b.Navigation("Translators");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Topic", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Translator", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("TranslationsWebApplication.Models.Type", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
