using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels;

public class TodayViewModel : ViewModelBase
{
    private IContentNavigationSecvices _contentNavigationSecvices;

    public TodayViewModel(IContentNavigationSecvices contentNavigationSecvices)
    {
        _contentNavigationSecvices = contentNavigationSecvices;
        
        ShowDetailCommand = new RelayCommand(ShowDetail);
    }
    public RelayCommand ShowDetailCommand { get; }

    public void ShowDetail()
    {
        _contentNavigationSecvices.NavigateTo(ContentNavigationConstant.TodayDetail);
    }
    
}