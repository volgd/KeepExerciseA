using KeepExerciseA.Library.Services;
using KeepExerciseA.UnitTest.Helpers;
using Moq;

namespace KeepExerciseA.UnitTest.Services;

public class ExerciseTipsStorageTest : IDisposable
{
    public ExerciseTipsStorageTest() =>
        ExerciseTipsStorageHelper.RemoveDBFile();
    
    public void Dispose() => ExerciseTipsStorageHelper.RemoveDBFile();
    
    [Fact]
    public async Task TestExerciseTipsStorageTest()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var exerciseTipsStorage = new ExerciseTipsStorage(mockPreferenceStorage);
        Assert.False(File.Exists(ExerciseTipsStorage.ExerciseTipsDBPath));
        await exerciseTipsStorage.InitializeAsync();
        Assert.True(File.Exists(ExerciseTipsStorage.ExerciseTipsDBPath));
    }
    
    [Fact]
    public void TestInitialized()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p => 
                p.Get(ExerciseTipsStorageConstant.VersionKey, 0))
            .Returns(ExerciseTipsStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var exerciseTipsStorage = new ExerciseTipsStorage(mockPreferenceStorage);
        Assert.True(exerciseTipsStorage.IsInitialized);
        
        preferenceStorageMock.Verify(
            p =>p.Get(ExerciseTipsStorageConstant.VersionKey, 0), Times.Once
            );
    }

    [Fact]
    public async Task TestGetExerciseTipsAsync_GivenCorrectId_ShouldReturnExerciseTips()
    {
        var exerciseTipsStorage = await ExerciseTipsStorageHelper.GetInitializedExerciseTipsStorageAsync();
        var exerciseTips = await exerciseTipsStorage.GetTipsAsync(1);
        Assert.Equal("腹部",exerciseTips.aimmuscle);
        await exerciseTipsStorage.CloseAsync();
    }
}