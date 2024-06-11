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
    }
}
