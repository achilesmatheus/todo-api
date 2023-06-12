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

    [HttpGet("/{id:int}")]
    public IActionResult GetById()
    {
        return Ok("");
    }

    [HttpPost("/")]
    public IActionResult Create()
    {
        return Ok("");
    }

    [HttpPut("/{id:int}")]
    public IActionResult Update()
    {
        return Ok("");
    }

    [HttpDelete("/{id:int}")]
    public IActionResult Delete()
    {
        return Ok("");
    }
}
