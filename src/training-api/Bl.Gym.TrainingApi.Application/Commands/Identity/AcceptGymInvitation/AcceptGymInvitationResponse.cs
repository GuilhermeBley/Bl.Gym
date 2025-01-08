namespace Bl.Gym.TrainingApi.Application.Commands.Identity.AcceptGymInvitation;

public record AcceptGymInvitationResponse(
    AcceptGymInvitationStatusResponse Status);

public enum AcceptGymInvitationStatusResponse
{
    Accepted,
    EmailIsNotRegistered
}