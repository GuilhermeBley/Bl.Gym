
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
            var roleFound = 
                await _trainingContext
                .Roles
                .AsNoTracking()
                .Where(e => e.NormalizedName == role.NormalizedName)
                .Select(e => new { e.Id })
                .FirstOrDefaultAsync(cancellationToken);

            if (roleFound is null)
            {
                var roleAdded = await _trainingContext
                    .Roles
                    .AddAsync(new Model.Identity.RoleModel
                    {
                        ConcurrencyStamp = Guid.NewGuid(),
                        Name = role.Name,
                        NormalizedName = role.NormalizedName,
                    });

                roleFound = new { Id = roleAdded.Entity.Id };
            }

            foreach (var claim in role)
            {
                var containsClaim = 
                    await _trainingContext
                    .RoleClaims
                    .AsNoTracking()
                    .Where(e => e.RoleId == role.Id)
                    .Where(e => e.ClaimValue == claim.Value)
                    .Where(e => e.ClaimType == claim.Type)
                    .AnyAsync(cancellationToken);

                if (containsClaim)
                    continue;

                await _trainingContext
                    .RoleClaims
                    .AddAsync(new Model.Identity.RoleClaimModel
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value,
                        RoleId = roleFound.Id
                    });
            }
        }

        await _trainingContext.SaveChangesAsync(cancellationToken);
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
