using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Communication.Requests;

public record UpdateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
