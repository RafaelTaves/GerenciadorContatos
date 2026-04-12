using FluentAssertions;
using GerenciadorContatos.Domain.Entities;
using GerenciadorContatos.Domain.Enums;
using Xunit;

namespace GerenciadorContatos.UnitTests.Domain.Entities;

public class ContactTests
{
    [Fact]
    public void Create_Should_Create_Active_Contact_When_Data_Is_Valid()
    {
        // Arrange
        var name = "John Doe";
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25));
        var gender = Gender.Male;

        // Act
        var contact = Contact.Create(name, birthDate, gender);

        // Assert
        contact.Id.Should().NotBeEmpty();
        contact.Name.Should().Be(name);
        contact.BirthDate.Should().Be(birthDate);
        contact.Gender.Should().Be(gender);
        contact.IsActive.Should().BeTrue();
        contact.Age.Should().BeGreaterThanOrEqualTo(18);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var name = string.Empty;
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25));

        // Act
        Action act = () => Contact.Create(name, birthDate, Gender.Female);

        // Assert
        var exception = act.Should()
            .Throw<ArgumentException>()
            .Which;

        exception.Message.Should().StartWith("Contact name is required.");
        exception.ParamName.Should().Be("name");
    }

    [Fact]
    public void Create_Should_Throw_When_BirthDate_Is_In_Future()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        // Act
        Action act = () => Contact.Create("John Doe", birthDate, Gender.Male);

        // Assert
        var exception = act.Should()
            .Throw<ArgumentException>()
            .Which;

        exception.Message.Should().StartWith("Birth date cannot be in the future.");
        exception.ParamName.Should().Be("birthDate");
    }

    [Fact]
    public void Create_Should_Throw_When_Contact_Is_Underage()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-17));

        // Act
        Action act = () => Contact.Create("John Doe", birthDate, Gender.Male);

        // Assert
        var exception = act.Should()
            .Throw<ArgumentException>()
            .Which;

        exception.Message.Should().StartWith("Contact must be of legal age.");
        exception.ParamName.Should().Be("birthDate");
    }

    [Fact]
    public void Create_Should_Throw_When_Contact_Age_Is_Zero()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        Action act = () => Contact.Create("John Doe", birthDate, Gender.Male);

        // Assert
        var exception = act.Should()
            .Throw<ArgumentException>()
            .Which;

        exception.Message.Should().StartWith("Contact age must be greater than zero.");
        exception.ParamName.Should().Be("birthDate");
    }

    [Fact]
    public void Create_Should_Throw_When_Gender_Is_Invalid()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25));

        // Act
        Action act = () => Contact.Create("John Doe", birthDate, (Gender)999);

        // Assert
        var exception = act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which;

        exception.Message.Should().StartWith("Invalid gender value.");
        exception.ParamName.Should().Be("gender");
    }

    [Fact]
    public void Update_Should_Change_Contact_Data_When_Request_Is_Valid()
    {
        // Arrange
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        var updatedName = "Jane Doe";
        var updatedBirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));
        var updatedGender = Gender.Female;

        // Act
        contact.Update(updatedName, updatedBirthDate, updatedGender);

        // Assert
        contact.Name.Should().Be(updatedName);
        contact.BirthDate.Should().Be(updatedBirthDate);
        contact.Gender.Should().Be(updatedGender);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False_When_Contact_Is_Active()
    {
        // Arrange
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        // Act
        contact.Deactivate();

        // Assert
        contact.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True_When_Contact_Is_Inactive()
    {
        // Arrange
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        contact.Deactivate();

        // Act
        contact.Activate();

        // Assert
        contact.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_Should_Throw_When_Contact_Is_Already_Inactive()
    {
        // Arrange
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        contact.Deactivate();

        // Act
        Action act = () => contact.Deactivate();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Contact is already inactive.");
    }

    [Fact]
    public void Activate_Should_Throw_When_Contact_Is_Already_Active()
    {
        // Arrange
        var contact = Contact.Create(
            "John Doe",
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Gender.Male);

        // Act
        Action act = () => contact.Activate();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Contact is already active.");
    }
}
