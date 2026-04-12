using GerenciadorContatos.Communication.Responses;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.GetById;

public class GetContactByIdUseCase(IContactRepository contactRepository)
{
    public ContactResponse Execute(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Contact id is required.", nameof(id));
        }

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        return new ContactResponse(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.Age,
            contact.IsActive);
    }
}
