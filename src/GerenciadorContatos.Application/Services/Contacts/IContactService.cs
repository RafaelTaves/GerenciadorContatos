using GerenciadorContatos.Application.DTOs.Contacts;

namespace GerenciadorContatos.Application.Services.Contacts;

public interface IContactService
{
    ContactResponse Create(CreateContactRequest request);
    IReadOnlyList<ContactResponse> GetAll();
    ContactResponse GetById(Guid id);
    ContactResponse Update(Guid id, UpdateContactRequest request);
    ContactResponse Activate(Guid id);
    ContactResponse Deactivate(Guid id);
    void Delete(Guid id);
}
