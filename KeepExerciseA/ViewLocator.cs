using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using KeepExerciseA.Library.ViewModels;
using KeepExerciseA.ViewModels;

namespace KeepExerciseA;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param) //Match成立会调用Build
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace("KeepExerciseA.Library.","KeepExerciseA.");
        
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}