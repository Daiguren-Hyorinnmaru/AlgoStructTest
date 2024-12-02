using ServerAPI.DataBase.Repository;

namespace ServerAPI.DataBase.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}
