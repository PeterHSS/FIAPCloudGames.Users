using System.Security.Claims;
using FIAPCloudGames.Users.Api.Commom.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FIAPCloudGames.Users.Api.Infrastructure.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId 
        => Guid.TryParse(_httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid id)
            ? id
            : throw new InvalidOperationException("User ID not found in the current context.");
}
