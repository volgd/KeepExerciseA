using CommunityToolkit.Mvvm.ComponentModel;

namespace KeepExerciseA.Library.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public virtual void SetParameter(object parameter) { }
}