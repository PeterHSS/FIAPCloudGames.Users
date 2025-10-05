using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Users.Api.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await context.Users.AddAsync(user, cancellationToken);

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Users.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Users.Include(usersGame => usersGame.Games).FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<bool> IsUniqueDocument(string document, CancellationToken cancellationToken = default)
        => !await context.Users.AsNoTracking().AnyAsync(user => user.Document == document, cancellationToken);

    public async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken = default) 
        => !await context.Users.AsNoTracking().AnyAsync(user => user.Email == email, cancellationToken);

    public void Update(User user) 
        => context.Update(user);
}
