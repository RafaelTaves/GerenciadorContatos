using GerenciadorContatos.Domain.Entities;

namespace GerenciadorContatos.Domain.Repositories;

public interface IContactRepository
{
    void Add(Contact contact);
    Contact? GetById(Guid id);
    Contact? GetActiveById(Guid id);
    IReadOnlyList<Contact> GetAllActive();
    void Update(Contact contact);
    void Delete(Contact contact);
}
