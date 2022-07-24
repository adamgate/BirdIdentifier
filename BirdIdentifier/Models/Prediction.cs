namespace BirdIdentifier.Models;

public class Prediction
{
    private int PredictionId { get; set; }
    private int ImageChecksum { get; set; }
    private string ImageBase64 { get; set; }
    private DateTime Timestamp { get; set; }
    private string PredictionName { get; set; }
    private float PredictionScore { get; set; }
    private string Link { get; set; }

    public Prediction()
    {
    }
}