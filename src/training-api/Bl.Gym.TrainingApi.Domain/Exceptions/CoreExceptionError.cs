namespace Bl.Gym.TrainingApi.Domain.Exceptions;

public record CoreExceptionError(
    CoreExceptionCode StatusCode,
    string? Message = null)
    : ICoreException;
