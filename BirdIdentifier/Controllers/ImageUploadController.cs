using Microsoft.AspNetCore.Mvc;

using BirdIdentifier.Utils;

namespace BirdIdentifier.Controllers;

[ApiController]
[Route("images")]
public class ImageUploadController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> OnPostUpload(IFormFile file)
    {
        var fileExt = Path.GetExtension(file.FileName);

        //Check for the correct filetypes
        if (fileExt == ".jpg" ||
            fileExt == ".jpeg" ||
            fileExt == ".png")
        {
            //Create a checksum used as the file name to avoid duplicate files
            var checksum = ChecksumUtils.GetChecksum(file);

            //Create directory if it doesn't exist
            Directory.CreateDirectory(@"./UploadedImages");
            var filePath = $@"./UploadedImages/{checksum}{fileExt}";
            
            // Save the file if it isn't already present
            if (!System.IO.File.Exists(filePath))
            {
                await using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            //Convert image into format the ML model can use
            var mlData = new MLModel.ModelInput
            {
                ImageSource = filePath
            };
            
            //Pass in the data and get a prediction
            var predictionResult = MLModel.Predict(mlData);

                //Return prediction to user
            return Ok(predictionResult.Prediction);
        }
        
        return BadRequest("File was not of type .png, .jpg, or .jpeg.");
    }
}