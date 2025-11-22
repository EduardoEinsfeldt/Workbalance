using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Workbalance.Application.Dtos;

public record MoodEntryCreateDto(
    [Required]
    [SwaggerSchema("Identificador do usuário dono do registro")]
    Guid UserId,

    [Required]
    [SwaggerSchema("Data do registro de humor (somente data, sem horário)")]
    DateOnly Date,

    [Required, Range(0, 10)]
    [SwaggerSchema("Nível de humor do usuário no dia (0 a 10)")]
    int Mood,

    [Required, Range(0, 10)]
    [SwaggerSchema("Nível de estresse do usuário no dia (0 a 10)")]
    int Stress,

    [Required, Range(0, 10)]
    [SwaggerSchema("Produtividade percebida no dia (0 a 10)")]
    int Productivity,

    [StringLength(500)]
    [SwaggerSchema("Notas adicionais sobre o dia, opcional")]
    string? Notes,

    [SwaggerSchema("Tags do registro, separadas por vírgula (ex: 'trabalho,cansado')")]
    string? Tags
);

public record MoodEntryUpdateDto(
    [Range(0, 10)]
    [SwaggerSchema("Novo humor (0 a 10)")]
    int? Mood,

    [Range(0, 10)]
    [SwaggerSchema("Novo estresse (0 a 10)")]
    int? Stress,

    [Range(0, 10)]
    [SwaggerSchema("Nova produtividade (0 a 10)")]
    int? Productivity,

    [StringLength(500)]
    [SwaggerSchema("Alteração nas notas adicionais")]
    string? Notes,

    [SwaggerSchema("Alteração nas tags separadas por vírgula")]
    string? Tags
);

public record MoodEntryResponseDto(
    [SwaggerSchema("Identificador único do registro de humor")]
    Guid Id,

    [SwaggerSchema("Usuário ao qual o registro pertence")]
    Guid UserId,

    [SwaggerSchema("Data do registro")]
    DateOnly Date,

    [SwaggerSchema("Humor registrado (0 a 10)")]
    int Mood,

    [SwaggerSchema("Estresse registrado (0 a 10)")]
    int Stress,

    [SwaggerSchema("Produtividade registrada (0 a 10)")]
    int Productivity,

    [SwaggerSchema("Notas adicionais sobre o dia")]
    string? Notes,

    [SwaggerSchema("Tags associadas ao registro")]
    string? Tags,

    [SwaggerSchema("Data de criação do registro")]
    DateTime CreatedAt,

    [SwaggerSchema("Data da última atualização do registro")]
    DateTime UpdatedAt
);
