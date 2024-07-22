
using Bl.Gym.EventBus;
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RequestToChangePassword;

public class RequestToChangePasswordHandler
    : IRequestHandler<RequestToChangePasswordRequest, RequestToChangePasswordResponse>
{
    private readonly TrainingContext _context;
    private readonly ILogger<RequestToChangePasswordHandler> _logger;
    private readonly IEventBus _eventBus;

    public RequestToChangePasswordHandler(
        TrainingContext context,
        ILogger<RequestToChangePasswordHandler> logger,
        IEventBus eventBus)
    {
        _context = context;
        _logger = logger;
        _eventBus = eventBus;
    }
    
    public async Task<RequestToChangePasswordResponse> Handle(
        RequestToChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context
            .Users
            .AsNoTracking()
            .Where(e => e.Email == request.Email)
            .Select(e => new { e.Id })
            .FirstOrDefaultAsync(cancellationToken);

        if (user != null)
        {
            //
            // Enqueue message to send to email
            //
            await _eventBus.SendMessageAsync(
                new EventBus.Events.UserRequestingToChangePasswordEvent()
                {
                    UserId = user!.Id
                });
            return new(true);
        }
        
        return new(false);
    }
}