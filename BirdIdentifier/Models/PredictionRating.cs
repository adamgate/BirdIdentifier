namespace BirdIdentifier.Models;

public class PredictionRating
{
    private int PredictionRatingId { get; set; }
    private bool WasCorrect { get; set; }
    private string? CorrectPrediction { get; set; }
    private string Description { get; set; }

    public PredictionRating()
    {
    }
}