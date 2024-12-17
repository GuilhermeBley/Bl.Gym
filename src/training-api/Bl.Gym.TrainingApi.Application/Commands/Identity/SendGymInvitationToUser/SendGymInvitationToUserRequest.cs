using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public delegate Uri UriTokenInvitationGenerator(Claim[] claims, DateTime ExpiresAt);

public record SendGymInvitationToUserRequest(
    string Email,
    Guid GymId,
    UriTokenInvitationGenerator Provider)
    : IRequest<SendGymInvitationToUserResponse>;
