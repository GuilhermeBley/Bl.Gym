namespace Bl.Gym.TrainingApi.Application.Commands.Training.UpdateCurrentDaysCountFromSection;

public record UpdateCurrentDaysCountFromSectionRequest(
    Guid SectionId,
    int NewCurrentDaysCount)
    : IRequest<Guid>;
