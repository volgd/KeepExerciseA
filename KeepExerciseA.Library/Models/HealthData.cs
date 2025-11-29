// Models/HealthData.cs
namespace HealthAssessment.Models;

public record HealthData
{
    public double Height { get; init; } // cm
    public double Weight { get; init; } // kg
    public double BodyFatPercentage { get; init; } // %
    public string Gender { get; init; } // "男" or "女"
    public double Chest { get; init; } // cm
    public double Waist { get; init; } // cm
    public double Hips { get; init; } // cm
    public double Arm { get; init; } // cm
}