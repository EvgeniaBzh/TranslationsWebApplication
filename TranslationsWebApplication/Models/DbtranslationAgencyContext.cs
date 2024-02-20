using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TranslationsWebApplication.Models;

public partial class DbtranslationAgencyContext : DbContext
{
    public DbtranslationAgencyContext()
    {
    }

    public DbtranslationAgencyContext(DbContextOptions<DbtranslationAgencyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PersonalAccount> PersonalAccounts { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Translator> Translators { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-DHK8L7H\\SQLEXPRESS; Database=DBTranslationAgency; Trusted_Connection=True;  Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(e => e.AdminEmail).HasMaxLength(50);
            entity.Property(e => e.AdminName).HasMaxLength(50);
            entity.Property(e => e.AdminPassword).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Admins)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admins_Account");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerEmail).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.CustomerPassword)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Account");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.Property(e => e.LanguageName).HasMaxLength(50);
        });

        /*modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.TextMessage).HasColumnType("text");

            entity.HasOne(d => d.Customer).WithMany(p => p.Messages)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Customers");

            entity.HasOne(d => d.Order).WithMany(p => p.Messages)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Orders");

            entity.HasOne(d => d.Translator).WithMany(p => p.Messages)
                .HasForeignKey(d => d.TranslatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Translators");
        });*/

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderName).HasMaxLength(50);
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.OrderSubmissionDate).HasColumnType("datetime");

           //entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                //.HasForeignKey(d => d.CustomerId)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_Orders_D_Customers");

            entity.HasOne(d => d.Topic).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK_Orders_Topic");

             /*entity.HasOne(d => d.Translator).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TranslatorId)
                .HasConstraintName("FK_Orders_D_Translators");

            entity.HasOne(d => d.Type).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Types");

            entity.HasOne(d => d.OriginalLanguage).WithMany(p => p.OrderOriginalLanguages)
                .HasForeignKey(d => d.OriginalLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Languages");

            entity.HasOne(d => d.TranslationLanguage).WithMany(p => p.OrderTranslationLanguages)
                .HasForeignKey(d => d.TranslationLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Languages2");*/
        });

        modelBuilder.Entity<PersonalAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("PersonalAccount");

            entity.Property(e => e.AccountName).HasMaxLength(50);
            entity.Property(e => e.AccountPhoto).HasColumnType("image");
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("Topic");

            entity.Property(e => e.TopicName)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK_Payment");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

           /* entity.HasOne(d => d.Order).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Orders");*/
        });

        modelBuilder.Entity<Translator>(entity =>
        {
            entity.Property(e => e.TranslatorEmail).HasMaxLength(50);
            entity.Property(e => e.TranslatorName).HasMaxLength(50);
            entity.Property(e => e.TranslatorPassword).HasMaxLength(50);

            entity.HasOne(d => d.AddedAdmin).WithMany(p => p.Translators)
                .HasForeignKey(d => d.AddedAdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Translators_Admins");

            entity.HasOne(d => d.Account).WithMany(p => p.Translators)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Translators_Account");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
