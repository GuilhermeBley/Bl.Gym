namespace Bl.Gym.TrainingApi.Application.Commands.Gym.CreateGymGroupByAdmin;

/// <summary>
/// Creates a new gym as user ADMIN
/// </summary>
/// <param name="Name">Gym name to create</param>
/// <param name="Description">Gym description</param>
public record CreateGymGroupByAdminRequest(
    string Name,
    string Description)
    : IRequest<CreateGymGroupByAdminResponse>;
