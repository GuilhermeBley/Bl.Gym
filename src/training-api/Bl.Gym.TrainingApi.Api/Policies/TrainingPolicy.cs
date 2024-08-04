using Bl.Gym.TrainingApi.Domain.Security;

namespace Bl.Gym.TrainingApi.Api.Policies;

/// <summary>
/// defines a policy that only users can access the endpoint
/// </summary>
internal static class TrainingPolicy
{
    public const string POLICY_NAME = "TrainingPolicy";
    public const string REQUIRE_CLAIM_TYPE = UserClaim.DEFAULT_USER_ID;
}
