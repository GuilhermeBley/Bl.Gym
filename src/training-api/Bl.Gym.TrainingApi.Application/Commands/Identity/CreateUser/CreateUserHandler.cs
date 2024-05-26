using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly TrainingContext _context;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(TrainingContext context, ILogger<CreateUserHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateUserResponse> Handle(
        CreateUserRequest request, 
        CancellationToken cancellationToken)
    {
        var entityToAdd =
            User.CreateWithHashedPassowrd(
                id: Guid.NewGuid(),
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                password: request.Password,
                phoneNumber: request.PhoneNumber)
            .RequiredResult;

        var userAlreadyCreated = await _context.Users.AnyAsync(
            e => e.NormalizedUserName == entityToAdd.NormalizedUserName,
            cancellationToken);

        if (userAlreadyCreated)
        {
            _logger.LogError("User {0} already registered.", entityToAdd.NormalizedUserName);
            throw AggregateCoreException.Create(CoreExceptionCode.Conflict);
        }

        await using var transaction
            = await _context.Database.BeginTransactionAsync(cancellationToken);

        var entityAddedResult
            = await _context.Users.AddAsync(
                Model.Identity.UserModel.MapFromEntity(entityToAdd));

        await transaction.CommitAsync();
        await _context.SaveChangesAsync();

        return new(entityAddedResult.Entity.Id);
    }
}
