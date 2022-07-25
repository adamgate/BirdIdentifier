namespace BirdIdentifier.Models;

public class PredictionRating
{
    public int PredictionRatingId { get; set; }
    public bool WasCorrect { get; set; }
    public string? CorrectPrediction { get; set; }
    public string Description { get; set; }

    public PredictionRating()
    {
    }
}