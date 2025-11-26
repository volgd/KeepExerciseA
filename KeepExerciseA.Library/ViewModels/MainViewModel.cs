using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels;

public class MainViewModel :ViewModelBase
{
    private readonly IMenuNavigationServices _menuNavigationServices;
    public MainViewModel(IMenuNavigationServices menuNavigationServices)
    {
        _menuNavigationServices = menuNavigationServices;
        OpenPaneCommand =new RelayCommand(OpenPane);
        ClosePaneCommand = new RelayCommand(ClosePane);
        GoBackCommand = new RelayCommand(GoBack);
        OnMenuItemCommand = new RelayCommand(OnMenuTapped);
    }
    
    public ICommand OpenPaneCommand { get; }

    public void OpenPane() => IsPaneOpen = true;

    public ICommand ClosePaneCommand { get; }

    public void ClosePane() => IsPaneOpen = false;
    
    private bool _isPaneOpen;
    
    public bool IsPaneOpen
    {
        get => _isPaneOpen;
        private set => SetProperty(ref _isPaneOpen, value);
    }

    private ViewModelBase _content;

    public ViewModelBase Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public void PushContent(ViewModelBase content) => ContentStack.Add(Content = content); //利用赋值语句会返回值

    public void SetMenuAndContent(string view, ViewModelBase content)
    {
        ContentStack.Clear();
        PushContent(content);
        SelectedMenuItem =MenuItem.MenuItems.First(p => p.View == view);
        Title = SelectedMenuItem.Name;
        IsPaneOpen = false;
    }

    private MenuItem _selectedMenuItem;

    public MenuItem SelectedMenuItem
    {
        get => _selectedMenuItem;
        set => SetProperty(ref _selectedMenuItem, value);
    }

    public ICommand OnMenuItemCommand { get; }

    public void OnMenuTapped()
    {
        _menuNavigationServices.NavigateTo(SelectedMenuItem.View);
    }
    
    public ObservableCollection<ViewModelBase> ContentStack { get; } = [];
    
    public ICommand GoBackCommand { get; }
    
    public void GoBack()
    {
        if (ContentStack.Count <= 1)
        {
            return;
        }
        ContentStack.RemoveAt(ContentStack.Count - 1);
        Content = ContentStack[^1];
    }
    private string _title = "Keep Exercise A";

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}

public class MenuItem
{
    public string View { get; private init; }
    public string Name { get; private init; }

    private MenuItem()
    {
        
    }

    public static MenuItem TodayView => new() { Name = "锻炼推荐", View = MenuNavigationConstant.TodayView };

    public static MenuItem QueryView => new() { Name = "建议搜索", View = MenuNavigationConstant.QueryView };
    public static IEnumerable<MenuItem> MenuItems { get; } = [
        TodayView,
        QueryView
    ];
}