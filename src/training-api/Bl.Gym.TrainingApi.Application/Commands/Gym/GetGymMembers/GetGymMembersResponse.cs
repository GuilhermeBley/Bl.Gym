namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetGymMembers;

public record GetGymMembersResponse(
    GetGymMembersItemResponse[] Students);

public record GetGymMembersItemResponse(
    Guid UserId,
    string Email,
    string Name,
    string LastName);
