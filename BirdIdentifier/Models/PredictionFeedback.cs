using System.ComponentModel.DataAnnotations;

namespace BirdIdentifier.Models;

public class PredictionFeedback
{
    public int PredictionFeedbackId { get; set; }
    public DateTime? Timestamp { get; set; }

    [Required]
    public bool WasCorrect { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageChecksum { get; set; }

    [Required]
    public string Prediction { get; set; }

    public PredictionFeedback() { }
}
