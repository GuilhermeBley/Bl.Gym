namespace Bl.Gym.TrainingApi.Domain.Enum;

public enum UserTrainingStatus
{
    /// <summary>
    /// Training to start
    /// </summary>
    ToStart = 0,
    /// <summary>
    /// Training in progress
    /// </summary>
    InProgress = 1,
    /// <summary>
    /// Training already finished
    /// </summary>
    Finished = 2,
    /// <summary>
    /// Training removed
    /// </summary>
    Removed
}
