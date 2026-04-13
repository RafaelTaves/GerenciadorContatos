using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;
using GerenciadorContatos.Application.Services.Contacts;
using GerenciadorContatos.Application.Validators.Contacts;
using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Enums;
using Xunit;

namespace GerenciadorContatos.UnitTests.Application.Services.Contacts;

public class ContactServiceTests
{
    [Fact]
    public void Create_Should_Add_Contact_And_Return_Response()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var request = CreateContactRequestBuilder.Build();

        // Act
        var response = service.Create(request);

        // Assert
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
        response.BirthDate.Should().Be(request.BirthDate);
        response.Gender.Should().Be(request.Gender);
        response.IsActive.Should().BeTrue();
        repository.GetById(response.Id).Should().NotBeNull();
    }

    [Fact]
    public void Create_Should_Throw_When_Request_Is_Invalid()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var request = CreateContactRequestBuilder.Build() with
        {
            Name = string.Empty
        };

        // Act
        Action act = () => service.Create(request);

        // Assert
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Update_Should_Change_Contact_Data_When_Contact_Exists()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);
        var request = UpdateContactRequestBuilder.Build();

        repository.Seed(contact);

        // Act
        var response = service.Update(contact.Id, request);

        // Assert
        response.Name.Should().Be(request.Name);
        response.BirthDate.Should().Be(request.BirthDate);
        response.Gender.Should().Be(request.Gender);
        repository.GetById(contact.Id)!.Name.Should().Be(request.Name);
    }

    [Fact]
    public void Update_Should_Throw_When_Contact_Does_Not_Exist()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var request = UpdateContactRequestBuilder.Build();

        // Act
        Action act = () => service.Update(Guid.NewGuid(), request);

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Update_Should_Throw_When_Contact_Is_Inactive()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);
        var request = UpdateContactRequestBuilder.Build();

        contact.Deactivate();
        repository.Seed(contact);

        // Act
        Action act = () => service.Update(contact.Id, request);

        // Assert
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetById_Should_Return_Active_Contact()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        repository.Seed(contact);

        // Act
        var response = service.GetById(contact.Id);

        // Assert
        response.Id.Should().Be(contact.Id);
        response.Name.Should().Be(contact.Name);
    }

    [Fact]
    public void GetById_Should_Return_Contact_When_It_Is_Inactive()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        contact.Deactivate();
        repository.Seed(contact);

        // Act
        var response = service.GetById(contact.Id);

        // Assert
        response.Id.Should().Be(contact.Id);
        response.IsActive.Should().BeFalse();
    }

    [Fact]
    public void GetAll_Should_Return_Only_Active_Contacts()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var activeContact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);
        var inactiveContact = Contact.Create(
            "Jane Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
            Gender.Female);

        inactiveContact.Deactivate();
        repository.Seed(activeContact, inactiveContact);

        // Act
        var response = service.GetAll();

        // Assert
        response.Should().HaveCount(1);
        response.Single().Id.Should().Be(activeContact.Id);
    }

    [Fact]
    public void Activate_Should_Activate_Contact()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        contact.Deactivate();
        repository.Seed(contact);

        // Act
        var response = service.Activate(contact.Id);

        // Assert
        response.IsActive.Should().BeTrue();
        repository.GetById(contact.Id)!.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_Should_Deactivate_Contact()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        repository.Seed(contact);

        // Act
        var response = service.Deactivate(contact.Id);

        // Assert
        response.IsActive.Should().BeFalse();
        repository.GetById(contact.Id)!.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Delete_Should_Remove_Contact_From_Repository()
    {
        // Arrange
        var repository = new InMemoryContactRepository();
        var service = CreateService(repository);
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        repository.Seed(contact);

        // Act
        service.Delete(contact.Id);

        // Assert
        repository.GetById(contact.Id).Should().BeNull();
    }

    private static ContactService CreateService(InMemoryContactRepository repository)
    {
        return new ContactService(
            repository,
            new CreateContactRequestValidator(),
            new UpdateContactRequestValidator());
    }
}
