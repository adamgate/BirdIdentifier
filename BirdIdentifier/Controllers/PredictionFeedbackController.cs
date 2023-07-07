using BirdIdentifier.DAL;
using BirdIdentifier.Data;
using BirdIdentifier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BirdIdentifier.Controllers;

/**
 * <summary>Controller accepts user feedback and stores it for later use</summary>
 */
[ApiController]
[Route("feedback")]
public class PredictionFeedbackController : ControllerBase
{
    private readonly PredictionFeedbackService _databaseService;

    public PredictionFeedbackController(DataContext context)
    {
        _databaseService = new PredictionFeedbackService(context);
    }

    /**
     * <summary>Returns a PredictionFeedback object.</summary>
     * <param name="id">The feedback id to be returned.</param>
     * <response code="200">The feedback with the given id.</response>
     * <response code="400">If no feedback was found with the id.</response>
     */
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        PredictionFeedback feedback;
        try
        {
            feedback = _databaseService.getFeedback(id);
        }
        catch (InvalidOperationException ioe)
        {
            return BadRequest("No item found with that id.");
        }

        return Ok(JsonConvert.SerializeObject(feedback, Formatting.Indented));
    }

    /**
     * <summary>Returns a list of all the feedback in the system.</summary>
     * <response code="200">A list of all feedback in the database.</response>
     */
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<PredictionFeedback> feedback;
        try
        {
            feedback = await _databaseService.getAllFeedback();
        }
        catch (InvalidOperationException ioe)
        {
            return StatusCode(500, ioe.Message);
        }

        return Ok(JsonConvert.SerializeObject(feedback, Formatting.Indented));
    }

    /**
     * <summary>Accepts a prediction feedback, which is then saved.</summary>
     * <param name="feedbackIn">a PredictionFeedback from the user.</param>
     * <response code="200">If the feedback was saved correctly.</response>
     * <response code="400">If the request body was empty.</response>
     */
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post(PredictionFeedback feedbackIn)
    {
        PredictionFeedback feedbackOut;
        try
        {
            feedbackOut = await _databaseService.createFeedback(feedbackIn);
        }
        catch (DbUpdateException dbe)
        {
            return BadRequest(dbe.Message);
        }

        return Ok(JsonConvert.SerializeObject(feedbackOut, Formatting.Indented));
    }
}
