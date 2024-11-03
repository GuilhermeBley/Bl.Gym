namespace Bl.Gym.TrainingApi.Application.Commands.Training.StartTrainingPeriod;

public record StartTrainingPeriodRequest(
    Guid TrainingSectionId)
    : IRequest<StartTrainingPeriodResponse>;
