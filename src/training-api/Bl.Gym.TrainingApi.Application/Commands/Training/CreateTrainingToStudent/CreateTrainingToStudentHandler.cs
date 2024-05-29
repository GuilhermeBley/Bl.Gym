using Bl.Gym.TrainingApi.Application.Providers;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

/// <summary>
/// Handler to add a new training sheet to a specific student. 
/// Only the instructor can attribute the training sheet to the 
/// users, and the instructor and his students need to be 
/// registered at the same gym.
/// </summary>
public class CreateTrainingToStudentHandler
    : IRequestHandler<CreateTrainingToStudentRequest, CreateTrainingToStudentResponse>
{
    private readonly IIdentityProvider _identityProvider;
    public Task<CreateTrainingToStudentResponse> Handle(
        CreateTrainingToStudentRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
