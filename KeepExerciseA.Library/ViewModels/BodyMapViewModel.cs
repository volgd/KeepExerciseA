using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels;

public class BodyMapViewModel : ViewModelBase
{
    private readonly IExerciseTipsStorage _exerciseTipsStorage;

    private string _selectedRegionTitle = "请点击人体部位查看锻炼建议";

    public string SelectedRegionTitle
    {
        get => _selectedRegionTitle;
        set => SetProperty(ref _selectedRegionTitle, value);
    }

    public ObservableCollection<ExerciseTips> Tips { get; } = new();

    public IAsyncRelayCommand<string> SelectRegionCommand { get; }

    public BodyMapViewModel(IExerciseTipsStorage exerciseTipsStorage)
    {
        _exerciseTipsStorage = exerciseTipsStorage;
        SelectRegionCommand = new AsyncRelayCommand<string>(LoadRegionAsync);
    }

    private async Task LoadRegionAsync(string regionKey)
    {
        if (string.IsNullOrWhiteSpace(regionKey))
            return;

        if (!_exerciseTipsStorage.IsInitialized)
        {
            await _exerciseTipsStorage.InitializeAsync();
        }

        Tips.Clear();

        Expression<Func<ExerciseTips, bool>> where =
            e => e.aimmuscle.Contains(regionKey);

        var list = await _exerciseTipsStorage.GetTipsAsync(where, 0, 50);

        if (list.Count == 0)
        {
            SelectedRegionTitle = $"暂未找到与“{regionKey}”相关的锻炼建议";
            return;
        }

        SelectedRegionTitle = $"{regionKey}（共 {list.Count} 条锻炼建议）";

        foreach (var item in list)
        {
            Tips.Add(item);
        }
    }
}