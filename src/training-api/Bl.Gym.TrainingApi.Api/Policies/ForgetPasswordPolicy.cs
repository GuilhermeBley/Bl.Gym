using System.Security.Claims;
using Bl.Gym.TrainingApi.Domain.Security;

namespace Bl.Gym.TrainingApi.Api.Policies;

/// <summary>
/// defines a policy to restore the password.
/// </summary>
internal static class ForgotPasswordPolicy
{
    public const string POLICY_NAME = "ForgetPasswordPolicy";
    public const string Scheme = "ForgetPasswordScheme";
    public static Claim RequireRole = UserClaim.ChangePassword;
}
