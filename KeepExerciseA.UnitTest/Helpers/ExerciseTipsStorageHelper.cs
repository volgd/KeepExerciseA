using KeepExerciseA.Library.Services;
using Moq;

namespace KeepExerciseA.UnitTest.Helpers;

public class ExerciseTipsStorageHelper
{
    public static void RemoveDBFile() =>  //静态方法，无需实例
        File.Delete(ExerciseTipsStorage.ExerciseTipsDBPath);

    public static async Task<ExerciseTipsStorage> GetInitializedExerciseTipsStorageAsync()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var exerciseTipsStorage = new ExerciseTipsStorage(mockPreferenceStorage);
        await exerciseTipsStorage.InitializeAsync();
        return exerciseTipsStorage;
    }
}