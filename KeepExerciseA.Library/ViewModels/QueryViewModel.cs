using System.Collections.ObjectModel;
using System.Linq.Expressions;
using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels;

public class QueryViewModel : ViewModelBase
{
    private readonly IExerciseTipsStorage _exerciseTipsStorage;
    private readonly IContentNavigationSecvices _contentNavigation;

    private string _searchText = string.Empty;
    private string _status = "请输入想锻炼的部位，例如“肱二头肌”。";

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public ObservableCollection<ExerciseTips> Results { get; } = new();

    // 搜索命令（异步）
    public IAsyncRelayCommand SearchCommand { get; }

    // 跳转到人体图的命令
    public IRelayCommand BodyMapCommand { get; }

    public QueryViewModel(IExerciseTipsStorage exerciseTipsStorage,
                          IContentNavigationSecvices contentNavigation)
    {
        _exerciseTipsStorage = exerciseTipsStorage;
        _contentNavigation = contentNavigation;

        SearchCommand = new AsyncRelayCommand(SearchAsync);
        BodyMapCommand = new RelayCommand(GoBodyMap);
    }

    private async Task SearchAsync()
    {
        var keyword = SearchText?.Trim();
        Results.Clear();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            Status = "请输入想锻炼的部位，例如“肱二头肌”。";
            return;
        }

        try
        {
            if (!_exerciseTipsStorage.IsInitialized)
            {
                await _exerciseTipsStorage.InitializeAsync();
            }

            Expression<Func<ExerciseTips, bool>> where =
                e => e.aimmuscle.Contains(keyword);

            var list = await _exerciseTipsStorage.GetTipsAsync(where, 0, 50);

            if (list.Count == 0)
            {
                Status = "没有找到相关的锻炼建议，请尝试更换关键词。";
                return;
            }

            foreach (var item in list)
            {
                Results.Add(item);
            }

            Status = $"共找到 {list.Count} 条锻炼建议。";
        }
        catch (Exception ex)
        {
            Status = $"查询过程中发生错误：{ex.Message}";
        }
    }

    // 真正负责“跳转到人体图”的方法
    private void GoBodyMap()
    {
        _contentNavigation.NavigateTo(
            KeepExerciseA.Library.Helpers.ContentNavigationConstant.BodyMap);
    }

}