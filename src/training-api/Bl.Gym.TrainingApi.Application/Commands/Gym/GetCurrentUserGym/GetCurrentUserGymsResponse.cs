namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetCurrentUserGym;

public record GetCurrentUserGymResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    string Role,
    bool IsInvite);

public record GetCurrentUserGymsResponse(
    IEnumerable<GetCurrentUserGymResponse> Gyms);
