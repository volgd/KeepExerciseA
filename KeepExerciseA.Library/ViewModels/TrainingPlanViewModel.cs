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
    public partial class TrainingPlanViewModel : ViewModelBase
    {
        private readonly ITrainingPlanStorage _trainingPlanStorage;
        private readonly IContentNavigationSecvices _contentNavigationSecvices;
        private readonly IExerciseTipsStorage _exerciseTipsStorage;

        public ObservableCollection<TrainingPlanItem> TrainingPlans { get; } = new();
        
        // 添加调试属性
        [ObservableProperty]
        private string _debugMessage = string.Empty;

        public TrainingPlanViewModel(
            ITrainingPlanStorage trainingPlanStorage,
            IContentNavigationSecvices contentNavigationSecvices,
            IExerciseTipsStorage exerciseTipsStorage)
        {
            _trainingPlanStorage = trainingPlanStorage;
            _contentNavigationSecvices = contentNavigationSecvices;
            _exerciseTipsStorage = exerciseTipsStorage;
            
            LoadTrainingPlansCommand = new AsyncRelayCommand(LoadTrainingPlansAsync);
            AddTrainingPlanCommand = new RelayCommand(AddTrainingPlan);
            EditTrainingPlanCommand = new RelayCommand<TrainingPlanItem>(EditTrainingPlan);
            DeleteTrainingPlanCommand = new AsyncRelayCommand<TrainingPlanItem>(DeleteTrainingPlanAsync);
        }

        public ICommand LoadTrainingPlansCommand { get; }
        public ICommand AddTrainingPlanCommand { get; }
        public ICommand EditTrainingPlanCommand { get; }
        public ICommand DeleteTrainingPlanCommand { get; }

        public async Task LoadTrainingPlansAsync()
        {
            try
            {
                DebugMessage = "开始加载训练计划...";
                TrainingPlans.Clear();
                
                var plans = await _trainingPlanStorage.GetTrainingPlansAsync();
                DebugMessage = $"找到 {plans.Count} 个训练计划";
                
                foreach (var plan in plans)
                {
                    DebugMessage = $"处理计划: {plan.Name} (ID: {plan.Id})";
                    
                    var actions = await _trainingPlanStorage.GetTrainingPlanActionsAsync(plan.Id);
                    var exerciseTips = new List<ExerciseTips>();
                    
                    DebugMessage = $"计划 {plan.Name} 有 {actions.Count} 个动作";
                    
                    foreach (var action in actions)
                    {
                        DebugMessage = $"获取动作 ID: {action.ActionId}";
                        var tip = await _exerciseTipsStorage.GetTipsAsync(action.ActionId);
                        if (tip != null) 
                        {
                            exerciseTips.Add(tip);
                            DebugMessage = $"成功添加动作: {tip.content}";
                        }
                        else
                        {
                            DebugMessage = $"警告: 动作 ID {action.ActionId} 不存在";
                        }
                    }
                    
                    TrainingPlans.Add(new TrainingPlanItem
                    {
                        Plan = plan,
                        ExerciseTips = exerciseTips
                    });
                    
                    DebugMessage = $"成功添加训练计划: {plan.Name}";
                }
                
                DebugMessage = $"加载完成，共 {TrainingPlans.Count} 个训练计划";
            }
            catch (Exception ex)
            {
                DebugMessage = $"加载失败: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"TrainingPlanViewModel Error: {ex}");
            }
        }

        private void AddTrainingPlan()
        {
            _contentNavigationSecvices.NavigateTo("AddTrainingPlan");
        }

        private void EditTrainingPlan(TrainingPlanItem item)
        {
            _contentNavigationSecvices.NavigateTo("AddTrainingPlan", item.Plan.Id);
        }

        private async Task DeleteTrainingPlanAsync(TrainingPlanItem item)
        {
            await _trainingPlanStorage.DeleteTrainingPlanAsync(item.Plan.Id);
            await LoadTrainingPlansAsync();
        }

        public async Task InitializeAsync()
        {
            await LoadTrainingPlansAsync();
        }
    }

    public class TrainingPlanItem
    {
        public TrainingPlan Plan { get; set; }
        public List<ExerciseTips> ExerciseTips { get; set; }
    }
}