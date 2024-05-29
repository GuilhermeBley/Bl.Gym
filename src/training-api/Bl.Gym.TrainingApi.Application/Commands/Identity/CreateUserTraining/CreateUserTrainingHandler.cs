
namespace Bl.Gym.TrainingApi.Application.Commands.Identity.CreateUserTraining;

public class CreateUserTrainingHandler
    : IRequestHandler<CreateUserTrainingRequest, CreateUserTrainingResponse>
{
    public Task<CreateUserTrainingResponse> Handle(
        CreateUserTrainingRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
