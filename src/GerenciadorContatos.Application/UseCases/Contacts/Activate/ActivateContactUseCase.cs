using GerenciadorContatos.Communication.Responses;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.Activate;

public class ActivateContactUseCase(IContactRepository contactRepository)
{
    public ContactResponse Execute(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Contact id is required.", nameof(id));
        }

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        contact.Activate();

        contactRepository.Update(contact);

        return new ContactResponse(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.Age,
            contact.IsActive);
    }
}
