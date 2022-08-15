namespace BirdIdentifier.Models;

public class PredictionRating
{
    public int PredictionRatingId { get; set; }
    public string ImageChecksum { get; set; }
    public DateTime Timestamp { get; set; }
    public bool WasCorrect { get; set; }
    public string Description { get; set; }

    public PredictionRating()
    {
    }
}