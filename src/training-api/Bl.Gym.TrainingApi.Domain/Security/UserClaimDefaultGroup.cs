using System.Collections;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Domain.Security;

public class UserClaimDefaultGroup
    : IReadOnlyList<Claim>
{
    public static UserClaimDefaultGroup Student
        => new(
            new[]
            {
                UserClaim.SeeTraining
            }
        );

    public static UserClaimDefaultGroup Teacher
        => new(
            new[]
            {
                UserClaim.ManageTraining,
                UserClaim.SeeTraining
            }
        );

    private readonly ImmutableArray<Claim> _claims;

    private UserClaimDefaultGroup(IEnumerable<Claim> claims)
        => _claims = claims.ToImmutableArray();

    public Claim this[int index] => _claims[index];

    public int Count => _claims.Length;

    public IEnumerator<Claim> GetEnumerator()
    {
        return _claims.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _claims.AsEnumerable().GetEnumerator();
    }
}
