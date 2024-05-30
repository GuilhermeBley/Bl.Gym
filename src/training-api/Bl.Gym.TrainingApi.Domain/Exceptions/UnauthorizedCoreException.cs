

namespace Bl.Gym.TrainingApi.Domain.Exceptions;

/// <summary>
/// This exception represents an unauthorized access error in some resource.
/// The user can't access the resource because he isn't logged.
/// </summary>
public class UnauthorizedCoreException
    : CoreException
{
    public override CoreExceptionCode StatusCode => CoreExceptionCode.Unauthorized;

    public UnauthorizedCoreException()
        : this("The user is not logged in.")
    {
    }

    public UnauthorizedCoreException(string? message) : base(message)
    {
    }

    public UnauthorizedCoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
