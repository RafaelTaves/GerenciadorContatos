using FluentValidation;
using GerenciadorContatos.Communication.Requests;
using GerenciadorContatos.Communication.Responses;
using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.Create;

public class CreateContactUseCase(IContactRepository contactRepository, IValidator<CreateContactRequest> validator)
{
    public ContactResponse Execute(CreateContactRequest request)
    {
        validator.ValidateAndThrow(request);

        var contact = Contact.Create(request.Name, request.BirthDate, request.Gender);

        contactRepository.Add(contact);

        return new ContactResponse(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.Age,
            contact.IsActive);
    }
}
