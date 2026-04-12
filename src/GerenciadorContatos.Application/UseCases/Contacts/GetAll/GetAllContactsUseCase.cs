using GerenciadorContatos.Communication.Responses;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.GetAll;

public class GetAllContactsUseCase(IContactRepository contactRepository)
{
    public IReadOnlyList<ContactResponse> Execute()
    {
        return contactRepository
            .GetAllActive()
            .Select(contact => new ContactResponse(
                contact.Id,
                contact.Name,
                contact.BirthDate,
                contact.Gender,
                contact.Age,
                contact.IsActive))
            .ToList();
    }
}
