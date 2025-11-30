using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;
using Moq;

namespace KeepExerciseA.UnitTest.ViewModels;

public class TodetailViewModelTest
{
    [Fact]
    public async Task ShowDetailCommandFunction_Default() {
        var contentNavigationServiceMock = new Mock<IContentNavigationSecvices>();
        var mockContentNavigationService = contentNavigationServiceMock.Object;

        var todayViewModel = new TodayViewModel(
            mockContentNavigationService,null);
        todayViewModel.ShowDetail();
        contentNavigationServiceMock.Verify(
            p => p.NavigateTo(ContentNavigationConstant.TodayDetail, null),
            Times.Once);
    }
}