﻿using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public record LoginResponse(
    string Username,
    string Email,
    string RefreshToken,
    IReadOnlyList<Claim> Claims);
