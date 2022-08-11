using BirdIdentifier.Models;
using Microsoft.AspNetCore.Mvc;

using BirdIdentifier.Utils;
using Newtonsoft.Json;

namespace BirdIdentifier.Controllers;

/**
 * <summary>Controller that accepts users images and processes them with an ML model</summary>
 */
[ApiController]
[Route("images")]
public class ImageUploadController : ControllerBase
{

    /**
     * <summary>Receives an image, analyzes it with an ML model, and returns the prediction to the user.</summary>
     * <param name="image">A file sent with the http request</param>
     * <returns>Http Status Code, a 400 with error or a 200 with a prediction.</returns>
     */
    [HttpPost]
    public async Task<IActionResult> OnPostUpload(IFormFile image)
    {
        var fileExt = Path.GetExtension(image.FileName);
        
        //Check for the correct filetypes & return error if they don't match
        if (fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".png")
            return BadRequest("File was not of type .png, .jpg, or .jpeg.");

        var prediction = new Prediction
        {
            //Create a checksum from the image to avoid saving duplicate files
            ImageChecksum = EncodingUtils.ToChecksum(image)
        };

        //Create directory if it doesn't exist
        Directory.CreateDirectory(@"./UploadedImages");
        var filePath = $@"./UploadedImages/{prediction.ImageChecksum}{fileExt}";
            
        // Save the file if it isn't already present
        if (!System.IO.File.Exists(filePath))
        {
            await using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
        }
        
        //Convert image into format the ML model can use
        var mlData = new MLModel.ModelInput
        {
            ImageSource = filePath
        };
            
        //Pass in the data and get a prediction
        var predictionResult = MLModel.Predict(mlData);

        prediction.PredictionName = predictionResult.Prediction;
        prediction.PredictionScore = predictionResult.Score;
        prediction.Timestamp = DateTime.UtcNow;

        //String processing
        prediction.PredictionName = prediction.PredictionName.Replace("-", " ");
        prediction.PredictionName = prediction.PredictionName.ToLower();
        //TODO- put parsing logic in prediction helper function
        prediction.SearchLink = $"https://www.audubon.org/search_results?search={prediction.PredictionName}";
        prediction.SearchLink = prediction.SearchLink.Replace(" ", "%20");
        prediction.ExactLink = $"https://www.audubon.org/field-guide/bird/{prediction.PredictionName}";
        prediction.ExactLink = prediction.ExactLink.Replace(" ", "-");

        //TODO - convert this to logging with serilog
        Console.WriteLine($"Prediction: {prediction.PredictionName} | Time: {prediction.Timestamp:f} | User: {Request.Headers["User-Agent"].ToString()}");

        Console.WriteLine(JsonConvert.SerializeObject(prediction));
        
        //Return prediction to the front end
        return Ok(JsonConvert.SerializeObject(prediction));
    }
}