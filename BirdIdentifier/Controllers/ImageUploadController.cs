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
     * <summary>Used to determine if the service is reachable</summary>
     * <returns>Http Status Code</returns>
     */
    [HttpGet]
    public IActionResult Heartbeat()
    {
        Console.WriteLine("Heartbeat reached");
        return Ok();
    }
    
    /**
     * <summary>Used to determine if the service is reachable</summary>
     * <param name="image">A file sent with the http request</param>
     * <returns>Http Status Code</returns>
     */

    [HttpPost]
    public async Task<IActionResult> OnPostUpload(IFormFile image)
    {
        var fileExt = Path.GetExtension(image.FileName);
        
        //Check for the correct filetypes & return error if they don't match
        if (fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".png")
            return BadRequest("File was not of type .png, .jpg, or .jpeg.");
        
        //Create a checksum from the image to avoid duplicate files
        var checksum = ChecksumUtils.GetChecksum(image);
        
        //Create directory if it doesn't exist
        Directory.CreateDirectory(@"./UploadedImages");
        var filePath = $@"./UploadedImages/{checksum}{fileExt}";
            
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
        
        //Return prediction to the front end
        return Ok(JsonConvert.SerializeObject(predictionResult.Prediction));
    }
}