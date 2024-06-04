namespace Bl.Gym.TrainingApi.Infrastructure.Options;

internal class MySqlOption
{
    public const string SECTION = "MySqlOption";

    public string ConnectionString { get; set; } = string.Empty;
}
