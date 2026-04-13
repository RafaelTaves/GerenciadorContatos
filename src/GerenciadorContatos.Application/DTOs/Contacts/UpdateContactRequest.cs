using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Application.DTOs.Contacts;

public record UpdateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
