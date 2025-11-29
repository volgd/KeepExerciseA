using KeepExerciseA.Library.Services;
using Moq;

namespace KeepExerciseA.UnitTest.Services;

public class JinriTipServicesTest
{
    [Fact]
    public async Task TestGetTodayExercisesTipAsync()
    {
        var alertServiceMock = new Mock<IAlertServices>();
        var mockAlertService = alertServiceMock.Object;
        
        var exerciseTipsStorageMock = new Mock<IExerciseTipsStorage>();
        var mockExerciseTipsStorage = exerciseTipsStorageMock.Object;
        
        var jinriExerciseTipsServices = new JinriTipServices(mockAlertService,mockExerciseTipsStorage);
        var todayExerciseTips = await jinriExerciseTipsServices.GetTodayExerciseTipsAsync();
        
        Assert.Equal(TodayExercisesTipsSource.Jinritip,todayExerciseTips.Source);
        Assert.False(string.IsNullOrEmpty(todayExerciseTips.Snippet));
        
        
    }
}