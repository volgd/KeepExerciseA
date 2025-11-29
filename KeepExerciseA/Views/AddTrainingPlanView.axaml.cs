using Avalonia;
using Avalonia.Controls;
using KeepExerciseA.Library.Models;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Views
{
    public partial class AddTrainingPlanView : UserControl
    {
        public AddTrainingPlanView()
        {
            InitializeComponent();
            // 添加按钮事件处理
            AddToSelectedButton.Click += AddToSelectedButton_Click;
            RemoveFromSelectedButton.Click += RemoveFromSelectedButton_Click;
        }

        protected override async void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (DataContext is AddTrainingPlanViewModel viewModel)
            {
                if (viewModel.Parameter is int trainingPlanId)
                    await viewModel.InitializeAsync(trainingPlanId);
                else
                    await viewModel.InitializeAsync();
            }
        }

        private void AddToSelectedButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is AddTrainingPlanViewModel viewModel)
            {
                var selectedItem = AvailableActionsList.SelectedItem as ExerciseTips;
                if (selectedItem != null && !viewModel.SelectedActions.Contains(selectedItem))
                {
                    viewModel.SelectedActions.Add(selectedItem);
                }
            }
        }

        private void RemoveFromSelectedButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is AddTrainingPlanViewModel viewModel)
            {
                var selectedItem = SelectedActionsList.SelectedItem as ExerciseTips;
                if (selectedItem != null)
                {
                    viewModel.SelectedActions.Remove(selectedItem);
                }
            }
        }
    }
}