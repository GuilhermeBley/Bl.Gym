namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public record SendGymInvitationToUserRequest(
    string Email,
    Guid GymId)
    : IRequest<SendGymInvitationToUserResponse>;
