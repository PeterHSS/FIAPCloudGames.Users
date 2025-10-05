using System.Data;
using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace FIAPCloudGames.Users.Api.Infrastructure.Persistence.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext _context;

    public UnitOfWork(UserDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public IDbTransaction BeginTransaction(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction dbContextTransaction = _context.Database.BeginTransaction();

        return dbContextTransaction.GetDbTransaction();
    }
}
