namespace KeepExerciseA.Library.Services;

public interface IMenuNavigationServices
{
    void NavigateTo(string view,object? parameter=null); //-null表示是可选参数，？是编译器要求
}

public static class MenuNavigationConstant
{
    public const string TodayView = nameof(TodayView);
    
    public const string QueryView = nameof(QueryView);
    
    public const string TrainingPlanView = nameof(TrainingPlanView);
    
    public const string FitnessAssessmentView = nameof(FitnessAssessmentView);
}