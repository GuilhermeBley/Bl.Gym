namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public record CreateTrainingToStudentRequest(
    Guid GymId)
    : IRequest<CreateTrainingToStudentResponse>;
