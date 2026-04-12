using CommonTestUtilities.Requests;
using FluentAssertions;
using GerenciadorContatos.Application.UseCases.Contacts.Create;
using GerenciadorContatos.Communication.Requests;
using GerenciadorContatos.Domain.Enums;

namespace Validator.Tests.Contacts.Create;

public class CreateContactRequestValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_When_Name_Is_Empty()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build() with
        {
            Name = string.Empty
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.ErrorMessage == "Contact name is required.");
    }

    [Fact]
    public void Error_When_BirthDate_Is_Default()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build() with
        {
            BirthDate = default
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage == "Birth date is required.");
    }

    [Fact]
    public void Error_When_BirthDate_Is_In_Future()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build() with
        {
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage == "Birth date cannot be in the future.");
    }

    [Fact]
    public void Error_When_Contact_Is_Underage()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build() with
        {
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-17))
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage == "Contact must be of legal age.");
    }

    [Fact]
    public void Error_When_Gender_Is_Invalid()
    {
        // Arrange
        var validator = new CreateContactRequestValidator();
        var request = CreateContactRequestBuilder.Build() with
        {
            Gender = (Gender)999
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage == "Invalid gender value.");
    }
}
