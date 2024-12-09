using Bl.Gym.TrainingApi.Domain.Primitive;

namespace Bl.Gym.TrainingApi.Domain.Test.Mocks;

internal class StaticDateTimeProvider
    : IDateTimeProvider
{
    /// <summary>
    /// Always starts with the current machine date, but you can change it later.
    /// </summary>
    public DateTime UtcNow {  get; set; } = DateTime.UtcNow;

    public void SimulateTimeGoing(TimeSpan time)
    {
        UtcNow = UtcNow.Add(time);
    }
}
