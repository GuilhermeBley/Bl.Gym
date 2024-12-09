using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using Bl.Gym.TrainingApi.Domain.Exceptions;

namespace Bl.Gym.TrainingApi.Domain.Test.Entities;

public class UserGymInvitationTest
{
    [Fact]
    public void Create_ShouldCreateAValidEntity()
    {
        var entity = Create();

        Assert.NotNull(entity);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid email")]
    [InlineData("invalid@email")]
    public void Create_ShouldFailBecauseHaveInvalidEmail(string invalidEmail)
    {
        var act = () => Create(userEmail: invalidEmail);

        Assert.ThrowsAny<CoreException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid group")]
    public void Create_ShouldFailBecauseHaveInvalidGymGroupRole(string invalidGymGroupRole)
    {
        var act = () => Create(gymGroupRole: invalidGymGroupRole);

        Assert.ThrowsAny<CoreException>(act);
    }

    [Fact]
    public void Equal_ShoudBeTrue()
    {
        var entity = Create();

        Assert.Equal(entity, entity);
    }

    [Fact]
    public void Equal_ShoudBeFalse()
    {
        var entity1 = Create();
        var entity2 = Create();

        Assert.NotEqual(entity1, entity2);
    }

    [Fact]
    public void IsExpired_ShoudBeExpired()
    {
        var dateProvider = new Mocks.StaticDateTimeProvider();

        var entity = Create(expiresAt: dateProvider.UtcNow.AddMinutes(1));

        dateProvider.SimulateTimeGoing(
            TimeSpan.FromMinutes(2));

        Assert.True(entity.IsExpired(dateProvider));
    }

    [Fact]
    public void IsExpired_ShoudNotBeExpired()
    {
        var dateProvider = new Mocks.StaticDateTimeProvider();

        var entity = Create(expiresAt: dateProvider.UtcNow.AddMinutes(3));

        dateProvider.SimulateTimeGoing(
            TimeSpan.FromMinutes(2));

        Assert.False(entity.IsExpired(dateProvider));
    }



    public static UserGymInvitation Create(
        Guid id = default,
        Guid invitedByUserId = default,
        string userEmail = "email@email.com",
        Guid gymId = default,
        DateTime? expiresAt = null,
        string gymGroupRole = nameof(Domain.Entities.Identity.Role.Student),
        DateTime? createdAt = null)
    {
        return UserGymInvitation.Create(
            id: id,
            invitedByUserId: invitedByUserId,
            userEmail: userEmail,
            gymId: gymId,
            expiresAt: expiresAt ?? DateTime.UtcNow.AddDays(1),
            gymGroupRole: gymGroupRole,
            createdAt: createdAt ?? DateTime.UtcNow)
            .RequiredResult;
    }
}
