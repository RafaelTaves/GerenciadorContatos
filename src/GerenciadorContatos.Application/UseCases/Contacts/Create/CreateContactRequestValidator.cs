using FluentValidation;
using GerenciadorContatos.Communication.Requests;

namespace GerenciadorContatos.Application.UseCases.Contacts.Create;

public class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
{
    public CreateContactRequestValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Contact name is required.");

        RuleFor(x => x.BirthDate)
            .NotEqual(default(DateOnly))
            .WithMessage("Birth date is required.")
            .LessThanOrEqualTo(today)
            .WithMessage("Birth date cannot be in the future.")
            .Must(birthDate => GetAge(birthDate, today) > 0)
            .WithMessage("Contact age must be greater than zero.")
            .Must(birthDate => GetAge(birthDate, today) >= 18)
            .WithMessage("Contact must be of legal age.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Invalid gender value.");
    }

    private static int GetAge(DateOnly birthDate, DateOnly today)
    {
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
