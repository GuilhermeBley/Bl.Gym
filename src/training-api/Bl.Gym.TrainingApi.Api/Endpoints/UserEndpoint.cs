﻿using Azure.Core;
using Bl.Gym.TrainingApi.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Bl.Gym.TrainingApi.Api.Endpoints;

public class UserEndpoint
{
    public static void MapEndpoints(
        IEndpointRouteBuilder builder,
        bool isDevelopment)
    {
        builder.MapPost("user", async (
            [FromBody]Application.Commands.Identity.CreateUser.CreateUserRequest request,
            IMediator mediator) =>
        {
            var response = await mediator.Send(request);

            return Results.Created();
        });

        builder.MapPost("user/login", async (
            [FromBody]Application.Commands.Identity.Login.LoginRequest request,
            IMediator mediator,
            TokenGeneratorService tokenGenerator) =>
        {
            var response = await mediator.Send(request);

            var tokenResult = tokenGenerator.Generate(
                response.Claims.ToArray(),
                DateTime.UtcNow.AddHours(2));

            return Results.Ok(
                new
                {
                    response.RefreshToken,
                    response.Email,
                    response.Username,
                    response.FirstName,
                    response.LastName,
                    Token = tokenResult
                });
        });

        builder.MapPost("user/login/gym", async (
            [FromBody]Application.Commands.Identity.LoginToSpecificGym.LoginToSpecificGymRequest request,
            IMediator mediator,
            HttpContext context,
            TokenGeneratorService tokenGenerator) =>
        {
            // Extract the JWT token from the user's claims
            var expClaim = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expClaim == null || !long.TryParse(expClaim.Value, out long expSeconds))
            {
                return Results.BadRequest();
            }

            //
            // Always keep the same expiration date.
            //
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

            var response = await mediator.Send(request);

            var tokenResult = tokenGenerator.Generate(
                response.Claims.ToArray(),
                expirationDate);

            return Results.Ok(
                new
                {
                    response.RefreshToken,
                    response.Email,
                    response.Username,
                    response.FirstName,
                    response.LastName,
                    response.GymId,
                    Token = tokenResult
                });
        }).RequireAuthorization();

        builder.MapPost("user/change-password/request", async (
            [FromBody] Application.Commands.Identity.RequestToChangePassword.RequestToChangePasswordRequest request,
            [FromServices] IMediator mediator
        ) => {

            await Task.CompletedTask;
        });

        builder.MapPatch("user/change-password", async (
            [FromBody] Application.Commands.Identity.ChangePassword.ChangePasswordRequest request,
            [FromServices] IMediator mediator) =>
        {
            var response = await mediator.Send(request);

            return Results.Ok();
        }).RequireAuthorization(cfg => {
            cfg.AddAuthenticationSchemes(Bl.Gym.TrainingApi.Api.Policies.ForgotPasswordPolicy.Scheme);
            cfg.RequireRole(Policies.ForgotPasswordPolicy.RequireRole.Value);
        });

        builder.MapPatch("user/refresh", async (
            [FromBody] Application.Commands.Identity.RefreshToken.RefreshTokenRequest request,
            [FromServices] IMediator mediator,
            [FromServices] TokenGeneratorService tokenGenerator) =>
        {
            var response = await mediator.Send(request);

            var tokenResult = tokenGenerator.Generate(
                response.Claims.ToArray(),
                DateTime.UtcNow.AddHours(2));

            return Results.Ok(
                new
                {
                    response.RefreshToken,
                    response.Email,
                    response.Username,
                    Token = tokenResult
                });
        });

        if (isDevelopment)
            MapDevelopment(builder);
    }

    private static void MapDevelopment(IEndpointRouteBuilder builder)
    {
        builder.MapPost("roles/default-roles", async (
            [FromServices] IMediator mediator) =>
        {
            var response = await mediator.Send(
                new Application.Commands.Identity.TryAddDefaultRoleClaims.TryAddDefaultRoleClaimsRequest());

            return Results.Ok();
        });
    }
}
