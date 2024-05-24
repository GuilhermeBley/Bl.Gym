﻿using Bl.Gym.TrainingApi.Domain.Primitive;

namespace Bl.Gym.TrainingApi.Domain.Exceptions;

/// <summary>
/// Exceptions in Core
/// </summary>
public abstract class CoreException : Exception, ICoreException
{
    /// <summary>
    /// Status code
    /// </summary>
    public abstract CoreExceptionCode StatusCode { get; }

    /// <summary>
    /// Source Core
    /// </summary>
    public override string? Source => "Bl.Gym.Domain";

    protected CoreException()
    {
    }

    protected CoreException(string? message) : base(message)
    {
    }

    protected CoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public override string ToString()
    {
        return $"{StatusCode}|{base.Message}";
    }
}
