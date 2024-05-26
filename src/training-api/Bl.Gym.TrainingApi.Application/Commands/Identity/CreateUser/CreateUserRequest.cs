﻿using MediatR;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.CreateUser;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    bool EmailConfirmed,
    string Password,
    string? PhoneNumber)
    : IRequest<CreateUserResponse>;
