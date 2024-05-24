namespace Bl.Gym.TrainingApi.Domain.Exceptions;

public class CommonCoreException : CoreException
{
    public const string DefaultMessage = "BadRequest";
    public override CoreExceptionCode StatusCode => CoreExceptionCode.BadRequest;
    public CommonCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}
