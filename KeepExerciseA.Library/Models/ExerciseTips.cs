namespace KeepExerciseA.Library.Models;

[SQLite.Table("KeepExerciseADBTable")]
public class ExerciseTips
{
    [SQLite.Column("id")]
    public int id { get; set; } = 0;
    
    [SQLite.Column("aim_muscle")]
    public string aimmuscle { get; set; } = string.Empty;
    
    [SQLite.Column("content")]
    public string content { get; set; } =  string.Empty;
    
    [SQLite.Column("precaution")]
    public string precautions { get; set; } =  string.Empty;

    private string? part;
    [SQLite.Ignore]
    public string Part => part ?? content.Split("。")[0].Replace("\r\n", " ");
    
}