
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Repositories.Training;
using System;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

public class GetAllTrainingsByCurrentUserHandler
    : IRequestHandler<GetAllTrainingsByCurrentUserRequest, IEnumerable<GetAllTrainingsByCurrentUserResponse>>
{
    private readonly TrainingContext _context;
    private readonly ILogger<GetAllTrainingsByCurrentUserHandler> _logger;
    private readonly IIdentityProvider _identityProvider;
    private readonly IGetAllTrainingsByCurrentUserRepository _repository;

    public GetAllTrainingsByCurrentUserHandler(
        TrainingContext context, 
        ILogger<GetAllTrainingsByCurrentUserHandler> logger, 
        IIdentityProvider identityProvider,
        IGetAllTrainingsByCurrentUserRepository repository)
    {
        _context = context;
        _logger = logger;
        _identityProvider = identityProvider;
        _repository = repository;
    }

    public async Task<IEnumerable<GetAllTrainingsByCurrentUserResponse>> Handle(
        GetAllTrainingsByCurrentUserRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync();

        var id = user.RequiredUserId();

        return await _repository.GetAsync(id, cancellationToken);
    }
}
