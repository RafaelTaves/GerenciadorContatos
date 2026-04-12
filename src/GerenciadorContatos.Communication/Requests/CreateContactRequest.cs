using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Communication.Requests;

public record CreateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
