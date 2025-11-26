using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Services;

public class MenuNavigationServices : IMenuNavigationServices
{
    public void NavigateTo(string view, object? parameter = null)
    {
        ViewModelBase viewModel = view switch
        {
            MenuNavigationConstant.TodayView => ServicesLocator.Current.TodayViewModel,
            MenuNavigationConstant.QueryView => ServicesLocator.Current.QueryViewModel,
        };
        
        ServicesLocator.Current.MainViewModel.SetMenuAndContent(view,viewModel);
    }
}