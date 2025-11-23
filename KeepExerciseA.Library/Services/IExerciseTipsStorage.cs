using System.Linq.Expressions;
using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.Services;

public interface IExerciseTipsStorage
{
    bool IsInitialized { get; }
    
    Task InitializeAsync();
    
    Task<ExerciseTips> GetTipsAsync(int id);
    
    Task<IList<ExerciseTips>> GetTipsAsync(
        Expression<Func<ExerciseTips, bool>> where,int skip,int take
        );
}