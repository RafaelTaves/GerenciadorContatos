using FluentValidation;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.Services.Contacts;

public class ContactService(
    IContactRepository contactRepository,
    IValidator<CreateContactRequest> createContactValidator,
    IValidator<UpdateContactRequest> updateContactValidator) : IContactService
{
    public ContactResponse Create(CreateContactRequest request)
    {
        createContactValidator.ValidateAndThrow(request);

        var contact = Contact.Create(request.Name, request.BirthDate, request.Gender);

        contactRepository.Add(contact);

        return MapToResponse(contact);
    }

    public IReadOnlyList<ContactResponse> GetAll()
    {
        return contactRepository
            .GetAllActive()
            .Select(MapToResponse)
            .ToList();
    }

    public ContactResponse GetById(Guid id)
    {
        ValidateId(id);

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        return MapToResponse(contact);
    }

    public ContactResponse Update(Guid id, UpdateContactRequest request)
    {
        ValidateId(id);
        updateContactValidator.ValidateAndThrow(request);

        var contact = contactRepository.GetActiveById(id)
            ?? throw new KeyNotFoundException("Active contact not found.");

        contact.Update(request.Name, request.BirthDate, request.Gender);

        contactRepository.Update(contact);

        return MapToResponse(contact);
    }

    public ContactResponse Activate(Guid id)
    {
        ValidateId(id);

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        contact.Activate();
        contactRepository.Update(contact);

        return MapToResponse(contact);
    }

    public ContactResponse Deactivate(Guid id)
    {
        ValidateId(id);

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        contact.Deactivate();
        contactRepository.Update(contact);

        return MapToResponse(contact);
    }

    public void Delete(Guid id)
    {
        ValidateId(id);

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        contactRepository.Delete(contact);
    }

    private static ContactResponse MapToResponse(Contact contact)
    {
        return new ContactResponse(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.Age,
            contact.IsActive);
    }

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Contact id is required.", nameof(id));
        }
    }
}
