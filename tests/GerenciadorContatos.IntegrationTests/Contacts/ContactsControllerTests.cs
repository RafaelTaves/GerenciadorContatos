using CommonTestUtilities.Requests;
using FluentAssertions;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Enums;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GerenciadorContatos.IntegrationTests.Contacts;

public class ContactsControllerTests
{
    [Fact]
    public async Task Create_Should_Return_Created_Contact()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var request = CreateContactRequestBuilder.Build();

        // Act
        var response = await client.PostAsJsonAsync("/api/contacts", request);
        var content = await response.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        content.Should().NotBeNull();
        content!.Name.Should().Be(request.Name);
        content.BirthDate.Should().Be(request.BirthDate);
        content.Gender.Should().Be(request.Gender);
        content.IsActive.Should().BeTrue();
    }
}
