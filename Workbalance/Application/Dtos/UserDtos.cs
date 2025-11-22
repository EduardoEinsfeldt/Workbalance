using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Workbalance.Application.Dtos;

public record UserCreateDto(
    [Required, StringLength(80, MinimumLength = 2)]
    [SwaggerSchema("Nome completo do usuário")]
    string Name,

    [Required, EmailAddress, StringLength(120)]
    [SwaggerSchema("Email único do usuário para login e identificação")]
    string Email,

    [Required, StringLength(255, MinimumLength = 6)]
    [SwaggerSchema("Senha do usuário, que será transformada em hash")]
    string Password,

    [SwaggerSchema("Idioma preferido do usuário (padrão: pt-BR)")]
    string? PreferredLanguage = "pt-BR"
);

public record UserUpdateDto(
    [StringLength(80, MinimumLength = 2)]
    [SwaggerSchema("Novo nome do usuário")]
    string? Name,

    [EmailAddress, StringLength(120)]
    [SwaggerSchema("Novo email do usuário (único)")]
    string? Email,

    [StringLength(255, MinimumLength = 6)]
    [SwaggerSchema("Nova senha do usuário (caso deseje alterar)")]
    string? Password,

    [StringLength(10)]
    [SwaggerSchema("Novo idioma preferido do usuário")]
    string? PreferredLanguage
);

public record UserResponseDto(
    [SwaggerSchema("Identificador único do usuário")]
    Guid Id,

    [SwaggerSchema("Nome registrado")]
    string Name,

    [SwaggerSchema("Email registrado")]
    string Email,

    [SwaggerSchema("Idioma preferido do usuário")]
    string PreferredLanguage,

    [SwaggerSchema("Data de criação do registro")]
    DateTime CreatedAt,

    [SwaggerSchema("Data da última atualização")]
    DateTime UpdatedAt
);
