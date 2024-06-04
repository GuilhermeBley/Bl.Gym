
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using System;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

public class GetAllTrainingsByCurrentUserHandler
    : IRequestHandler<GetAllTrainingsByCurrentUserRequest, IEnumerable<GetAllTrainingsByCurrentUserResponse>>
{
    private readonly TrainingContext _context;
    private readonly ILogger<GetAllTrainingsByCurrentUserHandler> _logger;
    private readonly IIdentityProvider _identityProvider;

    public GetAllTrainingsByCurrentUserHandler(TrainingContext context, ILogger<GetAllTrainingsByCurrentUserHandler> logger, IIdentityProvider identityProvider)
    {
        _context = context;
        _logger = logger;
        _identityProvider = identityProvider;
    }

    public async Task<IEnumerable<GetAllTrainingsByCurrentUserResponse>> Handle(
        GetAllTrainingsByCurrentUserRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync();

        var id = user.RequiredUserId();

        return await
            (from roleGym in _context.UserTrainingRoles.AsNoTracking()
             join trainingSheet in _context.UserTrainingSheets.AsNoTracking()
                 on new {
                     GymId = roleGym.GymGroupId,
                     UserId = roleGym.UserId
                 } equals new
                 {
                     GymId = trainingSheet.GymId,
                     UserId = id
                 }
             join gym in _context.GymGroups.AsNoTracking()
                 on roleGym.GymGroupId equals gym.Id
             join section in _context.TrainingSections.AsNoTracking()
                 on trainingSheet.Id equals section.UserTrainingSheetId
             select new GetAllTrainingsByCurrentUserResponse(
                 trainingSheet.Id,
                 gym.Id,
                 gym.Name,
                 gym.Description,
                 trainingSheet.CreatedAt,
                 _context.TrainingSections.AsNoTracking().Count(e => e.UserTrainingSheetId == trainingSheet.Id)))
            .ToListAsync(cancellationToken);
    }
}
