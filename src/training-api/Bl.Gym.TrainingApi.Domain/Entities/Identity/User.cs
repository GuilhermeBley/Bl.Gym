
using Bl.Gym.TrainingApi.Domain.Validations;

namespace Bl.Gym.TrainingApi.Domain.Entities.Identity;

public class User
    : Entity
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string NormalizedUserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string NormalizedEmail { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public string PasswordSalt { get; private set; } = string.Empty;
    public Guid? SecurityStamp { get; private set; }
    public Guid? ConcurrencyStamp { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }
    public bool LockoutEnabled { get; private set; }
    public int AccessFailedCount { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is User user &&
               base.Equals(obj) &&
               EntityId.Equals(user.EntityId) &&
               Id.Equals(user.Id) &&
               FirstName == user.FirstName &&
               LastName == user.LastName &&
               UserName == user.UserName &&
               PasswordSalt == user.PasswordSalt &&
               NormalizedUserName == user.NormalizedUserName &&
               Email == user.Email &&
               NormalizedEmail == user.NormalizedEmail &&
               EmailConfirmed == user.EmailConfirmed &&
               PasswordHash == user.PasswordHash &&
               EqualityComparer<Guid?>.Default.Equals(SecurityStamp, user.SecurityStamp) &&
               EqualityComparer<Guid?>.Default.Equals(ConcurrencyStamp, user.ConcurrencyStamp) &&
               PhoneNumber == user.PhoneNumber &&
               PhoneNumberConfirmed == user.PhoneNumberConfirmed &&
               TwoFactorEnabled == user.TwoFactorEnabled &&
               EqualityComparer<DateTimeOffset?>.Default.Equals(LockoutEnd, user.LockoutEnd) &&
               LockoutEnabled == user.LockoutEnabled &&
               AccessFailedCount == user.AccessFailedCount;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(FirstName);
        hash.Add(LastName);
        hash.Add(UserName);
        hash.Add(NormalizedUserName);
        hash.Add(Email);
        hash.Add(NormalizedEmail);
        hash.Add(EmailConfirmed);
        hash.Add(PasswordHash);
        hash.Add(SecurityStamp);
        hash.Add(ConcurrencyStamp);
        hash.Add(PhoneNumber);
        hash.Add(PhoneNumberConfirmed);
        hash.Add(TwoFactorEnabled);
        hash.Add(LockoutEnd);
        hash.Add(LockoutEnabled);
        hash.Add(PasswordSalt);
        hash.Add(AccessFailedCount);
        return hash.ToHashCode();
    }

    public static Result<User> CreateWithHashedPassowrd(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string password,
        string? phoneNumber)
    {
        var builder = new ResultBuilder<User>();

        var hashResult = CreatePasswordResult(password);

        builder.AddRange(hashResult.Errors);

        var userResult = Create(
            id: id,
            firstName: firstName,
            lastName: lastName,
            email: email,
            emailConfirmed: false,
            passwordHash: hashResult.ResultValue?.HashPassword ?? string.Empty,
            passwordSalt: hashResult.ResultValue?.Salt ?? string.Empty,
            securityStamp: Guid.NewGuid(),
            concurrencyStamp: Guid.NewGuid(),
            phoneNumber: phoneNumber,
            phoneNumberConfirmed: false,
            twoFactorEnabled: false,
            lockoutEnd: null,
            lockoutEnabled: false,
            accessFailedCount: 0);

        builder.AddRange(userResult.Errors);

        return builder.CreateResult(() => userResult.RequiredResult);
    }

    public static Result<User> Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        bool emailConfirmed,
        string passwordHash,
        string passwordSalt,
        Guid? securityStamp,
        Guid? concurrencyStamp,
        string? phoneNumber,
        bool phoneNumberConfirmed,
        bool twoFactorEnabled,
        DateTimeOffset? lockoutEnd,
        bool lockoutEnabled,
        int accessFailedCount)
    {
        ResultBuilder<User> builder = new();

        email = email?.Trim() ?? string.Empty;
        firstName = firstName?.Trim() ?? string.Empty;
        lastName = lastName?.Trim() ?? string.Empty;
        phoneNumber = string.IsNullOrEmpty(phoneNumber) ? 
            null :
            string.Concat(phoneNumber.Where(char.IsNumber));

        builder.AddIf(EmailValidation.IsInvalidEmail(email), CoreExceptionCode.InvalidEmail);

        builder.AddIf(firstName.Length > 50 || firstName.Length < 2, CoreExceptionCode.InvalidFirstName);
        builder.AddIf(lastName.Length > 255 || firstName.Length < 2, CoreExceptionCode.InvalidLastName);
        if (phoneNumber is not null)
            builder.AddIf(phoneNumber.Length > 11 || phoneNumber.Length < 8, CoreExceptionCode.InvalidPhoneNumber);

        return builder.CreateResult(() =>
            new()
            {
                AccessFailedCount = accessFailedCount,
                ConcurrencyStamp = concurrencyStamp,
                Email = email,
                PasswordHash = passwordHash,
                EmailConfirmed = emailConfirmed,
                FirstName = firstName,
                Id = id,
                LastName = lastName,
                LockoutEnabled = lockoutEnabled,
                LockoutEnd = lockoutEnd,
                NormalizedEmail = email.ToUpperInvariant(),
                NormalizedUserName = email.ToUpperInvariant(),
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                SecurityStamp = securityStamp,
                TwoFactorEnabled = twoFactorEnabled,
                UserName = email,
                PasswordSalt = passwordSalt,
            });
    }

    public static Result<PasswordResult> CreatePasswordResult(string password)
    {
        ResultBuilder<PasswordResult> builder = new();

        var isValidPassword =
            password.Length >= 8 &&
            password.Length <= 64 &&
            password.Any(char.IsUpper) &&
            password.Any(char.IsNumber) &&
            (password.Any(char.IsSeparator) ||
            password.Any(char.IsSymbol) ||
            password.Any(char.IsPunctuation));

        builder.AddIf(!isValidPassword, CoreExceptionCode.InvalidPassword);

        var hashResult = Security.Sha256Convert.CreateHashedPassword(password);

        return builder.CreateResult(() => new(hashResult.HashBase64, hashResult.Salt));
    }

    public record PasswordResult(
        string HashPassword,
        string Salt);
}
