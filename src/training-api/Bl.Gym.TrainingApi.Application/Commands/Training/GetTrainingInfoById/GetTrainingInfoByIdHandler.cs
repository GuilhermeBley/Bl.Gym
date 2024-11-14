using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;
using System.Collections.Generic;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetTrainingInfoById;

/// <summary>
/// Get all the data about the training section by ID.
/// </summary>
public class GetTrainingInfoByIdHandler
    : IRequestHandler<GetTrainingInfoByIdRequest, GetTrainingInfoByIdResponse>
{
    private readonly TrainingContext _context;
    private readonly IIdentityProvider _identityProvider;
    private readonly ILogger<GetTrainingInfoByIdHandler> _logger;
    private readonly GymRoleCheckerService _roleCheckerService;

    public GetTrainingInfoByIdHandler(
        TrainingContext context, 
        IIdentityProvider identityProvider, 
        ILogger<GetTrainingInfoByIdHandler> logger, 
        GymRoleCheckerService roleCheckerService)
    {
        _context = context;
        _identityProvider = identityProvider;
        _logger = logger;
        _roleCheckerService = roleCheckerService;
    }

    public async Task<GetTrainingInfoByIdResponse> Handle(
        GetTrainingInfoByIdRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Requesting for sheet {0}.", request.TrainingSheetId);

        var user = await _identityProvider.GetCurrentAsync(cancellationToken);
        var userId = user.RequiredUserId();

        var trainingGym = await _context.UserTrainingSheets
            .Where(e => e.StudentId == userId)
            .Select(e => new{ e.GymId, e.Status, e.CreatedAt })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CoreException.CreateByCode(CoreExceptionCode.NotFound);

        await _roleCheckerService.ThrowIfUserIsntInTheGymAsync(userId, trainingGym.GymId, cancellationToken);

        var sectionById = await
            (from section in _context.TrainingSections.AsNoTracking()
             where section.UserTrainingSheetId == request.TrainingSheetId
             select new GetTrainingInfoByIdResponseSection(
                 /*Id: */section.Id,
                 /*MuscularGroup: */section.MuscularGroup,
                 /*TargetDaysCount: */section.TargetDaysCount,
                 /*CurrentDaysCount: */section.CurrentDaysCount,
                 /*ConcurrencyStamp: */section.ConcurrencyStamp,
                 /*Sets: */Enumerable.Empty<GetTrainingInfoByIdResponseExerciseSet>(),
                 /*CreatedAt: */section.CreatedAt))
            .SingleAsync(cancellationToken);

        var setsBySection = await
            (from sets in _context.ExerciseSets.AsNoTracking()
             join exercises in _context.Exercises.AsNoTracking()
                on sets.ExerciseId equals exercises.Id
             where sets.TrainingSectionId == sectionById.SectionId
             select new GetTrainingInfoByIdResponseExerciseSet(
                /*Set: */sets.Set,
                /*Title: */exercises.Title,
                /*Description: */exercises.Description,
                /*CreatedAt: */exercises.CreatedAt))
            .ToListAsync(cancellationToken);

        var periodsTrained = await
            _context
            .TrainingsPeriod
            .AsNoTracking()
            .Where(e => e.SectionId == sectionById.SectionId)
            .Select(e => new GetTrainingInfoByIdResponsePeriod(
                /*Id*/ e.Id,
                /*StartedAt*/ e.StartedAt,
                /*EndedAt*/ e.EndedAt,
                /*Obs*/ e.Observation,
                /*Completed*/ e.EndedAt != null))
            .ToListAsync(cancellationToken);

        sectionById = sectionById with
        {
            //
            // Updating the empty sets
            //
            Sets = setsBySection,
        };

        return new GetTrainingInfoByIdResponse(
            Section: sectionById,
            Status: trainingGym.Status.ToString(),
            Periods: periodsTrained,
            CreatedAt: trainingGym.CreatedAt
        );
    }
}