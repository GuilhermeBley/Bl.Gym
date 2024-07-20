using Bl.Gym.TrainingApi.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bl.Gym.TrainingApi.Api.Endpoints;

public class UserEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
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
                    response.Email,
                    response.Username,
                    Token = tokenResult
                });
        });

        builder.MapPatch("user/change-password", async (
            Application.Commands.Identity.ChangePassword.ChangePasswordRequest request,
            IMediator mediator) =>
        {
            var response = await mediator.Send(request);

            return Results.Ok();
        });
    }
}
