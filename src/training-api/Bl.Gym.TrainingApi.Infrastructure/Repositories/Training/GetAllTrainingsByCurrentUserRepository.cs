using Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;
using Bl.Gym.TrainingApi.Application.Repositories.Training;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Infrastructure.Repositories.Training;

public class GetAllTrainingsByCurrentUserRepository
    : IGetAllTrainingsByCurrentUserRepository
{
    private readonly MySqlTrainingContext _context;

    public GetAllTrainingsByCurrentUserRepository(
        MySqlTrainingContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<GetAllTrainingsByCurrentUserResponse>> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _context
            .Database
            .GetDbConnection()
            .QueryAsync<GetAllTrainingsByCurrentUserResponse>(
                new CommandDefinition(
                    """
                    SELECT 
                        TrainingId
                        GymId
                        GymName
                        GymDescription
                        TrainingCreatedAt
                        SectionsCount
                    FROM 

                    """,
                    parameters: new
                    {
                        userId
                    },
                    cancellationToken: cancellationToken));
    }
}
