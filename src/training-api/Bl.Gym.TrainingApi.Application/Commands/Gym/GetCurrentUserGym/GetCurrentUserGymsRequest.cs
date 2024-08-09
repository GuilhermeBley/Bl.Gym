namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetCurrentUserGym;

public record GetCurrentUserGymsRequest(
    Guid UserId)
    : IRequest<GetCurrentUserGymsResponse>;