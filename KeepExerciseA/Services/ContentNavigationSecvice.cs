using System;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Services;

public class ContentNavigationSecvice : IContentNavigationSecvices
{
    public void NavigateTo(string view, object? parameter = null)
    {
        ViewModelBase content = view switch
        {
            ContentNavigationConstant.TodayDetail => ServicesLocator.Current.TodayDetailViewModel,
            ContentNavigationConstant.BodyMap     => ServicesLocator.Current.BodyMapViewModel,
            ContentNavigationConstant.TrainingPlan => ServicesLocator.Current.TrainingPlanViewModel,
            ContentNavigationConstant.AddTrainingPlan => ServicesLocator.Current.AddTrainingPlanViewModel,
            _ =>throw new Exception("View not supported")
        };
        if (parameter != null) {
            content.SetParameter(parameter);
        }
        ServicesLocator.Current.MainViewModel.PushContent(content);

        if (view == ContentNavigationConstant.AddTrainingPlan && parameter != null)
        {
            if (ServicesLocator.Current.MainViewModel.Content is AddTrainingPlanViewModel vm)
            {
                // 通过ViewModel的属性传递参数
                vm.Parameter = parameter;
            }
        }
    }
}