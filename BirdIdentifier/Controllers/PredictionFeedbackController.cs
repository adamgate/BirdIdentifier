using BirdIdentifier.DAL;
using BirdIdentifier.Data;
using BirdIdentifier.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BirdIdentifier.Controllers;

/**
 * <summary>Controller accepts user feedback and stores it for later use</summary>
 */
[ApiController]
[Route("feedback")]
public class PredictionFeedbackController : ControllerBase
{ 
    private DataContext _context;
    private PredictionFeedbackService _databaseService;

    public PredictionFeedbackController(DataContext context)
    {
        _context = context;
        _databaseService = new PredictionFeedbackService(_context);
    }
    
    /**
     * <summary>Returns a list of all the feedback in the system.</summary>
     * <response code="200">A list of all feedback in the database.</response>
     */
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //TODO wrap this in a try catch block and throw an error if issues occur
        var feedback = await _databaseService.getFeedback();

        return Ok(JsonConvert.SerializeObject(feedback));
    }
    
    /**
     * <summary>Accepts a prediction rating, which is then saved.</summary>
     * <param name="feedback">a PredictionRating from the user.</param>
     */
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post(PredictionFeedback feedback)
    {
        try
        {
            _databaseService.createFeedback(feedback);
        }
        catch (Exception e)
        {
            return BadRequest("Function not yet implemented.");
        }

        return NoContent();
    }
}