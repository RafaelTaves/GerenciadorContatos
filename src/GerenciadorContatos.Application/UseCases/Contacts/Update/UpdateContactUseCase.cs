using FluentValidation;
using GerenciadorContatos.Communication.Requests;
using GerenciadorContatos.Communication.Responses;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.Update;

public class UpdateContactUseCase(IContactRepository contactRepository, IValidator<UpdateContactRequest> validator)
{
    public ContactResponse Execute(Guid id, UpdateContactRequest request)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Contact id is required.", nameof(id));
        }

        validator.ValidateAndThrow(request);

        var contact = contactRepository.GetActiveById(id)
            ?? throw new KeyNotFoundException("Active contact not found.");

        contact.Update(request.Name, request.BirthDate, request.Gender);

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
