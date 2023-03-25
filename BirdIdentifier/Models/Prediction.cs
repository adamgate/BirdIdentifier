namespace BirdIdentifier.Models;

public class Prediction
{
    public string ImageChecksum { get; set; }
    public DateTime Timestamp { get; set; }
    public string PredictionName { get; set; }
    public decimal PredictionScore { get; set; }
    public string LearnMoreLink { get; set; }

    public Prediction()
    {
    }

    public void FindHighestScore(float[] scores)
    {
        PredictionScore = decimal.Round((decimal)scores.Max() * 100, 2);
    }
}