using System;
using Avalonia;
using Avalonia.Controls;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;
using KeepExerciseA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KeepExerciseA;

public class ServicesLocator
{
    private readonly IServiceProvider _serviceProvider;

    private static ServicesLocator _current;

    public static ServicesLocator Current
    {
        get
        {
            if (_current != null)
            {
                return _current;
            }

            if (Application.Current!.TryGetResource(nameof(ServicesLocator), out var resource)
                &&resource is ServicesLocator servicesLocator)
            {
                _current = servicesLocator;
            }
            return _current;
        }
    }
    
    
    public MainWindowViewModel MainWindowViewModel => _serviceProvider.GetService<MainWindowViewModel>();
    public ResultViewModel ResultViewModel => _serviceProvider.GetRequiredService<ResultViewModel>();
    public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
    public InitializationViewModel InitializationViewModel => _serviceProvider.GetRequiredService<InitializationViewModel>();
    public TodayViewModel TodayViewModel => _serviceProvider.GetRequiredService<TodayViewModel>();
    public QueryViewModel QueryViewModel => _serviceProvider.GetRequiredService<QueryViewModel>();
    public TodayDetailViewModel TodayDetailViewModel => _serviceProvider.GetRequiredService<TodayDetailViewModel>();
    public IMenuNavigationServices MenuNavigationServices => _serviceProvider.GetRequiredService<IMenuNavigationServices>();
    //TO
    public IRootNavigationServices RootNavigationServices => _serviceProvider.GetRequiredService<IRootNavigationServices>();
    
    public ServicesLocator()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection.AddSingleton<IExerciseTipsStorage, ExerciseTipsStorage>();
        serviceCollection.AddSingleton<ITodayExercisesTipServices, JinriTipServices>();
        serviceCollection.AddSingleton<IAlertServices, AlertServicess>();

        serviceCollection.AddSingleton<IRootNavigationServices, RootNavigationService>();
        serviceCollection.AddSingleton<IContentNavigationSecvices, ContentNavigationSecvice>();
        serviceCollection.AddSingleton<IMenuNavigationServices, MenuNavigationServices>();
        
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<InitializationViewModel>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<TodayViewModel>();
        serviceCollection.AddSingleton<TodayDetailViewModel>();
        serviceCollection.AddSingleton<QueryViewModel>();
        
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}