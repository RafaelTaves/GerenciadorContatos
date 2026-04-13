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

    [Fact]
    public async Task GetAll_Should_Return_Only_Active_Contacts()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var activeContact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);
        var inactiveContact = Contact.Create(
            "Jane Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
            Gender.Female);

        inactiveContact.Deactivate();
        factory.Repository.Seed(activeContact, inactiveContact);

        // Act
        var response = await client.GetAsync("/api/contacts");
        var content = await response.Content.ReadFromJsonAsync<List<ContactResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content.Should().HaveCount(1);
        content!.Single().Id.Should().Be(activeContact.Id);
    }

    [Fact]
    public async Task GetById_Should_Return_Contact_When_It_Is_Active()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        factory.Repository.Seed(contact);

        // Act
        var response = await client.GetAsync($"/api/contacts/{contact.Id}");
        var content = await response.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content!.Id.Should().Be(contact.Id);
    }

    [Fact]
    public async Task Deactivate_Should_Remove_Contact_From_List_But_Keep_Detail_Available()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        factory.Repository.Seed(contact);

        // Act
        var patchResponse = await client.PatchAsync($"/api/contacts/{contact.Id}/deactivate", null);
        var getByIdResponse = await client.GetAsync($"/api/contacts/{contact.Id}");
        var getAllResponse = await client.GetAsync("/api/contacts");
        var getAllContent = await getAllResponse.Content.ReadFromJsonAsync<List<ContactResponse>>();
        var getByIdContent = await getByIdResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getByIdContent.Should().NotBeNull();
        getByIdContent!.Id.Should().Be(contact.Id);
        getByIdContent.IsActive.Should().BeFalse();
        getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getAllContent.Should().NotBeNull();
        getAllContent.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_Should_Return_Contact_When_It_Is_Inactive()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        contact.Deactivate();
        factory.Repository.Seed(contact);

        // Act
        var response = await client.GetAsync($"/api/contacts/{contact.Id}");
        var content = await response.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content!.Id.Should().Be(contact.Id);
        content.IsActive.Should().BeFalse();
    }
}
