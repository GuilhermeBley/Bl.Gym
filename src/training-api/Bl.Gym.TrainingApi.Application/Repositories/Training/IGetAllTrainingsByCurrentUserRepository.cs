using Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

namespace Bl.Gym.TrainingApi.Application.Repositories.Training;

/// <summary>
/// Repository created to perform 'GetAllTrainingsByCurrentUserResponse' result.
/// </summary>
public interface IGetAllTrainingsByCurrentUserRepository
{
    Task<IEnumerable<GetAllTrainingsByCurrentUserResponse>> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
