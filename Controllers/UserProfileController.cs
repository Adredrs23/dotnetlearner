namespace MyApp.Namespace;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public UserProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userService.GetOrCreateUserAsync(User);
        return Ok(user);
    }
}
