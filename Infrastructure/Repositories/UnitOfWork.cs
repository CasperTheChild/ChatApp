using Application.Repository.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContextDb context;

    public UnitOfWork(ContextDb context)
    {
        this.context = context;
    }

    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync();
    }
}
