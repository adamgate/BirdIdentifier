using BirdIdentifierBackend.Models;
using BirdIdentifierBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BirdIdentifierBackend.Controllers
{
    [ApiController]
    [Route("birds/identification")]
    public class BirdsController : ControllerBase
    {
        private static readonly string[] AcceptableImageFormats = [".jpg", ".jpeg", ".png"];
        private const string ImageFolder = @"./UploadedImages";
        private const string LearnMoreBaseLink = "https://www.google.com/search?q={0}+bird";

        /// <summary>
        /// Checks to see if the image is of a bird, and then predicts what type of bird it is.
        /// </summary>
        [HttpPost("classify")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GetBirdClassification(IFormFile image, [FromQuery] int version = 1)
        {
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AcceptableImageFormats.Contains(fileExtension))
            { 
                return BadRequest("Incorrect file type.");
            }

            var imageChecksum = EncodingUtils.ToChecksum(image);

            // Save the file for processing if it isn't already saved
            Directory.CreateDirectory(@"./UploadedImages"); // Create directory if it doesn't exist
            var filePath = Path.Join(ImageFolder, $"{imageChecksum}{fileExtension}");
            if (!System.IO.File.Exists(filePath))
            {
                await using var stream = System.IO.File.Create(filePath);
                await image.CopyToAsync(stream);
            }

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            Prediction? prediction;

            // Determine what kind of bird image is
            if (version == 1)
            {
                var birdClassificationV1Input = new BirdClassificationModelV1.ModelInput { ImageSource = filePath };
                var birdClassificationV1Result = BirdClassificationModelV1.Predict(birdClassificationV1Input);

                prediction = new()
                {
                    Name = birdClassificationV1Result.Prediction,
                    Timestamp = DateTime.Now,
                    Score = GetHighestScore(birdClassificationV1Result.Score),
                    LearnMoreLink = GenerateLearnMoreLink(birdClassificationV1Result.Prediction)
                };
            }
            else if (version == 2)
            {
                // Determine if image is a bird
                var birdDetectorInput = new BirdDetectorModel.ModelInput { ImageSource = imageBytes };
                var birdDetectorResult = BirdDetectorModel.Predict(birdDetectorInput);
                if (!birdDetectorResult.PredictedLabel.Equals("bird"))
                {
                    prediction = new Prediction()
                    {
                        Name = birdDetectorResult.PredictedLabel,
                        Timestamp = DateTime.Now,
                        Score = GetHighestScore(birdDetectorResult.Score),
                        LearnMoreLink = ""
                    };
                    return Ok(JsonConvert.SerializeObject(prediction, Formatting.Indented));
                }

                // Determine what kind of bird image is
                var birdClassificationV2Input = new BirdClassificationModelV2.ModelInput { ImageSource = imageBytes };
                var birdClassificationV2Result = BirdClassificationModelV2.Predict(birdClassificationV2Input);

                prediction = new()
                {
                    Name = birdClassificationV2Result.PredictedLabel,
                    Timestamp = DateTime.Now,
                    Score = GetHighestScore(birdClassificationV2Result.Score),
                    LearnMoreLink = GenerateLearnMoreLink(birdClassificationV2Result.PredictedLabel)
                };
            }
            else
            { 
                return BadRequest("Version number not recognized.");
            }

            return Ok(JsonConvert.SerializeObject(prediction, Formatting.Indented));
        }

        /// <summary>
        /// Predicts what bird the image most closely resembles.
        /// </summary>
        [HttpPost("persona")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GetBirdPersona(IFormFile image, [FromQuery] int version = 1)
        {
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AcceptableImageFormats.Contains(fileExtension))
            { 
                return BadRequest("Incorrect file type.");
            }

            var imageChecksum = EncodingUtils.ToChecksum(image);

            // Save the file for processing if it isn't already saved
            Directory.CreateDirectory(@"./UploadedImages"); // Create directory if it doesn't exist
            var filePath = Path.Join(ImageFolder, $"{imageChecksum}{fileExtension}");
            if (!System.IO.File.Exists(filePath))
            {
                await using var stream = System.IO.File.Create(filePath);
                await image.CopyToAsync(stream);
            }

            // Determine what kind of bird image is
            Prediction? prediction;
            if (version == 1)
            {
                var birdClassificationV1Input = new BirdClassificationModelV1.ModelInput { ImageSource = filePath };
                var birdClassificationV1Result = BirdClassificationModelV1.Predict(birdClassificationV1Input);

                prediction = new()
                {
                    Name = birdClassificationV1Result.Prediction,
                    Timestamp = DateTime.Now,
                    Score = GetHighestScore(birdClassificationV1Result.Score),
                    LearnMoreLink = GenerateLearnMoreLink(birdClassificationV1Result.Prediction)
                };
            }
            else if (version == 2)
            {
                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                var birdClassificationV2Input = new BirdClassificationModelV2.ModelInput { ImageSource = imageBytes };
                var birdClassificationV2Result = BirdClassificationModelV2.Predict(birdClassificationV2Input);

                prediction = new()
                {
                    Name = birdClassificationV2Result.PredictedLabel,
                    Timestamp = DateTime.Now,
                    Score = GetHighestScore(birdClassificationV2Result.Score),
                    LearnMoreLink = GenerateLearnMoreLink(birdClassificationV2Result.PredictedLabel)
                };
            }
            else
            { 
                return BadRequest("Version number not recognized.");
            }

            return Ok(JsonConvert.SerializeObject(prediction, Formatting.Indented));
        }

        private decimal GetHighestScore(float[] scores)
            => decimal.Round((decimal)scores.Max() * 100, 2);
        
        private string GenerateLearnMoreLink(string bird)
        {
            var encodedBirdName = new StringBuilder()
                .Append(bird)
                .Replace("-", "+")
                .Replace(" ", "+")
                .ToString()
                .ToLowerInvariant();

            return string.Format(LearnMoreBaseLink, encodedBirdName);
        }
    }
}
