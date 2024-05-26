namespace Bl.Gym.TrainingApi.Domain.Primitive;

public enum CoreExceptionCode
{
    Unauthorized = 401,
    Forbbiden = 403,
    BadRequest = 400,
    Conflict = 409,

    InvalidEmail = 400_1,
    InvalidStringLength = 400_2,
    InvalidPassword = 400_3,
}
