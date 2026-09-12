using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    // Tracks, per model group, how much data existed the last time it was
    // (re)trained - lets the scheduler decide "has enough new data piled up
    // (or enough time passed) to retrain again" without re-deriving that
    // from scratch on every check.
    public class MlRetrainingWatermark
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string ModelGroup { get; set; } = default!;

        public DateTime LastTrainedAt { get; set; }
        public long RecordCountAtLastTraining { get; set; }
    }
}
