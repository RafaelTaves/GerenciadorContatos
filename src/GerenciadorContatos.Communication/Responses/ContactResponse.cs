using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Communication.Responses;

public record ContactResponse(
    Guid Id,
    string Name,
    DateOnly BirthDate,
    Gender Gender,
    int Age,
    bool IsActive);
