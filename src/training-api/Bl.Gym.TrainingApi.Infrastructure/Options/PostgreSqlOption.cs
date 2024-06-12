namespace Bl.Gym.TrainingApi.Infrastructure.Options;

public class PostgreSqlOption
{
    public const string SECTION = "PostgreSqlOption";

    public string ConnectionString { get; set; } = string.Empty;
}
