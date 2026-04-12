using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = [];

    public void Seed(params Contact[] contacts)
    {
        _contacts.AddRange(contacts);
    }

    public void Add(Contact contact)
    {
        _contacts.Add(contact);
    }

    public Contact? GetById(Guid id)
    {
        return _contacts.FirstOrDefault(contact => contact.Id == id);
    }

    public Contact? GetActiveById(Guid id)
    {
        return _contacts.FirstOrDefault(contact => contact.Id == id && contact.IsActive);
    }

    public IReadOnlyList<Contact> GetAllActive()
    {
        return _contacts
            .Where(contact => contact.IsActive)
            .OrderBy(contact => contact.Name)
            .ToList();
    }

    public void Update(Contact contact)
    {
    }

    public void Delete(Contact contact)
    {
        _contacts.Remove(contact);
    }
}
