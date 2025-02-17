﻿using Bl.Gym.TrainingApi.Application.Model.Identity;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Extensions;

public static class ClaimsPrincipalExtension
{
    /// <summary>
    /// Get the user ID, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static Guid RequiredUserId(this ClaimsPrincipal principal)
        => GetUserId(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user ID or null.
    /// </summary>
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    /// <summary>
    /// Get the gym ID, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static Guid RequiredGymId(this ClaimsPrincipal principal)
        => GetGymId(principal)
        ?? throw new ForbbidenCoreException();

    /// <summary>
    /// Get the gym invitation ID, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static Guid RequiredGymInvitationId(this ClaimsPrincipal principal)
        => GymInvitationId(principal)
        ?? throw new ForbbidenCoreException();

    /// <summary>
    /// This method ensures that the <paramref name="gymIdToCheck"/> is the same of the current user.
    /// </summary>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void EnsureGymId(this ClaimsPrincipal principal, Guid gymIdToCheck)
    {
        var currentGymId = principal.RequiredGymId();

        if (gymIdToCheck == currentGymId)
            return;

        throw new ForbbidenCoreException();
    }

    /// <summary>
    /// Get the gym ID or null.
    /// </summary>
    public static Guid? GetGymId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_GYM_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    /// <summary>
    /// Get the gym invitation ID or null.
    /// </summary>
    public static Guid? GymInvitationId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_GYM_INVITATION_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    /// <summary>
    /// Get the user email, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredUserEmail(this ClaimsPrincipal principal)
        => GetUserEmail(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user email, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredFirstName(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_FIRST_NAME);

        if (claim is null)
            throw new UnauthorizedCoreException();

        return claim.Value;
    }

    /// <summary>
    /// Get the user email, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredLastName(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_LAST_NAME);

        if (claim is null)
            throw new UnauthorizedCoreException();

        return claim.Value;
    }

    /// <summary>
    /// Get the user email or null.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    /// <summary>
    /// Get the user name, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredUserName(this ClaimsPrincipal principal)
        => GetUserName(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user name or null.
    /// </summary>
    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    public static bool IsInRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        if (principal.Identities.Any(id => id.RoleClaimType == roleClaim.Type))
            return false;
        
        return principal.IsInRole(roleClaim.Value);
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfIsnInTheGym(this ClaimsPrincipal principal, Guid gymId)
    {
        ThrowIfIsntLogged(principal);

        var claim = principal.Claims.FirstOrDefault(
            e => e.Type == Domain.Security.UserClaim.DEFAULT_GYM_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var claimGymId) ||
            claimGymId != gymId)
        {
            throw new ForbbidenCoreException($"Your current claim is not assigned to gym {gymId}.");
        }
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        ThrowIfIsntLogged(principal);

        if (!principal.IsInRole(roleClaim.Value))
            throw new ForbbidenCoreException();
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, string role)
    {
        ThrowIfIsntLogged(principal);

        if (!principal.IsInRole(role))
            throw new ForbbidenCoreException();
    }

    public static void ThrowIfIsntLogged(this ClaimsPrincipal principal)
    {
        if (!IsLogged(principal))
            throw new UnauthorizedCoreException();
    }

    public static bool IsLogged(this ClaimsPrincipal principal)
    {
        return principal.HasClaim(p => p.Type == Domain.Security.UserClaim.DEFAULT_USER_ID);
    }
}
