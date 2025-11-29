using System.Linq.Expressions;
using KeepExerciseA.Library.Helpers;
using KeepExerciseA.Library.Models;
using SQLite;

namespace KeepExerciseA.Library.Services;

public class ExerciseTipsStorage : IExerciseTipsStorage
{
    private IPreferenceStorage _preferenceStorage;
    
    public const string DBName = "KeepExerciseA.sqlite3";
    
    public static readonly string ExerciseTipsDBPath =
        PathHelper.GetLocalFilePath(DBName);
    
    private SQLiteAsyncConnection _connection;
    
    private SQLiteAsyncConnection connection => 
        _connection ??= new SQLiteAsyncConnection(ExerciseTipsDBPath);

    public ExerciseTipsStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }
    
    public bool IsInitialized => _preferenceStorage.
        Get(ExerciseTipsStorageConstant.VersionKey,0) == ExerciseTipsStorageConstant.Version;
    
    public async Task InitializeAsync() //在本地文件系统中写入嵌入式资源
    {
        await connection.CreateTableAsync<ExerciseTips>();
        await connection.CreateTableAsync<TrainingPlan>();
        await connection.CreateTableAsync<TrainingPlanAction>();
        
        
        await using var dbFileStream = new FileStream(ExerciseTipsDBPath, FileMode.OpenOrCreate);
        await using var dbAssertStream = typeof(ExerciseTips).Assembly.
            GetManifestResourceStream(DBName); //从程序集中读取嵌入式资源（数据库）
        await dbAssertStream.CopyToAsync(dbFileStream);
        _preferenceStorage.Set(ExerciseTipsStorageConstant.VersionKey,
            ExerciseTipsStorageConstant.Version);
        
        await connection.CreateTableAsync<ExerciseTips>();
        await connection.CreateTableAsync<TrainingPlan>();
        await connection.CreateTableAsync<TrainingPlanAction>();
    }

    public async Task<ExerciseTips> GetTipsAsync(int id) =>
        await connection.Table<ExerciseTips>()
            .FirstOrDefaultAsync(p => p.id == id
        );

    public async Task<IList<ExerciseTips>> GetTipsAsync
        (Expression<Func<ExerciseTips, bool>> where, int skip, int take) => //语言集成查询 LINQ
        await connection.Table<ExerciseTips>()
            .Where(where).Skip(skip).Take(take).ToListAsync();
    
    public async Task CloseAsync() =>await  connection.CloseAsync();
}
public static class ExerciseTipsStorageConstant {
    public const string VersionKey =
        nameof(ExerciseTipsStorageConstant) + "." + nameof(Version);

    public const int Version = 1;
}