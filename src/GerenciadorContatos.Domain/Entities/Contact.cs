using GerenciadorContatos.Domain.Enums;

namespace GerenciadorContatos.Domain.Entities;

public class Contact
{
    private const int LegalAge = 18;

    private Contact()
    {
    }

    private Contact(Guid id, string name, DateOnly birthDate, Gender gender)
    {
        Id = id;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
        IsActive = true;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public bool IsActive { get; private set; }
    public int Age => GetAge();

    public static Contact Create(string name, DateOnly birthDate, Gender gender)
    {
        var sanitizedName = SanitizeName(name);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        ValidateBirthDate(birthDate, today);
        ValidateLegalAge(birthDate, today);
        ValidateGender(gender);

        return new Contact(Guid.NewGuid(), sanitizedName, birthDate, gender);
    }

    public void Update(string name, DateOnly birthDate, Gender gender)
    {
        var sanitizedName = SanitizeName(name);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        ValidateBirthDate(birthDate, today);
        ValidateLegalAge(birthDate, today);
        ValidateGender(gender);

        Name = sanitizedName;
        BirthDate = birthDate;
        Gender = gender;
    }

    public void Activate()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("Contact is already active.");
        }

        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Contact is already inactive.");
        }

        IsActive = false;
    }

    public int GetAge()
    {
        return CalculateAge(BirthDate, DateOnly.FromDateTime(DateTime.UtcNow));
    }

    private static string SanitizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Contact name is required.", nameof(name));
        }

        return name.Trim();
    }

    private static void ValidateBirthDate(DateOnly birthDate, DateOnly today)
    {
        if (birthDate > today)
        {
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
        }

        if (CalculateAge(birthDate, today) <= 0)
        {
            throw new ArgumentException("Contact age must be greater than zero.", nameof(birthDate));
        }
    }

    private static void ValidateLegalAge(DateOnly birthDate, DateOnly today)
    {
        if (CalculateAge(birthDate, today) < LegalAge)
        {
            throw new ArgumentException("Contact must be of legal age.", nameof(birthDate));
        }
    }

    private static void ValidateGender(Gender gender)
    {
        if (!Enum.IsDefined(gender))
        {
            throw new ArgumentOutOfRangeException(nameof(gender), gender, "Invalid gender value.");
        }
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly today)
    {
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
