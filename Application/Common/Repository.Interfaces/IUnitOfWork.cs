namespace Domain.Common.Repository.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}
