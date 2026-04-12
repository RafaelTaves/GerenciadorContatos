using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Repositories;
using GerenciadorContatos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorContatos.Infrastructure.Repositories;

public class ContactRepository(AppDbContext context) : IContactRepository
{
    public void Add(Contact contact)
    {
        context.Contacts.Add(contact);
        context.SaveChanges();
    }

    public Contact? GetById(Guid id)
    {
        return context.Contacts.FirstOrDefault(contact => contact.Id == id);
    }

    public Contact? GetActiveById(Guid id)
    {
        return context.Contacts
            .AsNoTracking()
            .FirstOrDefault(contact => contact.Id == id && contact.IsActive);
    }

    public IReadOnlyList<Contact> GetAllActive()
    {
        return context.Contacts
            .AsNoTracking()
            .Where(contact => contact.IsActive)
            .OrderBy(contact => contact.Name)
            .ToList();
    }

    public void Update(Contact contact)
    {
        context.Contacts.Update(contact);
        context.SaveChanges();
    }

    public void Delete(Contact contact)
    {
        context.Contacts.Remove(contact);
        context.SaveChanges();
    }
}
