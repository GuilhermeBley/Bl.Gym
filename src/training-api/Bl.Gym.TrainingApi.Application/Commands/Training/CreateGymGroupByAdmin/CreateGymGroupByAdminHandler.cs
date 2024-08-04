
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Training;
using System.Xml.Linq;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateGymGroupByAdmin;

/// <summary>
/// This handler creates and assign the new gym to the current user.
/// </summary>
public class CreateGymGroupByAdminHandler
    : IRequestHandler<CreateGymGroupByAdminRequest, CreateGymGroupByAdminResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly ILogger<CreateGymGroupByAdminHandler> _logger;
    private readonly TrainingContext _trainingContext;

    public CreateGymGroupByAdminHandler(
        IIdentityProvider identityProvider, 
        ILogger<CreateGymGroupByAdminHandler> logger, 
        TrainingContext trainingContext)
    {
        _identityProvider = identityProvider;
        _logger = logger;
        _trainingContext = trainingContext;
    }

    public async Task<CreateGymGroupByAdminResponse> Handle(
        CreateGymGroupByAdminRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);
        var userId = user.RequiredUserId();

        //
        // This role is applied just to admins create new gym.
        //
        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.ManageAnyGym);

        var entityCreated = GymGroup.Create(
            id: Guid.NewGuid(),
            name: request.Name,
            description: request.Description,
            createdAt: DateTime.UtcNow)
            .RequiredResult;

        using var transaction = await _trainingContext.Database.BeginTransactionAsync(cancellationToken);

        var gymAddedResult = await _trainingContext.GymGroups.AddAsync(
            Model.Training.GymGroupModel.MapFromEntity(entityCreated),
            cancellationToken);

        await _trainingContext.UserTrainingRoles.AddAsync(
            new Model.Identity.UserRoleTrainingModel{
                GymGroupId = gymAddedResult.Entity.Id,
                RoleId = 1,
                UserId = userId
            });

        transaction.Commit();

        await _trainingContext.SaveChangesAsync();
    }
}
