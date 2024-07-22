namespace Bl.Gym.TrainingApi.Infrastructure.Options;

public class AzureEventbusOption
{
    public const string SECTION = nameof(AzureEventbusOption);
    public string ConnectionsString { get; set; } = string.Empty;
}
