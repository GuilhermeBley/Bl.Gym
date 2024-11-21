using Bl.Gym.TrainingApi.Domain.Security;

namespace Bl.Gym.TrainingApi.Api.Policies;

/// <summary>
/// defines a policy that only users user logged on a gym can access the endpoint.
/// </summary>
internal static class UserGymPolicy
{
    public const string POLICY_NAME = "UserGymPolicy";
    public const string REQUIRE_CLAIM_TYPE = UserClaim.DEFAULT_GYM_ID;
}
