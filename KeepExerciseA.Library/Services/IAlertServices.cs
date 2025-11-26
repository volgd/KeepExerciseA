namespace KeepExerciseA.Library.Services;

public interface IAlertServices
{
    Task AlertAsync(string title, string message);
}