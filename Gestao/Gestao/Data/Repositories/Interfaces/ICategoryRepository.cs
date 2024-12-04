using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category entity);
        Task DeleteAsync(int id);
        Task<Category?> GetAsync(int id);
        Task<List<Category>> GetAllAsync(int companyId);
        Task<PaginatedList<Category>> GetAllAsync(int companyId, int pageIndex, int pageSize);
        Task UpdateAsync(Category entity);
    }
}