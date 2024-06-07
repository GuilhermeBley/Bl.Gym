using MediatR;

namespace Bl.Gym.TrainingApi.Api.Endpoints;

public class TrainingEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
    {
        builder.MapGet("Training/details/{userId}", async (HttpContext context, IMediator mediator, Guid userId) =>
        {
            if (userId != context.User.RequiredUserId())
                return Results.Unauthorized();

            var result =
                await mediator.Send(new Application.Commands.Training.GetAllTrainingsByCurrentUser.GetAllTrainingsByCurrentUserRequest());

            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
