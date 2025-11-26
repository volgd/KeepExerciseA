namespace KeepExerciseA.Library.Services;

public interface IContentNavigationSecvices
{
    void NavigateTo(string view,object? parameter=null);
}

public static class ContentNavigationConstant
{
    public const string TodayDetail =  nameof(TodayDetail);
}