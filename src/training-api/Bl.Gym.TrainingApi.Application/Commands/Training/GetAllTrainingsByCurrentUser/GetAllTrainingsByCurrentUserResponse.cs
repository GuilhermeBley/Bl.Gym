namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

public record GetAllTrainingsByCurrentUserResponse(
    Guid TrainingId,
    Guid GymId,
    string GymName,
    string GymDescription,
    DateTime TrainingCreatedAt,
    int SectionsCount);
