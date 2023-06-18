using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.Repositories;
using Todo.Repositories.Contracts;
using Todo.ViewModels;

namespace Todo.Controllers;

[ApiController]
[Route("v1")]
public class FolderController : ControllerBase
{
    [HttpGet("folders")]
    public async Task<IActionResult> GetAll(
        [FromServices] IFolderRepository repository
    )
    {
        try
        {
            var folders = await repository.GetAllAsync();
            var result = new ResultViewModel<IEnumerable<FolderModel>>(folders);
            return StatusCode(200, result);

        }
        catch
        {
            var result = new ResultViewModel<string>("Could not fetch folders");
            return StatusCode(500, result);
        }
    }

    [HttpGet("folder/{id:int}")]
    public async Task<IActionResult> GetById(
        [FromServices] IFolderRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var folder = await repository.GetByIdAsync(id);
            if (folder == null) throw new DbUpdateException("The folder was not found");

            var result = new ResultViewModel<FolderModel>(folder);
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

    [HttpPost("folder")]
    public async Task<IActionResult> CreateList(
        [FromServices] IFolderRepository repository,
        [FromBody] FolderViewModel model
    )
    {
        try
        {
            var folder = new FolderModel
            {
                Name = model.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await repository.CreateAsync(folder);

            var result = new ResultViewModel<FolderModel>(folder);
            return StatusCode(201, result);

        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>("Could not create folder");
            return StatusCode(500, result);

        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);

        }
    }

    [HttpPut("folder/{id:int}")]
    public async Task<IActionResult> UpdateList(
        [FromServices] IFolderRepository repository,
        [FromBody] FolderViewModel model,
        [FromRoute] int id
    )
    {
        try
        {
            var folder = await repository.GetByIdAsync(id);
            if (folder == null) throw new DbUpdateException($"The folder with id {id} was not found");

            folder.Name = model.Name;
            folder.UpdatedAt = DateTime.Now;

            await repository.UpdateAsync(folder);

            var result = new ResultViewModel<FolderModel>(folder);
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

    [HttpDelete("folder/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] IFolderRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var folder = await repository.GetByIdAsync(id);
            if (folder == null) throw new DbUpdateException($"The folder with id '{id}' does not exist");

            await repository.DeleteAsync(folder);

            var result = new ResultViewModel<FolderModel>(folder);
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
