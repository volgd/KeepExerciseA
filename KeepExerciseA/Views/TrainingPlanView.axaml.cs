using Avalonia;
using Avalonia.Controls;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Views
{
    public partial class TrainingPlanView : UserControl
    {
        public TrainingPlanView()
        {
            InitializeComponent();
        }

        protected override async void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (DataContext is TrainingPlanViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }
    }
}