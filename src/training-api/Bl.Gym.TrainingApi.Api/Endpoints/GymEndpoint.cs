using MediatR;

namespace Bl.Gym.TrainingApi.Api.Endpoints;

public class GymEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("gym/user/{userId}", async (HttpContext context, IMediator mediator, Guid userId) =>
        {
            if (userId != context.User.RequiredUserId())
                return Results.Unauthorized();

            var result =
                await mediator.Send(new Application.Commands.Gym.GetCurrentUserGym.GetCurrentUserGymsRequest(userId));

            if (result.Gyms.Any())
                return Results.Ok(result);

            return Results.NoContent();

        }).RequireAuthorization();
    }
}
