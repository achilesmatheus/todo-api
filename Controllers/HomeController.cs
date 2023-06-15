using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    // Heath check
    [HttpGet("v1")]
    public IActionResult Get()
    {
        return Ok(new
        {
            Title = "Api heath check",
            Online = true
        });
    }
}
