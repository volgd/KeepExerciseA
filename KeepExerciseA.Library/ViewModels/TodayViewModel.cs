using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.ViewModels;

public class TodayViewModel : ViewModelBase
{
    private readonly IContentNavigationSecvices _contentNavigationSecvices;
    private readonly ITodayExercisesTipServices _todayExercisesTipServices;
    

    public TodayViewModel(IContentNavigationSecvices contentNavigationSecvices,
        ITodayExercisesTipServices todayExercisesTipServices)
    {
        _todayExercisesTipServices =  todayExercisesTipServices;
        _contentNavigationSecvices = contentNavigationSecvices;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
        ShowDetailCommand = new RelayCommand(ShowDetail);
    }
    public RelayCommand ShowDetailCommand { get; }
    
    private TodayExerciseTips _todayExerciseTips;

    public TodayExerciseTips TodayExerciseTips
    {
        get => _todayExerciseTips;
        private set => SetProperty(ref _todayExerciseTips, value);
    }
    
    public ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync()
    {
        Task.Run(async () => {
            
            await Task.Delay(1000);
            TodayExerciseTips = await _todayExercisesTipServices.GetTodayExerciseTipsAsync();

        });
    }

    public void ShowDetail()
    {
        _contentNavigationSecvices.NavigateTo(ContentNavigationConstant.TodayDetail,TodayExerciseTips);
    }
    
}