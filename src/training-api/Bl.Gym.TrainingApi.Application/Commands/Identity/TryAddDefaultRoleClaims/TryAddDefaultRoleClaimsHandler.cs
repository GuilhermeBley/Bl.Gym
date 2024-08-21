
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.TryAddDefaultRoleClaims;

/// <summary>
/// Use this handler to development scenarios.
/// </summary>
public class TryAddDefaultRoleClaimsHandler
    : IRequestHandler<TryAddDefaultRoleClaimsRequest, TryAddDefaultRoleClaimsResponse>
{
    private static ImmutableArray<Role> _rolesToMap = new[]
    {
        Role.Admin,
        Role.GymGroupOwner,
        Role.Instructor,
        Role.Student,
    }
    .ToHashSet(new RoleComparer())
    .ToImmutableArray();

    private readonly TrainingContext _trainingContext;

    public TryAddDefaultRoleClaimsHandler(TrainingContext trainingContext)
    {
        _trainingContext = trainingContext;
    }

    public async Task<TryAddDefaultRoleClaimsResponse> Handle(
        TryAddDefaultRoleClaimsRequest request, 
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _trainingContext.Database.BeginTransactionAsync(cancellationToken);

        foreach (var role in _rolesToMap)
        {
            var containsRole = 
                await _trainingContext
                .Roles
                .AsNoTracking()
                .AnyAsync(e => e.NormalizedName == role.NormalizedName, cancellationToken);

            List<Claim> claimsToTryAdd = new List<Claim>();

            if (!containsRole)
            {
                
            }
        }

        await transaction.CommitAsync();

        return new TryAddDefaultRoleClaimsResponse();
    }

    private class RoleComparer
        : IEqualityComparer<Role>
    {
        public bool Equals(Role? x, Role? y)
        {
            if (x is null ||  y is null) return false;

            return x.NormalizedName.Equals(y.NormalizedName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] Role obj)
        {
            return obj.NormalizedName.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}
