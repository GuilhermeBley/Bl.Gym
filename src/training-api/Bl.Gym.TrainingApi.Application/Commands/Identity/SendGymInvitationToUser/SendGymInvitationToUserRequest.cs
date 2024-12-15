namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public record SendGymInvitationToUserRequest(
    string userEmail)
    : IRequest<SendGymInvitationToUserResponse>;
