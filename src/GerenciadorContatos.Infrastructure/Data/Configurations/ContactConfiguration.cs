using GerenciadorContatos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorContatos.Infrastructure.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(contact => contact.Id);

        builder.Property(contact => contact.Id)
            .ValueGeneratedNever();

        builder.Property(contact => contact.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(contact => contact.BirthDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(contact => contact.Gender)
            .IsRequired();

        builder.Property(contact => contact.IsActive)
            .IsRequired();

        builder.Ignore(contact => contact.Age);
    }
}
