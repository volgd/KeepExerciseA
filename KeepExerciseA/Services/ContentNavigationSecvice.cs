using System;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Services;

public class ContentNavigationSecvice : IContentNavigationSecvices
{
    public void NavigateTo(string view, object? parameter = null)
    {
        var content = view switch
        {
            ContentNavigationConstant.TodayDetail => ServicesLocator.Current.TodayDetailViewModel
            ,_ =>throw new Exception("View not supported")
        };
        ServicesLocator.Current.MainViewModel.PushContent(content);

    }
}