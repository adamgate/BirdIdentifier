namespace BirdIdentifier.Models;

public class Prediction
{
    public int PredictionId { get; set; }
    public string ImageChecksum { get; set; }
    public DateTime Timestamp { get; set; }
    public string PredictionName { get; set; }
    public float[] PredictionScore { get; set; }
    public string ExactLink { get; set; }
    public string SearchLink { get; set; }

    public Prediction()
    {
    }
}