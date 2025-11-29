using SQLite;
using System;

namespace KeepExerciseA.Library.Models
{
    [Table("TrainingPlan")]
    public class TrainingPlan
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime TrainingTime { get; set; }
    }
}