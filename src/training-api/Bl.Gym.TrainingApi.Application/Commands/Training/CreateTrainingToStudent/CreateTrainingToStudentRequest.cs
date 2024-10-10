namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public record CreateTrainingToStudentRequest(
    Guid GymId,
    Guid StudentId,
    IEnumerable<CreateTrainingToStudentSection> Sections)
    : IRequest<CreateTrainingToStudentResponse>;

public record CreateTrainingToStudentSection(
    string MuscularGroup,
    IEnumerable<CreateTrainingToStudentSet> Sets);

public record CreateTrainingToStudentSet(
    string Set,
    Guid ExerciseId);