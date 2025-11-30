namespace BirdIdentifierBackend.Models
{
    public class Prediction
    {
        public DateTime Timestamp { get; set; }
        public required string Name { get; set; }
        public decimal Score { get; set; }
        public required string LearnMoreLink { get; set; }
    }
}
