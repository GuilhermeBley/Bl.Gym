namespace Bl.Gym.TrainingApi.Application.Commands.Training.UpdateCurrentDaysCountFromSection;

[Obsolete("Use 'FinishTrainingPeriodHandler'.")]
public record UpdateCurrentDaysCountFromSectionRequest(
    Guid SectionId,
    int NewCurrentDaysCount)
    : IRequest<Guid>;
