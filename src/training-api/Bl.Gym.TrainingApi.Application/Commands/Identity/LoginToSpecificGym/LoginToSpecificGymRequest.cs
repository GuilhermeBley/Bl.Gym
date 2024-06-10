namespace Bl.Gym.TrainingApi.Application.Commands.Identity.LoginToSpecificGym;

public record LoginToSpecificGymRequest(
    Guid GymId)
    : IRequest<LoginToSpecificGymResponse>;
