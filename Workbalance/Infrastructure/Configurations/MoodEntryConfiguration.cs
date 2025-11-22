using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workbalance.Domain.Entity;

namespace Workbalance.Infrastructure.Configurations
{
    public class MoodEntryConfiguration : IEntityTypeConfiguration<MoodEntry>
    {
        public void Configure(EntityTypeBuilder<MoodEntry> builder)
        {
            // Nome da tabela
            builder.ToTable("WB_MOOD_ENTRY");

            // PK
            builder.HasKey(m => m.Id);

            // CD_ID
            builder.Property(m => m.Id)
                .HasColumnName("CD_MOOD_ID")
                .HasColumnType("VARCHAR2(36)")
                .IsRequired();

            // CD_USER_ID
            builder.Property(m => m.UserId)
                .HasColumnName("CD_USER_ID")
                .HasColumnType("VARCHAR2(36)")
                .IsRequired();

            // FK → WB_USERS(CD_ID)
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // DT_DATE → Oracle DATE
            builder.Property(m => m.Date)
                .HasColumnName("DT_DATE")
                .HasColumnType("DATE")
                .IsRequired();

            // Unique Constraint: (CD_USER_ID, DT_DATE)
            builder.HasIndex(m => new { m.UserId, m.Date })
                .IsUnique()
                .HasDatabaseName("UQ_WB_MOOD_USER_DATE");

            // NR_MOOD
            builder.Property(m => m.Mood)
                .HasColumnName("NR_MOOD")
                .HasColumnType("NUMBER(3)")
                .IsRequired();

            // NR_STRESS
            builder.Property(m => m.Stress)
                .HasColumnName("NR_STRESS")
                .HasColumnType("NUMBER(3)")
                .IsRequired();

            // NR_PRODUCTIVITY
            builder.Property(m => m.Productivity)
                .HasColumnName("NR_PRODUCTIVITY")
                .HasColumnType("NUMBER(3)")
                .IsRequired();

            // DS_NOTES
            builder.Property(m => m.Notes)
                .HasColumnName("DS_NOTES")
                .HasColumnType("VARCHAR2(500)");

            // DS_TAGS
            builder.Property(m => m.Tags)
                .HasColumnName("DS_TAGS")
                .HasColumnType("VARCHAR2(500)");

            // TS_CREATED_AT
            builder.Property(m => m.CreatedAt)
                .HasColumnName("TS_CREATED_AT")
                .HasColumnType("TIMESTAMP(0)")
                .IsRequired();

            // TS_UPDATED_AT
            builder.Property(m => m.UpdatedAt)
                .HasColumnName("TS_UPDATED_AT")
                .HasColumnType("TIMESTAMP(0)")
                .IsRequired();
        }
    }
}
