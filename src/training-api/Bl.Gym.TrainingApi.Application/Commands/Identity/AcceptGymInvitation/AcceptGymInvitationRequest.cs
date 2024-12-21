namespace Bl.Gym.TrainingApi.Application.Commands.Identity.AcceptGymInvitation;

public record AcceptGymInvitationRequest(
    Guid InvitationId)
    : IRequest<AcceptGymInvitationResponse>;
