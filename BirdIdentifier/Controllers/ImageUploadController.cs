using Microsoft.AspNetCore.Mvc;
using UploadImage_Api.Util;

namespace UploadImage_Api.Controllers;

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

            var filePath = $@"./UploadedImages/{checksum}{fileExt}";

            //save file to the UploadedImages folder
            await using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
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

        return NotFound();
    }
}