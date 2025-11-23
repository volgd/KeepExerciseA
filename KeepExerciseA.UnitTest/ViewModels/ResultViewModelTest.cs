using KeepExerciseA.Library.ViewModels;
using KeepExerciseA.UnitTest.Helpers;

namespace KeepExerciseA.UnitTest.ViewModels;

public class ResultViewModelTest : IDisposable
{
    public ResultViewModelTest() => ExerciseTipsStorageHelper.RemoveDBFile();
    
    public void Dispose() => ExerciseTipsStorageHelper.RemoveDBFile();
    
    [Fact]
    public async Task TestExerciseTipsCollection()
    {
        var exerciseTipsStorage = await ExerciseTipsStorageHelper.GetInitializedExerciseTipsStorageAsync();
        var resultViewModel = new ResultViewModel(exerciseTipsStorage);

        await resultViewModel.ExerciseTipsCollection.LoadMoreAsync();
        Assert.Equal(2, resultViewModel.ExerciseTipsCollection.Count);
        await exerciseTipsStorage.CloseAsync();
    }
    
}