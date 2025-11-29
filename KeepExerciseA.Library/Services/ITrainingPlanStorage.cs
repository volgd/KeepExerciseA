using System.Collections.Generic;
using System.Threading.Tasks;
using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.Services
{
    public interface ITrainingPlanStorage
    {
        Task InitializeAsync();
        Task<int> SaveTrainingPlanAsync(TrainingPlan plan);
        Task<int> SaveTrainingPlanActionAsync(TrainingPlanAction action);
        Task<List<TrainingPlan>> GetTrainingPlansAsync();
        Task<TrainingPlan> GetTrainingPlanAsync(int id);
        Task<List<TrainingPlanAction>> GetTrainingPlanActionsAsync(int trainingPlanId);
        Task<int> DeleteTrainingPlanAsync(int id);
        Task<int> DeleteTrainingPlanActionsAsync(int trainingPlanId);
    }
}