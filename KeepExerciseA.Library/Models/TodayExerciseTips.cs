namespace KeepExerciseA.Library.Models;

public class TodayExerciseTips
{
    public string Aimmuscle {get; set;} =string.Empty;
    
    public string Content {get; set;} =string.Empty;
    
    public string Warning {get; set;} =string.Empty;

    public string? snippet;
    public string Snippet => snippet ?? snippet.Split("\n")[0].Replace("*#\n", " ");
}