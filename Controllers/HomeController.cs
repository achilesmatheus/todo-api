using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("");
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
