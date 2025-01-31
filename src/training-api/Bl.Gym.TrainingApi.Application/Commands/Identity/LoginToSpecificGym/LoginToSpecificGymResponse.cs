﻿using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.LoginToSpecificGym;

public record LoginToSpecificGymResponse(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string RefreshToken,
    Guid GymId,
    IReadOnlyList<Claim> Claims);
