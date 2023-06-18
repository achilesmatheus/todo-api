using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.Repositories;
using Todo.Repositories.Contracts;
using Todo.ViewModels;

namespace Todo.Controllers;

[ApiController]
[Route("v1")]
public class ListController : ControllerBase
{
    [HttpGet("lists")]
    public async Task<IActionResult> GetAll(
        [FromServices] IListRepository repository
    )
    {
        try
        {
            var lists = await repository.GetAllAsync();
            var result = new ResultViewModel<IEnumerable<ListModel>>(lists);
            return StatusCode(200, result);

        }
        catch
        {
            var result = new ResultViewModel<string>("Could not fetch lists");
            return StatusCode(500, result);
        }
    }

    [HttpGet("list/{id:int}")]
    public async Task<IActionResult> GetById(
        [FromServices] IListRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var list = await repository.GetByIdAsync(id);
            if (list == null) throw new DbUpdateException("The list was not found");

            var result = new ResultViewModel<ListModel>(list);
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

    [HttpPost("list")]
    public async Task<IActionResult> CreateList(
        [FromServices] IListRepository repository,
        [FromBody] ListViewModel model
    )
    {
        try
        {
            var list = new ListModel
            {
                Name = model.Name,
            };

            await repository.CreateAsync(list);

            var result = new ResultViewModel<ListModel>(list);
            return StatusCode(201, result);

        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>("Could not create list");
            return StatusCode(500, result);

        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);

        }
    }

    [HttpPut("list/{id:int}")]
    public async Task<IActionResult> UpdateList(
        [FromServices] IListRepository repository,
        [FromBody] ListViewModel model,
        [FromRoute] int id
    )
    {
        try
        {
            var list = await repository.GetByIdAsync(id);
            if (list == null) throw new DbUpdateException($"The list with id {id} was not found");

            list.Name = model.Name;
            list.UpdatedAt = DateTime.Now;

            await repository.UpdateAsync(list);

            var result = new ResultViewModel<ListModel>(list);
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

    [HttpPut("list/{listId:int}/folder/{folderId:int}")]
    public async Task<IActionResult> MoveListToFolder(
       [FromServices] FolderRepository folderRepository,
       [FromServices] IListRepository listRepository,
       [FromRoute] int listId,
       [FromRoute] int folderId

   )
    {
        try
        {
            var folder = await folderRepository.GetByIdAsync(folderId);
            var list = await listRepository.GetByIdAsync(listId);

            if (folder == null) throw new DbUpdateException($"Folder with id '{folderId}' could not be found");
            if (list == null) throw new DbUpdateException($"List with id '{listId}' could not be found");

            folder.AddList(list);
            await listRepository.UpdateAsync(list);

            var result = new ResultViewModel<ListModel>(list);
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

    [HttpDelete("list/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] IListRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var list = await repository.GetByIdAsync(id);
            if (list == null) throw new DbUpdateException($"The list with id '{id}' does not exist");

            await repository.DeleteAsync(list);

            var result = new ResultViewModel<ListModel>(list);
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
