using FIAPCloudGames.Users.Api.Features.Users.Models;

namespace FIAPCloudGames.Users.Api.Features.Users.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUniqueDocument(string document, CancellationToken cancellationToken = default);
}
