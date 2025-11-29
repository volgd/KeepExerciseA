// KeepExerciseA/Library/Models/TrainingPlanAction.cs
using SQLite;

namespace KeepExerciseA.Library.Models
{
    [Table("TrainingPlanAction")]
    public class TrainingPlanAction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TrainingPlanId { get; set; }
        public int ActionId { get; set; }

        public override string ToString()
        {
            return $"TrainingPlanAction(Id={Id}, TrainingPlanId={TrainingPlanId}, ActionId={ActionId})";
        }
    }
}