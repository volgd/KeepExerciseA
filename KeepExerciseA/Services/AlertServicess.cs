using System.Threading.Tasks;
using KeepExerciseA.Library.Services;
using Ursa.Controls;

namespace KeepExerciseA.Services;

public class AlertServicess : IAlertServices
{
    public async Task AlertAsync(string title, string message)
    {
        await MessageBox.ShowAsync(message,title,button:MessageBoxButton.OK);
        
    }
}