

namespace Bl.Gym.TrainingApi.Domain.Exceptions;

/// <summary>
/// This exception represents an unauthorized access error in some resource.
/// The user can't access the resource because he doesn't have the required role.
/// </summary>
public class ForbbidenCoreException
    : CoreException
{
    public override CoreExceptionCode StatusCode => CoreExceptionCode.Forbbiden;

    public ForbbidenCoreException()
        : this("The user does not contain the required role.")
    {
    }

    public ForbbidenCoreException(string? message) : base(message)
    {
    }

    public ForbbidenCoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
