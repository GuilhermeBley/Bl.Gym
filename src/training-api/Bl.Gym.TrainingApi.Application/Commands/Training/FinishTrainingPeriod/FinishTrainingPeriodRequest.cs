namespace Bl.Gym.TrainingApi.Application.Commands.Training.FinishTrainingPeriod;

public record FinishTrainingPeriodRequest(
    Guid PeriodId)
    : IRequest<FinishTrainingPeriodResponse>;
