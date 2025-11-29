using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepExerciseA.UnitTest.Helpers;

public class TrainingPlanStorageHelper
{
    public static void RemoveDBFile()
    {
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "KeepExerciseA.sqlite3");
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }
    }

    public static async Task<TrainingPlanStorage> GetInitializedTrainingPlanStorageAsync()
    {
        var trainingPlanStorage = new TrainingPlanStorage();
        await trainingPlanStorage.InitializeAsync();
        return trainingPlanStorage;
    }

    public static async Task<TrainingPlan> CreateTestTrainingPlanAsync(TrainingPlanStorage storage, string name = "测试训练计划")
    {
        var plan = new TrainingPlan
        {
            Name = name,
            TrainingTime = DateTime.Now
        };
        await storage.SaveTrainingPlanAsync(plan);
        return plan;
    }

    public static async Task<TrainingPlanAction> CreateTestTrainingPlanActionAsync(
        TrainingPlanStorage storage, int trainingPlanId, int actionId = 1)
    {
        var action = new TrainingPlanAction
        {
            TrainingPlanId = trainingPlanId,
            ActionId = actionId
        };
        await storage.SaveTrainingPlanActionAsync(action);
        return action;
    }
}