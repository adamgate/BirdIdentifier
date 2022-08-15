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

    public PredictionFeedback getFeedback(int id)
    {
        return _context.PredictionFeedback.First(pf => pf.PredictionFeedbackId == id);
    }

    public async Task<List<PredictionFeedback>> getAllFeedback()
    {
        return await _context.PredictionFeedback.ToListAsync();
    }

    public async Task createFeedback(PredictionFeedback feedback)
    {
        feedback.Timestamp ??= DateTime.UtcNow;
        
        _context.Add(feedback);
        await _context.SaveChangesAsync();
    }
}