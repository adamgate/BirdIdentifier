using System.Linq;

using BirdIdentifier.Data;
using BirdIdentifier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BirdIdentifier.DAL;

public class PredictionFeedbackService
{
    private DataContext _context;

    public  PredictionFeedbackService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<PredictionFeedback>> getFeedback()
    {
        return await _context.PredictionFeedback.ToListAsync();
    }

    public async Task createFeedback(PredictionFeedback feedback)
    {
        feedback.Timestamp ??= DateTime.UtcNow;
        
        _context.Add(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task updateFeedback(int id, PredictionFeedback feedback)
    {
        
    }

    public async Task deleteFeedback(int id)
    {
        
    }
}