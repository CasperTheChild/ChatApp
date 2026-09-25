using Domain.Common.Repository.Interfaces;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextDb context;

    public UnitOfWork(ContextDb context)
    {
        this.context = context;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
