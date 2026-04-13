using Bogus;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class CreateContactRequestBuilder
{
    public static CreateContactRequest Build() 
    {
        var faker = new Faker();

        var request = new CreateContactRequest(
            faker.Name.FullName(),
            DateOnly.FromDateTime(faker.Date.Past(50, DateTime.UtcNow.AddYears(-18))),
            faker.PickRandom<Gender>()
        );

        return request;
    }
}
