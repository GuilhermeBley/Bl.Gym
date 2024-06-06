namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

public record GetAllTrainingsByCurrentUserResponse(
    Guid TrainingId,
    Guid GymId,
    string GymName,
    string GymDescription,
    DateTimeOffset TrainingCreatedAt,
    int SectionsCount);
