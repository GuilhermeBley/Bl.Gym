﻿using Bl.Gym.TrainingApi.Domain.Enum;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetTrainingInfoById;

/// <summary>
/// This represents the sheet of training and contains the training for an entire week.
/// </summary>
/// <param name="Sections"></param>
public record GetTrainingInfoByIdResponse(
    GetTrainingInfoByIdResponseSection Section,
    IEnumerable<GetTrainingInfoByIdResponsePeriod> Periods,
    string Status,
    DateTimeOffset CreatedAt);

/// <summary>
/// Specific from an single training of day.
/// </summary>
public record GetTrainingInfoByIdResponseSection(
    Guid SectionId,
    string MuscularGroup,
    int TargetDaysCount,
    int CurrentDaysCount,
    Guid ConcurrencyStamp,
    IEnumerable<GetTrainingInfoByIdResponseExerciseSet> Sets,
    DateTimeOffset CreatedAt)
    ;

public record GetTrainingInfoByIdResponsePeriod(
    Guid Id,
    DateTime? StartedAt,
    DateTime? EndedAt,
    string? Obs,
    bool Completed)
    ;

public record GetTrainingInfoByIdResponseExerciseSet(
    string Set,
    string Title,
    string Description,
    DateTimeOffset CreatedAt);
