using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Data;
using Todo.Models;
using Todo.Repositories.Contracts;
using Todo.Services;
using Todo.ViewModels;

namespace Todo.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [SwaggerOperation(Summary = "Creates a new user")]
    [HttpPost("v1/signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] CreateUserViewModel model,
        [FromServices] AppDbContext context,
        [FromServices] EmailService emailService
    )
    {
        try
        {
            var userExists = await context.Users.AnyAsync(x => x.Email == model.Email);
            if (userExists) throw new DbUpdateException("Username already registered");

            var user = new UserModel
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                subject: "Todo - Api. Your password, sir!",
                body: $"Your password is {model.Password}"
            );

            var result = new ResultViewModel<object>("User created successfully");
            return StatusCode(201, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(400, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Login user and returns a JWT key")]
    [HttpPost("v1/login")]
    public async Task<IActionResult> Login(
        [FromServices] AppDbContext context,
        [FromBody] UserViewModel model
    )
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null) throw new DbUpdateException("Username or password invalid");

            var passwordsAreNotEquals = !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
            if (passwordsAreNotEquals) throw new DbUpdateException("Username or password invalid");

            var token = _tokenService.GenerateToken(user);

            var result = new ResultViewModel<string>(token, null);
            return StatusCode(200, result);
        }
        catch (DbUpdateException ex)
        {
            var result = new ResultViewModel<string>(ex.Message);
            return StatusCode(400, result);
        }
        catch
        {
            var result = new ResultViewModel<string>("Internal server error");
            return StatusCode(500, result);
        }
    }

    [SwaggerOperation(Summary = "Assign user to role")]
    [HttpPost("v1/user/{userId:int}/role/{roleId:int}")]
    public async Task<IActionResult> AssignUserToRole(
        [FromServices] IRoleRepository roleRepository,
        [FromServices] IUserRepository userRepository,
        [FromRoute] int userId,
        [FromRoute] int roleId
    )
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null) throw new DbUpdateException($"User with id {userId} could not be found");

            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null) throw new DbUpdateException($"Role with id {roleId} could not be found");

            user.Roles.Add(role);

            var result = new ResultViewModel<string>($"The user {user.Name} has been atached to role {role.Name}");
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
}

