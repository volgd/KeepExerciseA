using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.Services;

public interface ITodayExercisesTipServices
{
    Task<TodayExerciseTips> GetTodayExerciseTipsAsync();
}

public static class TodayExercisesTipsSource
{
    public const string Jinritip = nameof(Jinritip);
    
    public const string Local =  nameof(Local);
}