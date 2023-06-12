using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    // Heath check
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(new
        {
            Title = "Api heath check",
            Online = true
        });
    }
}
