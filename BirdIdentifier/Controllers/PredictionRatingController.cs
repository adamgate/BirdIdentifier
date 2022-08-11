using BirdIdentifier.Models;
using Microsoft.AspNetCore.Mvc;

namespace BirdIdentifier.Controllers;

/**
 * <summary>Controller accepts user feedback and stores it for later use</summary>
 */
[ApiController]
[Route("feedback")]
public class PredictionRatingController : ControllerBase
{
    /**
     * <summary>Accepts a prediction rating, which is then saved.</summary>
     * <param name="feedback">a PredictionRating from the user.</param>
     */
    [HttpPost]
    public async Task<IActionResult> Post(PredictionRating feedback)
    {
        //check the data to ensure it's accurate

        //Throw 400 if the request is bad

        //save the prediction in await. if successful, return 200. If unsuccessful, return 500.

        return BadRequest("Function not yet implemented.");
    }
}