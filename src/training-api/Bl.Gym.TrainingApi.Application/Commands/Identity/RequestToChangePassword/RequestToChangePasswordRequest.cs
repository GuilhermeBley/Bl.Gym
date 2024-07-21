
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RequestToChangePassword;

public class RequestToChangePasswordHandler
    : IRequestHandler<RequestToChangePasswordRequest, RequestToChangePasswordResponse>
{
    private readonly TrainingContext _context;
    private readonly ILogger<RequestToChangePasswordHandler> _logger;

    public RequestToChangePasswordHandler(
        TrainingContext context,
        ILogger<RequestToChangePasswordHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<RequestToChangePasswordResponse> Handle(
        RequestToChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var containsUser = await _context
            .Users
            .AsNoTracking()
            .Where(e => e.Email == request.Email)
            .AnyAsync(cancellationToken);

        if (containsUser)
        {
            //
            // Enqueue email message
            //
        }
        
        return new(containsUser);
    }
}