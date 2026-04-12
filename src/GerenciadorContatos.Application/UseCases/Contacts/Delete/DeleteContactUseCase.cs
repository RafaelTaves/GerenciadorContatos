using GerenciadorContatos.Domain.Repositories;

namespace GerenciadorContatos.Application.UseCases.Contacts.Delete;

public class DeleteContactUseCase(IContactRepository contactRepository)
{
    public void Execute(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Contact id is required.", nameof(id));
        }

        var contact = contactRepository.GetById(id)
            ?? throw new KeyNotFoundException("Contact not found.");

        contactRepository.Delete(contact);
    }
}
