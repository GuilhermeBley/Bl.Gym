namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetGymMembers;

public record GetGymMembersRequest(
    Guid GymId)
    : IRequest<GetGymMembersResponse>;
