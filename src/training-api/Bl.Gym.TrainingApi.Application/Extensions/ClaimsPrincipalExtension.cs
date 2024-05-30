using Bl.Gym.TrainingApi.Application.Model.Identity;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.ValueType == Domain.Security.UserClaim.DEFAULT_USER_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.ValueType == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.ValueType == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    public static bool IsInRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        if (principal.Identities.Any(id => id.RoleClaimType == roleClaim.ValueType))
            return false;
        
        return principal.IsInRole(roleClaim.Value);
    }

    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        if (!IsLogged(principal))
            throw new UnauthorizedCoreException();

        if (principal.Identities.Any(id => id.RoleClaimType == roleClaim.ValueType))
            throw new ForbbidenCoreException();

        if (!principal.IsInRole(roleClaim.Value))
            throw new ForbbidenCoreException();
    }

    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, string role)
    {
        if (!IsLogged(principal))
            throw new UnauthorizedCoreException();

        if (!principal.IsInRole(role))
            throw new ForbbidenCoreException();
    }

    public static bool IsLogged(this ClaimsPrincipal principal)
    {
        return principal.HasClaim(p => p.ValueType == Domain.Security.UserClaim.DEFAULT_USER_ID);
    }
}
