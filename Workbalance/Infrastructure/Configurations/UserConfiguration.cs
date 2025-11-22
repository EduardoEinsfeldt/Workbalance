using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workbalance.Domain.Entity;

namespace Workbalance.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Nome da tabela
            builder.ToTable("WB_USER");

            // PK
            builder.HasKey(u => u.Id);

            // ID (36)
            builder.Property(u => u.Id)
                .HasColumnName("CD_USER_ID")
                .HasColumnType("VARCHAR2(36)")
                .IsRequired();

            // NAME
            builder.Property(u => u.Name)
                .HasColumnName("NM_NAME")
                .HasMaxLength(80)
                .IsRequired();

            // EMAIL (único)
            builder.Property(u => u.Email)
                .HasColumnName("DS_EMAIL")
                .HasMaxLength(120)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            // PASSWORD_HASH
            builder.Property(u => u.PasswordHash)
                .HasColumnName("DS_PASSWORD_HASH")
                .HasMaxLength(255)
                .IsRequired();

            // Preferred Language
            builder.Property(u => u.PreferredLanguage)
                .HasColumnName("DS_PREFERRED_LANGUAGE")
                .HasMaxLength(10)
                .HasDefaultValueSql("'pt-BR'");

            // CreatedAt
            builder.Property(u => u.CreatedAt)
                .HasColumnName("TS_CREATED_AT")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            // UpdatedAt
            builder.Property(u => u.UpdatedAt)
                .HasColumnName("TS_UPDATED_AT")
                .HasColumnType("TIMESTAMP")
                .IsRequired();
        }
    }
}