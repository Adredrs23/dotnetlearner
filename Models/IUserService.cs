

using System.Security.Claims;
using dotnetlearner.Models;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(ClaimsPrincipal userClaims);
}
