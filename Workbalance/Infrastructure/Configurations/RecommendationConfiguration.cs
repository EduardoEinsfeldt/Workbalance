using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workbalance.Domain.Entity;
using Workbalance.Domain.Enums;

namespace Workbalance.Infrastructure.Configurations
{
    public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
    {
        public void Configure(EntityTypeBuilder<Recommendation> builder)
        {
            // Nome da tabela
            builder.ToTable("WB_RECOMMENDATION");

            // PK
            builder.HasKey(r => r.Id);

            // CD_RECOMMENDATION_ID
            builder.Property(r => r.Id)
                .HasColumnName("CD_RECOMMENDATION_ID")
                .HasColumnType("VARCHAR2(36)")
                .IsRequired();

            // CD_USER_ID (FK)
            builder.Property(r => r.UserId)
                .HasColumnName("CD_USER_ID")
                .HasColumnType("VARCHAR2(36)")
                .IsRequired();

            // FK → WB_USERS(CD_ID)
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // DS_TYPE → enum string
            builder.Property(r => r.Type)
                .HasColumnName("DS_TYPE")
                .HasColumnType("VARCHAR2(20)")
                .HasConversion<string>()
                .IsRequired();

            // DS_MESSAGE
            builder.Property(r => r.Message)
                .HasColumnName("DS_MESSAGE")
                .HasColumnType("VARCHAR2(200)")
                .IsRequired();

            // DS_ACTION_URL
            builder.Property(r => r.ActionUrl)
                .HasColumnName("DS_ACTION_URL")
                .HasColumnType("VARCHAR2(300)");

            // TS_SCHEDULED_AT
            builder.Property(r => r.ScheduledAt)
                .HasColumnName("TS_SCHEDULED_AT")
                .HasColumnType("TIMESTAMP(0)");

            // DS_SOURCE → enum string
            builder.Property(r => r.Source)
                .HasColumnName("DS_SOURCE")
                .HasColumnType("VARCHAR2(20)")
                .HasConversion<string>()
                .IsRequired();

            // TS_CREATED_AT
            builder.Property(r => r.CreatedAt)
                .HasColumnName("TS_CREATED_AT")
                .HasColumnType("TIMESTAMP(0)")
                .IsRequired();

            // Índice: Usuário + criação
            builder.HasIndex(r => new { r.UserId, r.CreatedAt })
                .HasDatabaseName("IDX_WB_RECOM_USER_CREATED");
        }
    }
}
