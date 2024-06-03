namespace Bl.Gym.TrainingApi.Domain.Primitive;

public enum CoreExceptionCode
{
    Unauthorized = 401,

    Forbbiden = 403,
    UserIsntMemberOfThisGym = 403_1,

    NotFound = 404,

    Conflict = 409,
    UserAlreadyHasATrainingInProgressInThisGym = 409_1,

    BadRequest = 400,
    InvalidEmail = 400_1,
    InvalidStringLength = 400_2,
    InvalidPassword = 400_3,
    InvalidFirstName = 400_4,
    InvalidLastName = 400_5,
    InvalidPhoneNumber = 400_6,
    InvalidTrainingSectionName = 400_7,
    InvalidSetOfTrainingSections = 400_8,
    InvalidSetValue = 400_9,
    ItsRequiredAtLeastOneExerciseForSection = 400_10,
    InvalidDaysCount = 400_11,
}
