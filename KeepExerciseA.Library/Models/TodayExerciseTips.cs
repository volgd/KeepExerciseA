namespace KeepExerciseA.Library.Models;

public class TodayExerciseTips
{
    
    public string Content {get; set;} =string.Empty;
    
    public string Warning {get; set;} =string.Empty;
    
    public string? snippet = "正在加载，请稍等片刻......";
    
    public string Source { get; set; } = string.Empty;

    public string Snippet =>
                snippet?.Substring(0, Math.Min(snippet.Length, 200)) + "...";
       
    
}