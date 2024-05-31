namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public record CreateTrainingToStudentRequest(
    Guid GymId,
    Guid StudentId,
    IEnumerable<CreateTrainingToStudentSet> Sets)
    : IRequest<CreateTrainingToStudentResponse>;


public record CreateTrainingToStudentSet(
    string Set,
    Guid ExerciseId);