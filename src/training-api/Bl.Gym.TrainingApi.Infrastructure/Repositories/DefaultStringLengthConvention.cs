using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Infrastructure.Repositories;

internal class DefaultStringLengthConvention : IEntityTypeConfiguration<object>
{
    private readonly int _maxLength;

    public DefaultStringLengthConvention(int maxLength)
    {
        _maxLength = maxLength;
    }

    public void Configure(EntityTypeBuilder<object> builder)
    {
        foreach (var property in builder.Metadata.GetProperties())
        {
            if (property.ClrType == typeof(string) && !property.GetMaxLength().HasValue)
            {
                property.SetMaxLength(_maxLength);
            }
        }
    }
}
