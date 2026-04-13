using Bogus;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class UpdateContactRequestBuilder
{
    public static UpdateContactRequest Build()
    {
        var faker = new Faker();

        return new UpdateContactRequest(
            faker.Name.FullName(),
            DateOnly.FromDateTime(faker.Date.Past(50, DateTime.UtcNow.AddYears(-18))),
            faker.PickRandom<Gender>());
    }
}
