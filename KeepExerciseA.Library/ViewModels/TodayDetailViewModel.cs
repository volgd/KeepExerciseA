using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.ViewModels;

public class TodayDetailViewModel : ViewModelBase
{
    private TodayExerciseTips _todayExerciseTips;

    public TodayExerciseTips TodayExerciseTips
    {
        get => _todayExerciseTips;
        private set => SetProperty(ref _todayExerciseTips, value);
    }
    
    public override void SetParameter(object parameter) {
        TodayExerciseTips = parameter as TodayExerciseTips;
    }
   
}