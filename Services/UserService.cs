namespace dotnetlearner.Services;

using System.Security.Claims;
using System.Threading.Tasks;
using dotnetlearner.Data;
using dotnetlearner.Models;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<User> GetOrCreateUserAsync(ClaimsPrincipal userClaims)
    {
        var sub = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = userClaims.FindFirst(ClaimTypes.Email)?.Value;
        var firstName = userClaims.FindFirst(ClaimTypes.GivenName)?.Value ?? "Unknown";
        var lastName = userClaims.FindFirst(ClaimTypes.Surname)?.Value ?? "Unknown";


        if (sub == null || email == null)
            throw new Exception("Invalid token: missing sub or email");

        var parsedSub = Guid.Parse(sub);

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.KeycloakId == parsedSub);

        if (user == null)
        {
            user = new User
            {
                UserId = Guid.NewGuid(),
                KeycloakId = parsedSub,
                Email = email,
                FirstName = firstName,
                LastName = lastName,

            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        return user;
    }
}