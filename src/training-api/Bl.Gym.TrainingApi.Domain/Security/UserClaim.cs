using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Domain.Security;

public static class UserClaim
{
    public const string DEFAULT_ROLE = "role";
    public const string DEFAULT_USER_NAME = "name";
    public const string DEFAULT_USER_ID = "nameidentifier";
    public const string DEFAULT_USER_EMAIL = "emailaddress";
    public static Claim SeeTraining => new(DEFAULT_ROLE, "SeeTraining");
    public static Claim ManageTraining => new(DEFAULT_ROLE, "ManageTraining");
    public static Claim ManageGymGroup => new(DEFAULT_ROLE, "ManageGymGroup");

    public static Claim CreateUserNameClaim(string userName)
        => new(DEFAULT_USER_NAME, userName);
    public static Claim CreateUserIdClaim(string userId)
        => new(DEFAULT_USER_ID, userId);
    public static Claim CreateUserEmailClaim(string email)
        => new(DEFAULT_USER_EMAIL, email);
}
