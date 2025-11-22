using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Workbalance.Domain.Enums;

namespace Workbalance.Application.Dtos;

public record RecommendationCreateDto(
    [Required]
    [SwaggerSchema("Identificador do usuário que receberá a recomendação")]
    Guid UserId,

    [Required]
    [SwaggerSchema("Tipo da recomendação (BREAK, STRETCH, FOCUS, MUSIC, BREATHING, HYDRATION, CUSTOM)")]
    RecommendationType Type,

    [Required, StringLength(200)]
    [SwaggerSchema("Mensagem principal da recomendação")]
    string Message,

    [SwaggerSchema("Link opcional relacionado à recomendação (vídeo, ferramenta, guia)")]
    string? ActionUrl,

    [SwaggerSchema("Horário agendado para execução da recomendação, se existir")]
    DateTime? ScheduledAt,

    [Required]
    [SwaggerSchema("Origem da recomendação (AI ou RULE)")]
    RecommendationSource Source
);

public record RecommendationUpdateDto(
    [SwaggerSchema("Atualizar o tipo de recomendação")]
    RecommendationType? Type,

    [StringLength(200)]
    [SwaggerSchema("Atualizar a mensagem principal")]
    string? Message,

    [SwaggerSchema("Atualizar o link ou ação relacionada")]
    string? ActionUrl,

    [SwaggerSchema("Atualizar o horário agendado")]
    DateTime? ScheduledAt,

    [SwaggerSchema("Atualizar a origem da recomendação")]
    RecommendationSource? Source
);

public record RecommendationResponseDto(
    [SwaggerSchema("Identificador único da recomendação")]
    Guid Id,

    [SwaggerSchema("Usuário destinatário da recomendação")]
    Guid UserId,

    [SwaggerSchema("Tipo de recomendação")]
    RecommendationType Type,

    [SwaggerSchema("Mensagem da recomendação")]
    string Message,

    [SwaggerSchema("URL opcional associada à ação recomendada")]
    string? ActionUrl,

    [SwaggerSchema("Data/hora agendada da recomendação, se houver")]
    DateTime? ScheduledAt,

    [SwaggerSchema("Origem da recomendação (AI ou RULE)")]
    RecommendationSource Source,

    [SwaggerSchema("Data de criação da recomendação")]
    DateTime CreatedAt
);
