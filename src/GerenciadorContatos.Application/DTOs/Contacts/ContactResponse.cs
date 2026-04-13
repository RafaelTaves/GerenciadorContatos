using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Application.DTOs.Contacts;

public record ContactResponse(
    Guid Id,
    string Name,
    DateOnly BirthDate,
    Gender Gender,
    int Age,
    bool IsActive);
