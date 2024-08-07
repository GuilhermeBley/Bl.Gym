namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetCurrentUserGym;

public record GetCurrentUserGymsRequest(
    Guid UserId)
    : IRequest<GetCurrentUserGymsResponse>;