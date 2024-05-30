namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public record CreateTrainingToStudentRequest(
    Guid GymId,
    Guid StudentId)
    : IRequest<CreateTrainingToStudentResponse>;
