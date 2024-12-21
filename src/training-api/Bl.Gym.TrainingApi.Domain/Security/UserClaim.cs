using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Domain.Security;

public static class UserClaim
{
    public const string DEFAULT_ROLE = "role";
    public const string DEFAULT_USER_NAME = "name";
    public const string DEFAULT_FIRST_NAME = "firstname";
    public const string DEFAULT_LAST_NAME = "lastname";
    public const string DEFAULT_USER_ID = "nameidentifier";
    public const string DEFAULT_USER_EMAIL = "emailaddress";
    public const string DEFAULT_GYM_ID = "gymidentifier";
    public const string DEFAULT_GYM_INVITATION_ID = "gyminvitationid";

    /// <summary>
    /// Student that can see its own training sheets.
    /// </summary>
    public static Claim SeeTraining => new(DEFAULT_ROLE, "SeeTraining");
    /// <summary>
    /// Gym instructor that can manage the user's training sheets.
    /// </summary>
    public static Claim ManageTraining => new(DEFAULT_ROLE, "ManageTraining");
    /// <summary>
    /// Gym owner, it can manage it.
    /// </summary>
    public static Claim ManageGymGroup => new(DEFAULT_ROLE, "ManageGymGroup");

    /// <summary>
    /// The user with this role is authorized to change his password.
    /// </summary>
    public static Claim ChangePassword => new(DEFAULT_ROLE, "ChangePassword");

    /// <summary>
    /// The user with this role is authorized to create or manage any gym.
    /// </summary>
    /// <remarks>
    ///     <para>This role is usually given to admins.</para>
    /// </remarks>
    public static Claim ManageAnyGym => new(DEFAULT_ROLE, "ManageAnyGym");


    public static Claim CreateUserNameClaim(string userName)
        => new(DEFAULT_USER_NAME, userName);
    public static Claim CreateFirstNameClaim(string firstName)
        => new(DEFAULT_FIRST_NAME, firstName);

    public static Claim CreateLastNameClaim(string lastName)
        => new(DEFAULT_LAST_NAME, lastName);

    public static Claim CreateUserIdClaim(Guid userId)
        => new(DEFAULT_USER_ID, userId.ToString());
    public static Claim CreateUserEmailClaim(string email)
        => new(DEFAULT_USER_EMAIL, email);
    public static Claim CreateGymClaim(Guid gymId)
        => new(DEFAULT_GYM_ID, gymId.ToString());
    public static Claim CreateGymInvitationId(Guid invitationId)
        => new(DEFAULT_GYM_INVITATION_ID, invitationId.ToString());

    public static IReadOnlyList<Claim> CreateBasicUserClaims(
        Guid userId,
        string email,
        string userName,
        string firstName,
        string lastName,
        IEnumerable<Claim> others)
        => CreateBasicUserClaims(userId, email, userName, firstName, lastName, others.ToArray());

    /// <summary>
    /// Creates a new immutable array with all the specified claims with no duplications.
    /// </summary>
    public static IReadOnlyList<Claim> CreateBasicUserClaims(
        Guid userId,
        string email,
        string userName,
        string firstName,
        string lastName,
        params Claim[] others)
    {
        Claim[] claims = [
            CreateUserEmailClaim(email),
            CreateUserIdClaim(userId),
            CreateUserNameClaim(userName),
            CreateFirstNameClaim(firstName),
            CreateLastNameClaim(lastName),
        ];

        var nonDuplicatedClaims = claims
            .Concat(others)
            .ToHashSet(new ClaimEquatable());

        return nonDuplicatedClaims.ToImmutableArray();
    }

    private class ClaimEquatable
        : IEqualityComparer<Claim>
    {
        public bool Equals(Claim? x, Claim? y)
        {
            if (x is null ||  y is null) return false;

            var keyX = GetKey(x);
            var keyY = GetKey(y);

            return keyX.Equals(keyY, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] Claim obj)
        {
            var key = GetKey(obj);

            return key.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        private static string GetKey(Claim obj)
            => string.Concat(obj.Type, obj.Value);
    }
}
