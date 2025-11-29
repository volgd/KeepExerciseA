using System;
using System.Threading.Tasks;
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
    public FitnessAssessmentViewModel FitnessAssessmentViewModel =>
        _serviceProvider.GetRequiredService<FitnessAssessmentViewModel>();
    public BodyMapViewModel BodyMapViewModel => _serviceProvider.GetRequiredService<BodyMapViewModel>();
    public AddTrainingPlanViewModel AddTrainingPlanViewModel => 
        _serviceProvider.GetRequiredService<AddTrainingPlanViewModel>();
    public TrainingPlanViewModel TrainingPlanViewModel => _serviceProvider.GetRequiredService<TrainingPlanViewModel>();
    public TodayDetailViewModel TodayDetailViewModel => _serviceProvider.GetRequiredService<TodayDetailViewModel>();
    public IMenuNavigationServices MenuNavigationServices => _serviceProvider.GetRequiredService<IMenuNavigationServices>();
    //TO
    public IRootNavigationServices RootNavigationServices => _serviceProvider.GetRequiredService<IRootNavigationServices>();
    
    public ServicesLocator()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection.AddSingleton<IExerciseTipsStorage, ExerciseTipsStorage>();
        serviceCollection.AddSingleton<ITrainingPlanStorage, TrainingPlanStorage>();
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
        serviceCollection.AddSingleton<TrainingPlanViewModel>();
        serviceCollection.AddSingleton<AddTrainingPlanViewModel>();
        serviceCollection.AddSingleton<FitnessAssessmentViewModel>();
        serviceCollection.AddSingleton<BodyMapViewModel>(); 
        
        _serviceProvider = serviceCollection.BuildServiceProvider();
        
        // 异步初始化数据库
        Task.Run(async () =>
        {
            try
            {
                var exerciseTipsStorage = _serviceProvider.GetRequiredService<IExerciseTipsStorage>();
                await exerciseTipsStorage.InitializeAsync();
                // 初始化新的训练计划数据库
                var trainingPlanStorage = _serviceProvider.GetRequiredService<ITrainingPlanStorage>();
                await trainingPlanStorage.InitializeAsync();
            }
            catch (Exception ex)
            {
                // 记录错误但不阻止应用启动
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");
            }
        });
    }
}