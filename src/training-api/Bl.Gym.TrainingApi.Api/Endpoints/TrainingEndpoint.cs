using MediatR;
using Microsoft.AspNetCore.Mvc;

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

            if (result.Any())
                return Results.Ok(result);

            return Results.NoContent();

        }).RequireAuthorization();

        builder.MapGet("Training/details/{userId}/sheet/{sheetId}", async (
            HttpContext context, 
            IMediator mediator, 
            Guid userId,
            Guid sheetId) =>
        {
            if (userId != context.User.RequiredUserId())
                return Results.Unauthorized();

            var result =
                await mediator.Send(new Application.Commands.Training.GetTrainingInfoById.GetTrainingInfoByIdRequest(sheetId));

            return Results.Ok(result);

        }).RequireAuthorization();

        builder.MapPost("Training", async (
            HttpContext context, 
            IMediator mediator, 
            [FromBody] Application.Commands.Training.CreateUserTraining.CreateTrainingToStudentRequest request) =>
        {
            var result =
                await mediator.Send(request);

            return Results.Ok(result);
        }).RequireAuthorization();

        builder.MapPatch("Training/{sectionId}/update-current-training-days", async (
            HttpContext context, 
            IMediator mediator, 
            Guid sectionId,
            [FromBody] Application.Commands.Training.UpdateCurrentDaysCountFromSection.UpdateCurrentDaysCountFromSectionRequest request) =>
        {
            if (sectionId != request.SectionId)
                return Results.Unauthorized();

            var result =
                await mediator.Send(request);

            return Results.Ok(result);
        }).RequireAuthorization();

        builder.MapGet("Training/exercises/gym/{gymId}", async (
            HttpContext context,
            Guid gymId,
            [FromServices] IMediator mediator,
            [FromQuery] int skip,
            [FromQuery] int take) =>
        {
            var result =
                await mediator.Send(
                    new Application.Commands.Training.GetAvailableExercises.GetAvailableExercisesRequest(gymId, skip, take));

            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
