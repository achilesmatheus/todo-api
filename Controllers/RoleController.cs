using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Models;
using Todo.Repositories.Contracts;
using Todo.ViewModels;

namespace Todo.Controllers;

// [Authorize(Roles = "User")]
[ApiController]
[Route("v1")]
public class RoleController : ControllerBase
{
    [SwaggerOperation(Summary = "Returns a list of roles")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAll(
        [FromServices] IRoleRepository repository,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 5
    )
    {
        try
        {
            var roles = await repository.GetAllAsync(skip, take);

            var result = new ResultViewModel<IEnumerable<RoleModel>>(roles);
            return StatusCode(200, result);
        }
        catch (DbUpdateException)
        {
            var result = new ResultViewModel<string>("Could not fetch roles");
            return StatusCode(200, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(200, result);
        }
    }

    [SwaggerOperation(Summary = "Return a role with provided id")]
    [HttpGet("role/{id:int}")]
    public async Task<IActionResult> GetById(
            [FromServices] IRoleRepository repository,
            [FromRoute] int id
        )
    {
        try
        {
            var role = await repository.GetByIdAsync(id);
            if (role == null) throw new DbUpdateException("The role was not found");

            var result = new ResultViewModel<RoleModel>(role);
            return StatusCode(200, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(500, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Creates a new role")]
    [HttpPost("role")]
    public async Task<IActionResult> CreateTask(
    [FromServices] IRoleRepository repository,
    [FromBody] RoleViewModel model
)
    {
        try
        {
            var role = new RoleModel
            {
                Name = model.Name,
            };

            await repository.CreateAsync(role);

            var result = new ResultViewModel<RoleModel>(role);
            return StatusCode(201, result);

        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(404, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Updates a role")]
    [HttpPut("role/{id:int}")]
    public async Task<IActionResult> UpdateTask(
        [FromServices] IRoleRepository repository,
        [FromBody] RoleViewModel model,
        [FromRoute] int id
    )
    {
        try
        {
            var role = await repository.GetByIdAsync(id);
            if (role == null) throw new DbUpdateException($"The role with id {id} was not found");

            role.Name = model.Name;
            role.UpdatedAt = DateTime.Now;

            await repository.UpdateAsync(role);

            var result = new ResultViewModel<RoleModel>(role);
            return StatusCode(200, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(404, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Deletes a role")]
    [HttpDelete("role/{id:int}")]
    public async Task<IActionResult> Delete(
        [FromServices] IRoleRepository repository,
        [FromRoute] int id
)
    {
        try
        {
            var role = await repository.GetByIdAsync(id);
            if (role == null) throw new DbUpdateException($"The role with id '{id}' does not exist");

            await repository.DeleteAsync(role);

            var result = new ResultViewModel<RoleModel>(role);
            return StatusCode(200, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(404, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

}