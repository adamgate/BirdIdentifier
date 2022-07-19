using Microsoft.AspNetCore.Mvc;

namespace BirdIdentifier.Controllers;

public class ImageUploadController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}