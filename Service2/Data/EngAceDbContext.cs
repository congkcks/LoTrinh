using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Service2.Models;

namespace Service2.Data;

public partial class EngAceDbContext : DbContext
{
    public EngAceDbContext()
    {
    }

    public EngAceDbContext(DbContextOptions<EngAceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DictationQuestion> DictationQuestions { get; set; }

    public virtual DbSet<Flashcard> Flashcards { get; set; }

    public virtual DbSet<GrammarExercise> GrammarExercises { get; set; }

    public virtual DbSet<ListeningPractice> ListeningPractices { get; set; }

    public virtual DbSet<ReadingPassage> ReadingPassages { get; set; }

    public virtual DbSet<ReadingQuestion> ReadingQuestions { get; set; }

    public virtual DbSet<UserFlashcardProgress> UserFlashcardProgresses { get; set; }

    public virtual DbSet<UserHighlight> UserHighlights { get; set; }

    public virtual DbSet<UserNote> UserNotes { get; set; }

    public virtual DbSet<UserVocabulary> UserVocabularies { get; set; }

    public virtual DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("");

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
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<DictationQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dictation_questions_pkey");
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("flashcards_pkey");
        });

        modelBuilder.Entity<GrammarExercise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grammar_exercises_pkey");
        });

        modelBuilder.Entity<ListeningPractice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("listening_practice_pkey");
        });

        modelBuilder.Entity<ReadingPassage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reading_passages_pkey");
        });

        modelBuilder.Entity<ReadingQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reading_questions_pkey");
        });

        modelBuilder.Entity<UserFlashcardProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_flashcard_progress_pkey");

            entity.Property(e => e.Score).HasDefaultValue(0);
        });

        modelBuilder.Entity<UserHighlight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_highlights_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UserNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_notes_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UserVocabulary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_vocabulary_pkey");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vocabulary_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
