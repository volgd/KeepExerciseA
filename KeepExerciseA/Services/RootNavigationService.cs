using System;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Services;

public class RootNavigationService : IRootNavigationServices
{
    public void NavigateTo(string view)
    {
        ServicesLocator.Current.MainWindowViewModel.Content = view switch
        {
            RootNavigationConstant.InitializationView => ServicesLocator.Current.InitializationViewModel,
            RootNavigationConstant.MainView => ServicesLocator.Current.MainViewModel,
            
            _ => throw new Exception("UNKOWN VIEW")
        };
    }
}