using Bl.Gym.TrainingApi.Application.Providers;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public class CreateTrainingToUserHandler
    : IRequestHandler<CreateTrainingToUserRequest, CreateTrainingToUserResponse>
{
    private readonly IIdentityProvider _identityProvider;
    public Task<CreateTrainingToUserResponse> Handle(
        CreateTrainingToUserRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
