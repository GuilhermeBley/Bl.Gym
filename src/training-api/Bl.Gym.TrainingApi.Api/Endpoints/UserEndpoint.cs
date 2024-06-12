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
            IMediator mediator) =>
        {
            var response = await mediator.Send(request);

            return Results.Ok();
        });

        builder.MapPost("user/login/gym/{gymId}", async (
            Guid gymId,
            IMediator mediator) =>
        {
            var request
                = new Application.Commands.Identity.LoginToSpecificGym.LoginToSpecificGymRequest(gymId);

            var response = await mediator.Send(request);

            return Results.Ok();
        });
    }
}
