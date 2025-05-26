namespace dotnetlearner.Controllers;

using dotnetlearner.Data;
using dotnetlearner.Models;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllUsers), new { id = user.UserId }, user);
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        var name = User.Identity?.Name ?? "Unknown";
        return Ok(new { message = $"Hello {name}, you're authenticated!" });
    }

}
