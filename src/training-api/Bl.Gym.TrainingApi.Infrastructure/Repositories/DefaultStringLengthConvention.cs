using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Infrastructure.Repositories;

internal static class DefaultStringLengthConvention
{
    public static void Apply(ModelBuilder modelBuilder, int maxLength)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                {
                    property.SetMaxLength(maxLength);
                }
            }
        }
    }
}
