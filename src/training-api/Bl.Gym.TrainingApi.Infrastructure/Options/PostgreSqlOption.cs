namespace Bl.Gym.TrainingApi.Infrastructure.Options;

internal class PostgreSqlOption
{
    public const string SECTION = "PostgreSqlOption";

    public string ConnectionString { get; set; } = string.Empty;
}
