namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetTrainingInfoById;

public record GetTrainingInfoByIdRequest(
    Guid TrainingId)
    : IRequest<GetTrainingInfoByIdResponse>;
