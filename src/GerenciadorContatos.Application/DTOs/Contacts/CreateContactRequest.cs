using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Application.DTOs.Contacts;

public record CreateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
