using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Service2.Models;

public partial class Service2DbContext : DbContext
{
    public Service2DbContext()
    {
    }

    public Service2DbContext(DbContextOptions<Service2DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserFlashcardProgress> UserFlashcardProgresses { get; set; }

    public virtual DbSet<UserGrammarProgress> UserGrammarProgresses { get; set; }

    public virtual DbSet<UserListeningProgress> UserListeningProgresses { get; set; }

    public virtual DbSet<UserReadingProgress> UserReadingProgresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.nitqneodiyligoydzyne;Password=Congkcs12345@@@;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<UserFlashcardProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_flashcard_progress_pkey");

            entity.ToTable("user_flashcard_progress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FlashcardId).HasColumnName("flashcard_id");
            entity.Property(e => e.IsMastered)
                .HasDefaultValue(false)
                .HasColumnName("is_mastered");
            entity.Property(e => e.LastReview)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_review");
            entity.Property(e => e.ReviewCount)
                .HasDefaultValue(0)
                .HasColumnName("review_count");
            entity.Property(e => e.Score)
                .HasDefaultValue(0)
                .HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserGrammarProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_grammar_progress_pkey");

            entity.ToTable("user_grammar_progress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.GrammarExerciseId).HasColumnName("grammar_exercise_id");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.Score)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserListeningProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_listening_progress_pkey");

            entity.ToTable("user_listening_progress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.ListeningId).HasColumnName("listening_id");
            entity.Property(e => e.Score)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserReadingProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_reading_progress_pkey");

            entity.ToTable("user_reading_progress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.ReadingPassageId).HasColumnName("reading_passage_id");
            entity.Property(e => e.Score)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
