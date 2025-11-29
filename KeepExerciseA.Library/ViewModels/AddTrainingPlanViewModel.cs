using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;

namespace KeepExerciseA.Library.ViewModels
{
    public partial class AddTrainingPlanViewModel : ViewModelBase
    {
        private readonly ITrainingPlanStorage _trainingPlanStorage;
        private readonly IExerciseTipsStorage _exerciseTipsStorage;
        private readonly IContentNavigationSecvices _contentNavigationSecvices;

        [ObservableProperty]
        private string _name = string.Empty;

        // 修改为DateTimeOffset?类型，匹配DatePicker的返回类型
        [ObservableProperty]
        private DateTimeOffset? _trainingDate = DateTimeOffset.Now;

        [ObservableProperty]
        private TimeSpan _trainingTimeOfDay = DateTime.Now.TimeOfDay;

        // 计算属性，合并日期和时间
        public DateTime TrainingTime
        {
            get
            {
                try
                {
                    // 如果TrainingDate为null，使用当前日期
                    var date = TrainingDate?.DateTime ?? DateTime.Now.Date;
                    return date.Add(TrainingTimeOfDay);
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set
            {
                try
                {
                    TrainingDate = new DateTimeOffset(value.Date);
                    TrainingTimeOfDay = value.TimeOfDay;
                }
                catch
                {
                    TrainingDate = DateTimeOffset.Now;
                    TrainingTimeOfDay = DateTime.Now.TimeOfDay;
                }
            }
        }

        public ObservableCollection<ExerciseTips> AvailableActions { get; } = new();
        public ObservableCollection<ExerciseTips> SelectedActions { get; } = new();

        private int? _trainingPlanId;
        public object? Parameter { get; set; }

        public AddTrainingPlanViewModel(
            ITrainingPlanStorage trainingPlanStorage,
            IExerciseTipsStorage exerciseTipsStorage,
            IContentNavigationSecvices contentNavigationSecvices)
        {
            _trainingPlanStorage = trainingPlanStorage;
            _exerciseTipsStorage = exerciseTipsStorage;
            _contentNavigationSecvices = contentNavigationSecvices;
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
            LoadAvailableActionsCommand = new AsyncRelayCommand(LoadAvailableActionsAsync);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand LoadAvailableActionsCommand { get; }

        public async Task InitializeAsync(int? trainingPlanId = null)
        {
            _trainingPlanId = trainingPlanId;
            await LoadAvailableActionsAsync();
            
            if (trainingPlanId.HasValue)
            {
                var plan = await _trainingPlanStorage.GetTrainingPlanAsync(trainingPlanId.Value);
                if (plan != null)
                {
                    Name = plan.Name;
                    // 安全地设置日期和时间 - 转换为DateTimeOffset
                    try
                    {
                        TrainingDate = new DateTimeOffset(plan.TrainingTime.Date);
                        TrainingTimeOfDay = plan.TrainingTime.TimeOfDay;
                    }
                    catch
                    {
                        TrainingDate = DateTimeOffset.Now;
                        TrainingTimeOfDay = DateTime.Now.TimeOfDay;
                    }
                    
                    var planActions = await _trainingPlanStorage.GetTrainingPlanActionsAsync(trainingPlanId.Value);
                    foreach (var action in planActions)
                    {
                        var exercise = AvailableActions.FirstOrDefault(x => x.id == action.ActionId);
                        if (exercise != null)
                        {
                            SelectedActions.Add(exercise);
                        }
                    }
                }
            }
        }

        private async Task LoadAvailableActionsAsync()
        {
            try
            {
                AvailableActions.Clear();
                var actions = await _exerciseTipsStorage.GetTipsAsync(p => true, 0, 1000);
                foreach (var action in actions)
                {
                    AvailableActions.Add(action);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadAvailableActionsAsync Error: {ex.Message}");
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(Name))
                {
                    System.Diagnostics.Debug.WriteLine("计划名称不能为空");
                    return;
                }

                TrainingPlan plan;
                if (_trainingPlanId.HasValue)
                {
                    plan = await _trainingPlanStorage.GetTrainingPlanAsync(_trainingPlanId.Value);
                    if (plan == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"无法找到训练计划ID: {_trainingPlanId.Value}");
                        return;
                    }
                    plan.Name = Name;
                    plan.TrainingTime = TrainingTime; // 使用计算属性
                    await _trainingPlanStorage.SaveTrainingPlanAsync(plan);
                    await _trainingPlanStorage.DeleteTrainingPlanActionsAsync(_trainingPlanId.Value);
                }
                else
                {
                    plan = new TrainingPlan
                    {
                        Name = Name,
                        TrainingTime = TrainingTime // 使用计算属性
                    };
                    var planId = await _trainingPlanStorage.SaveTrainingPlanAsync(plan);
                    plan.Id = planId;
                }

                // 确保计划ID有效
                if (plan.Id <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("训练计划保存失败，ID无效");
                    return;
                }

                // 保存动作关联
                foreach (var action in SelectedActions)
                {
                    var planAction = new TrainingPlanAction
                    {
                        TrainingPlanId = plan.Id,
                        ActionId = action.id
                    };
                    await _trainingPlanStorage.SaveTrainingPlanActionAsync(planAction);
                }

                _contentNavigationSecvices.NavigateTo("TrainingPlan");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveAsync Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void Cancel()
        {
            _contentNavigationSecvices.NavigateTo("TrainingPlan");
        }
    }
}
