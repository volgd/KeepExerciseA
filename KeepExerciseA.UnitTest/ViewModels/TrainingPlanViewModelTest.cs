using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;
using Moq;
using Xunit;

namespace KeepExerciseA.UnitTest.ViewModels
{
    public class TrainingPlanViewModelTest
    {
        private readonly Mock<ITrainingPlanStorage> _mockTrainingPlanStorage;
        private readonly Mock<IContentNavigationSecvices> _mockNavigationService;
        private readonly Mock<IExerciseTipsStorage> _mockExerciseTipsStorage;
        private readonly TrainingPlanViewModel _viewModel;

        public TrainingPlanViewModelTest()
        {
            // 在每个测试运行前，创建新的 Mock 对象和 ViewModel 实例
            _mockTrainingPlanStorage = new Mock<ITrainingPlanStorage>();
            _mockNavigationService = new Mock<IContentNavigationSecvices>();
            _mockExerciseTipsStorage = new Mock<IExerciseTipsStorage>();

            _viewModel = new TrainingPlanViewModel(
                _mockTrainingPlanStorage.Object,
                _mockNavigationService.Object,
                _mockExerciseTipsStorage.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeCommandsAndProperties()
        {
            // Assert
            Assert.NotNull(_viewModel.TrainingPlans);
            Assert.Empty(_viewModel.TrainingPlans);
            Assert.Equal(string.Empty, _viewModel.DebugMessage);

            Assert.NotNull(_viewModel.LoadTrainingPlansCommand);
            Assert.NotNull(_viewModel.AddTrainingPlanCommand);
            Assert.NotNull(_viewModel.EditTrainingPlanCommand);
            Assert.NotNull(_viewModel.DeleteTrainingPlanCommand);
        }

        [Fact]
        public async Task LoadTrainingPlansAsync_WithValidData_ShouldPopulateCollectionAndMessage()
        {
            // Arrange
            var mockPlans = new List<TrainingPlan>
            {
                new TrainingPlan { Id = 1, Name = "胸部训练", TrainingTime = DateTime.Now },
                new TrainingPlan { Id = 2, Name = "背部训练", TrainingTime = DateTime.Now.AddDays(1) }
            };

            var mockActionsForPlan1 = new List<TrainingPlanAction>
            {
                new TrainingPlanAction { Id = 1, TrainingPlanId = 1, ActionId = 101 },
                new TrainingPlanAction { Id = 2, TrainingPlanId = 1, ActionId = 102 }
            };
            var mockActionsForPlan2 = new List<TrainingPlanAction>
            {
                new TrainingPlanAction { Id = 3, TrainingPlanId = 2, ActionId = 201 }
            };

            var mockTip101 = new ExerciseTips { id = 101, content = "俯卧撑" };
            var mockTip102 = new ExerciseTips { id = 102, content = "哑铃卧推" };
            var mockTip201 = new ExerciseTips { id = 201, content = "引体向上" };

            // 设置 Mock 对象的返回值
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlansAsync()).ReturnsAsync(mockPlans);
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlanActionsAsync(1)).ReturnsAsync(mockActionsForPlan1);
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlanActionsAsync(2)).ReturnsAsync(mockActionsForPlan2);
            _mockExerciseTipsStorage.Setup(s => s.GetTipsAsync(101)).ReturnsAsync(mockTip101);
            _mockExerciseTipsStorage.Setup(s => s.GetTipsAsync(102)).ReturnsAsync(mockTip102);
            _mockExerciseTipsStorage.Setup(s => s.GetTipsAsync(201)).ReturnsAsync(mockTip201);

            // Act - 直接调用异步方法
            await _viewModel.LoadTrainingPlansAsync();

            // Assert
            Assert.Equal(2, _viewModel.TrainingPlans.Count);
            Assert.Equal("加载完成，共 2 个训练计划", _viewModel.DebugMessage);

            // 验证第一个计划
            var plan1 = _viewModel.TrainingPlans[0];
            Assert.Equal("胸部训练", plan1.Plan.Name);
            Assert.Equal(2, plan1.ExerciseTips.Count);
            Assert.Contains(plan1.ExerciseTips, t => t.content == "俯卧撑");
            Assert.Contains(plan1.ExerciseTips, t => t.content == "哑铃卧推");

            // 验证第二个计划
            var plan2 = _viewModel.TrainingPlans[1];
            Assert.Equal("背部训练", plan2.Plan.Name);
            Assert.Single(plan2.ExerciseTips);
            Assert.Equal("引体向上", plan2.ExerciseTips[0].content);
        }
        

        [Fact]
        public async Task LoadTrainingPlansAsync_WhenStorageThrowsException_ShouldHandleErrorAndUpdateMessage()
        {
            // Arrange
            var errorMessage = "Database connection failed";
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlansAsync()).ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            await _viewModel.LoadTrainingPlansAsync();

            // Assert
            Assert.Empty(_viewModel.TrainingPlans);
            Assert.Equal($"加载失败: {errorMessage}", _viewModel.DebugMessage);
        }
        

        [Fact]
        public void AddTrainingPlanCommand_ShouldNavigateToAddView()
        {
            // Act
            _viewModel.AddTrainingPlanCommand.Execute(null);

            // Assert - 修复：使用 It.IsAny 来处理可选参数
            _mockNavigationService.Verify(ns => ns.NavigateTo("AddTrainingPlan", It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void EditTrainingPlanCommand_ShouldNavigateToEditViewWithId()
        {
            // Arrange
            var item = new TrainingPlanItem { Plan = new TrainingPlan { Id = 123, Name = "Test Plan" } };

            // Act
            _viewModel.EditTrainingPlanCommand.Execute(item);

            // Assert - 修复：使用 It.IsAny 来处理可选参数
            _mockNavigationService.Verify(ns => ns.NavigateTo("AddTrainingPlan", It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTrainingPlanCommand_ShouldDeletePlanAndReload()
        {
            // Arrange
            var item = new TrainingPlanItem { Plan = new TrainingPlan { Id = 456, Name = "Plan To Delete" } };
            _mockTrainingPlanStorage.Setup(s => s.DeleteTrainingPlanAsync(456)).ReturnsAsync(1); // 模拟成功删除1行
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlansAsync()).ReturnsAsync(new List<TrainingPlan>());

            // Act - 修复：使用 Execute 而不是 ExecuteAsync
            _viewModel.DeleteTrainingPlanCommand.Execute(item);

            // 等待异步操作完成
            await Task.Delay(100);

            // Assert
            _mockTrainingPlanStorage.Verify(s => s.DeleteTrainingPlanAsync(456), Times.Once);
            // 验证删除后是否重新加载了数据
            _mockTrainingPlanStorage.Verify(s => s.GetTrainingPlansAsync(), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_ShouldCallLoadTrainingPlans()
        {
            // Arrange
            // 模拟 LoadTrainingPlans 什么都不做，只为了验证它被调用
            _mockTrainingPlanStorage.Setup(s => s.GetTrainingPlansAsync()).ReturnsAsync(new List<TrainingPlan>());

            // Act
            await _viewModel.InitializeAsync();

            // Assert
            _mockTrainingPlanStorage.Verify(s => s.GetTrainingPlansAsync(), Times.Once);
        }
    }
}
