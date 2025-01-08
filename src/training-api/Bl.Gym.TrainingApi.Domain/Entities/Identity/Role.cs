using Bl.Gym.TrainingApi.Domain.Security;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Domain.Entities.Identity;

/// <summary>
/// This entity represents an array of claims.
/// </summary>
public class Role
    : Entity, IReadOnlyList<Claim>
{
    private readonly ImmutableArray<Claim> _claims;

    /// <summary>
    /// Default roles of a system admin.
    /// </summary>
    public static Role Admin { get; }
        = new(
            new[]
            {
                UserClaim.ManageAnyGym,
                UserClaim.ManageTraining,
                UserClaim.SeeTraining,
                UserClaim.ManageGymGroup
            }
        )
        {
            Name = nameof(Admin),
            NormalizedName = NormalizeName(nameof(Admin))
        };

    /// <summary>
    /// Default roles of an gym student. Use the <see cref="NormalizedName"/> to don't duplicate this entity.
    /// </summary>
    public static Role Student { get; }
        = new(
            new[]
            {
                UserClaim.SeeTraining
            }
        )
        {
            Name = nameof(Student),
            NormalizedName = NormalizeName(nameof(Student))
        };

    /// <summary>
    /// Default roles of an student instructor. Use the <see cref="NormalizedName"/> to don't duplicate this entity.
    /// </summary>
    public static Role Instructor { get; }
        = new(
            new[]
            {
                UserClaim.ManageTraining,
                UserClaim.SeeTraining
            }
        )
        {
            Name = nameof(Instructor),
            NormalizedName = NormalizeName(nameof(Instructor))
        };

    /// <summary>
    /// Default roles of an gym owner. Use the <see cref="NormalizedName"/> to don't duplicate this entity.
    /// </summary>
    public static Role GymGroupOwner { get; }
        = new(
            new[]
            {
                UserClaim.ManageTraining,
                UserClaim.SeeTraining,
                UserClaim.ManageGymGroup
            }
        )
        {
            Name = nameof(GymGroupOwner),
            NormalizedName = NormalizeName(nameof(GymGroupOwner))
        };

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// The <see cref="Name"/> but normalized, it can be used as an unique field in this entity.
    /// </summary>
    public string NormalizedName { get; private set; } = string.Empty;
    public Guid ConcurrencyStamp { get; private set; }

    public Claim this[int index] => _claims[index];

    public int Count => _claims.Length;

    private Role(IEnumerable<Claim> claims)
        => _claims = claims.ToImmutableArray();

    public static string NormalizeName(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        return name.ToUpperInvariant();
    }

    public IEnumerator<Claim> GetEnumerator()
    {
        return _claims.AsEnumerable().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _claims.AsEnumerable().GetEnumerator();
    }

    public static bool IsRoleRegistered(string roleName)
    {
        string[] availableRoles = [
            nameof(Admin), 
            nameof(GymGroupOwner),
            nameof(Instructor),
            nameof(Student),
        ];

        return availableRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
    }
}
