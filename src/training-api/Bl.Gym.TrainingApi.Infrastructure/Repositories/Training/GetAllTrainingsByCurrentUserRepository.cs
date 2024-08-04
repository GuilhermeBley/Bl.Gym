using Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;
using Bl.Gym.TrainingApi.Application.Repositories.Training;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Remoting;

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

    public async Task<IEnumerable<GetAllTrainingsByCurrentUserResponse>> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return (await _context
            .Database
            .GetDbConnection()
            .QueryAsync<GetAllTrainingsByCurrentUserResponsePostgree>(
                new CommandDefinition(
                    """
                    SELECT
                    	sheet."Id" TrainingId,
                    	sheet."GymId" GymId,
                    	gym."Name" GymName,
                    	gym."Description" GymDescription,
                    	sheet."CreatedAt" TrainingCreatedAt,
                    	COUNT(*) SectionsCount
                    FROM public."UserTrainingSheets" sheet
                    INNER JOIN public."GymGroups" gym
                    	ON sheet."GymId" = gym."Id"
                    INNER JOIN public."TrainingSections" trainingSection
                    	ON trainingSection."UserTrainingSheetId" = sheet."Id"
                    WHERE sheet."StudentId" = @StudentId
                    GROUP BY sheet."Id", sheet."GymId", gym."Name", gym."Description"
                    """,
                    parameters: new
                    {
                        StudentId = userId,
                    },
                    cancellationToken: cancellationToken)))
            .Select(e => (GetAllTrainingsByCurrentUserResponse)e);
    }

    private class GetAllTrainingsByCurrentUserResponsePostgree
    {
        public Guid TrainingId { get; set; }
        public Guid GymId { get; set; }
        public string GymName { get; set; } = string.Empty;
        public string GymDescription { get; set; } = string.Empty;
        public DateTimeOffset TrainingCreatedAt { get; set; }
        public int SectionsCount { get; set; }

        public static implicit operator GetAllTrainingsByCurrentUserResponse(GetAllTrainingsByCurrentUserResponsePostgree other)
        {
            return new GetAllTrainingsByCurrentUserResponse(
                TrainingId: other.TrainingId,
                GymId: other.GymId,
                GymName: other.GymName,
                GymDescription: other.GymDescription,
                TrainingCreatedAt: other.TrainingCreatedAt,
                SectionsCount: other.SectionsCount);
        }
    }
}
