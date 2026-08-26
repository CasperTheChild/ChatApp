namespace Application.Repository.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}
