using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bl.Gym.TrainingApi.Infrastructure.Repositories;

internal static class ConversionUtils
{
    public static ValueConverter DateTimeToUtcDateTimeConversion
        = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),  // Convert to UTC when storing
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));// Ensure DateTimeKind.Utc when reading

    public static ValueConverter DateoffSetToUtcDateoffSetConversion
        = new ValueConverter<DateTimeOffset, DateTimeOffset>(
        v => v.ToUniversalTime(),                  // Convert to UTC when storing
        v => v);
}
