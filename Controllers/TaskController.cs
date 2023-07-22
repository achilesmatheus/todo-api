using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Models;
using Todo.Repositories;
using Todo.Repositories.Contracts;
using Todo.ViewModels;

namespace Todo.Controllers;

// [Authorize(Roles = "User")]
[ApiController]
[Route("v1")]
public class TaskController : ControllerBase
{
    [SwaggerOperation(Summary = "Returns a list of tasks")]
    [HttpGet("tasks")]
    public async Task<IActionResult> GetAll(
        [FromServices] ITaskRepository repository,
        [FromQuery] int skip,
        [FromQuery] int take
    )
    {
        try
        {
            var tasks = await repository.GetAllAsync(skip, take);
            var result = new ResultViewModel<IEnumerable<TaskModel>>(tasks);
            return StatusCode(200, result);

        }
        catch
        {
            var result = new ResultViewModel<string>("Could not fetch task list");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Return a user with provided id")]
    [HttpGet("task/{id:int}")]
    public async Task<IActionResult> GetById(
            [FromServices] ITaskRepository repository,
            [FromRoute] int id
        )
    {
        try
        {
            var task = await repository.GetByIdAsync(id);
            if (task == null) throw new DbUpdateException("The task was not found");

            var result = new ResultViewModel<TaskModel>(task);
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

    [SwaggerOperation(Summary = "Returns a list of tasks that match the provided date")]
    [HttpGet("tasks/{date:datetime}")]
    public async Task<IActionResult> GetByPeriod(
        [FromServices] ITaskRepository repository,
        [FromRoute] DateTime date
    )
    {
        try
        {
            var tasks = await repository.GetByPeriodAsync(date);
            if (tasks == null) throw new DbUpdateException("The task was not found");

            var result = new ResultViewModel<IEnumerable<TaskModel>>(tasks);
            return StatusCode(200, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(404, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("The specified items could not be found");
            return StatusCode(500, result);
        }

    }

    [SwaggerOperation(Summary = "Creates a new Task")]
    [HttpPost("task")]
    public async Task<IActionResult> CreateTask(
        [FromServices] ITaskRepository repository,
        [FromBody] TaskViewModel model
    )
    {
        try
        {
            var task = new TaskModel
            {
                Title = model.Title,
                Description = model.Description,
            };

            await repository.CreateAsync(task);

            var result = new ResultViewModel<TaskModel>(task);
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

    [SwaggerOperation(Summary = "Updates a task")]
    [HttpPut("task/{id:int}")]
    public async Task<IActionResult> UpdateTask(
        [FromServices] ITaskRepository repository,
        [FromBody] TaskViewModel viewModel,
        [FromRoute] int id
    )
    {
        try
        {
            var task = await repository.GetByIdAsync(id);
            if (task == null) throw new DbUpdateException($"The task with id {id} was not found");

            task.Title = viewModel.Title;
            task.Description = viewModel.Description;
            task.UpdatedAt = DateTime.Now;

            await repository.UpdateAsync(task);

            var result = new ResultViewModel<TaskModel>(task);
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

    [SwaggerOperation(Summary = "Marks a task as done")]
    [HttpPut("task/{id:int}/mark-as-done")]
    public async Task<IActionResult> MarkAsDone(
        [FromServices] ITaskRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var task = await repository.GetByIdAsync(id);
            if (task == null) throw new DbUpdateException($"The task with id {id} was not found");

            task.Done = true;
            task.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(task);

            var result = new ResultViewModel<TaskModel>(task);
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

    [SwaggerOperation(Summary = "Marks a task as undone")]
    [HttpPut("task/{id:int}/mark-as-undone")]
    public async Task<IActionResult> MarkAsUnDone(
        [FromServices] ITaskRepository repository,
        [FromRoute] int id
    )
    {
        try
        {
            var task = await repository.GetByIdAsync(id);
            if (task == null) throw new DbUpdateException($"The task with id {id} was not found");

            task.Done = false;
            task.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(task);

            var result = new ResultViewModel<TaskModel>(task);
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

    [SwaggerOperation(Summary = "Deletes a task")]
    [HttpDelete("task/{id:int}")]
    public async Task<IActionResult> Delete(
    [FromServices] ITaskRepository repository,
    [FromRoute] int id
)
    {
        try
        {
            var task = await repository.GetByIdAsync(id);
            if (task == null) throw new DbUpdateException($"The task with id '{id}' does not exist");

            await repository.DeleteAsync(task);

            var result = new ResultViewModel<TaskModel>(task);
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
