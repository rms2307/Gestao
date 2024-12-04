using Gestao.Domain;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task AddAsync(Document entity);
        Task DeleteAsync(int id);
        Task<Document?> GetAsync(int id);
        Task UpdateAsync(Document entity);
    }
}