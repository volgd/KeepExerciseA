using System.Linq.Expressions;
using AvaloniaInfiniteScrolling;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels;

public class ResultViewModel : ViewModelBase
{
    private Expression<Func<ExerciseTips, bool>> _where =
        Expression.Lambda<Func<ExerciseTips,bool>>(Expression.Constant(true), 
            Expression.Parameter(typeof(ExerciseTips),"p"));

    public const int pagesize = 10;

    private bool _canloadmore = true;
    public ResultViewModel(IExerciseTipsStorage exerciseTipsStorage)
    {
        exerciseTipsStorage.InitializeAsync().ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
        ExerciseTipsCollection = new AvaloniaInfiniteScrollCollection<ExerciseTips>
        {
            OnCanLoadMore =()=> _canloadmore,
            OnLoadMore = async () =>
            {
                Status = Loading;
                var exerciseTips = await exerciseTipsStorage.GetTipsAsync(_where, 
                    ExerciseTipsCollection.Count, pagesize);
                Status = string.Empty;
                if (exerciseTips.Count < pagesize)
                {
                    _canloadmore = false;
                }

                if (ExerciseTipsCollection.Count == 0 && exerciseTips.Count == 0)
                {
                    Status = NoMoreResult;
                }
                return exerciseTips;
            }
        };
    }

    private string _status;

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }
    
    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";

    public AvaloniaInfiniteScrollCollection<ExerciseTips> ExerciseTipsCollection
    {
        get;
    }
}