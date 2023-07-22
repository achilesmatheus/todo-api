using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [SwaggerOperation(Summary = "Api heath check")]
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
