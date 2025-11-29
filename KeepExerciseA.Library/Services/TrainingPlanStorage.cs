using System.Collections.Generic;
using System.Threading.Tasks;
using KeepExerciseA.Library.Helpers;
using KeepExerciseA.Library.Models;
using SQLite;

namespace KeepExerciseA.Library.Services
{
    public class TrainingPlanStorage : ITrainingPlanStorage
    {
        private readonly SQLiteAsyncConnection _connection;

        // 默认构造函数，用于生产环境
        public TrainingPlanStorage()
        {
            var dbPath = PathHelper.GetLocalFilePath("KeepExerciseA.sqlite3");
            _connection = new SQLiteAsyncConnection(dbPath);
        }

        // 测试构造函数，允许注入数据库路径
        public TrainingPlanStorage(string databasePath)
        {
            _connection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task InitializeAsync()
        {
            await _connection.CreateTableAsync<TrainingPlan>();
            await _connection.CreateTableAsync<TrainingPlanAction>();
        }

        public async Task<int> SaveTrainingPlanAsync(TrainingPlan plan)
        {
            try
            {
                if (plan.Id == 0)
                {
                    var id = await _connection.InsertAsync(plan);
                    plan.Id = id;
                    return id;
                }
                else
                {
                    return await _connection.UpdateAsync(plan);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveTrainingPlanAsync Error: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> SaveTrainingPlanActionAsync(TrainingPlanAction action)
        {
            try
            {
                if (action.Id == 0)
                {
                    var id = await _connection.InsertAsync(action);
                    action.Id = id;
                    return id;
                }
                else
                {
                    return await _connection.UpdateAsync(action);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveTrainingPlanActionAsync Error: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<TrainingPlan>> GetTrainingPlansAsync()
        {
            try
            {
                return await _connection.Table<TrainingPlan>().ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTrainingPlansAsync Error: {ex.Message}");
                return new List<TrainingPlan>();
            }
        }

        public async Task<TrainingPlan> GetTrainingPlanAsync(int id)
        {
            try
            {
                return await _connection.Table<TrainingPlan>()
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTrainingPlanAsync Error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<TrainingPlanAction>> GetTrainingPlanActionsAsync(int trainingPlanId)
        {
            try
            {
                return await _connection.Table<TrainingPlanAction>()
                    .Where(x => x.TrainingPlanId == trainingPlanId).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTrainingPlanActionsAsync Error: {ex.Message}");
                return new List<TrainingPlanAction>();
            }
        }

        public async Task<int> DeleteTrainingPlanAsync(int id)
        {
            try
            {
                await DeleteTrainingPlanActionsAsync(id);
                return await _connection.DeleteAsync<TrainingPlan>(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteTrainingPlanAsync Error: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> DeleteTrainingPlanActionsAsync(int trainingPlanId)
        {
            try
            {
                var actions = await GetTrainingPlanActionsAsync(trainingPlanId);
                foreach (var action in actions)
                {
                    await _connection.DeleteAsync(action);
                }
                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteTrainingPlanActionsAsync Error: {ex.Message}");
                return 0;
            }
        }

        // 添加清理方法用于测试
        public async Task ClearAllDataAsync()
        {
            await _connection.DeleteAllAsync<TrainingPlanAction>();
            await _connection.DeleteAllAsync<TrainingPlan>();
        }

        public async Task CloseAsync()
        {
            await _connection.CloseAsync();
        }
    }
}
