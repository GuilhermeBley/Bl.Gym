using Bl.Gym.TrainingApi.Api.Model.Gym;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        builder.MapPost("gym/createAsAdmin", async (
            HttpContext context, 
            IMediator mediator,
            [FromBody] CreateGymAsAdminModel model) =>
        {
            var userId = context.User.RequiredUserId();

            var result =
                await mediator.Send(new Application.Commands.Gym.CreateGymGroupByAdmin.CreateGymGroupByAdminRequest(
                    model.Name,
                    model.Description));

            return Results.Created($"gym/user/{userId}", new { result.GymCreatedId });

        }).RequireAuthorization(cfg =>
        {
            cfg.RequireRole(Domain.Security.UserClaim.ManageAnyGym.Value);
        });

        builder.MapGet("gym/{gymId}/members", async (
            Guid gymId, 
            IMediator mediator) =>
        {
            var result =
                await mediator.Send(new Application.Commands.Gym.GetGymMembers.GetGymMembersRequest(
                    gymId));

            return Results.Ok(result);

        }).RequireAuthorization();
    }
}
