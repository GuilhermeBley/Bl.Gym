
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
        hash.Add(AccessFailedCount);
        return hash.ToHashCode();
    }

    public static Result<User> Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        bool emailConfirmed,
        string passwordHash,
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

        builder.AddIf(EmailValidation.IsInvalidEmail(email), CoreExceptionCode.InvalidEmail);

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
            });
    }
}
