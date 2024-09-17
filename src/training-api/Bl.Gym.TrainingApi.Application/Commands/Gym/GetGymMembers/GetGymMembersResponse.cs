namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetGymMembers;

public record GetGymMembersResponse(
    Guid UserId,
    string Email,
    string Name,
    string LastName);
