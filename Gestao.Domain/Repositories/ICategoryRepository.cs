using Gestao.Domain.Libraries.Utilities;

namespace Gestao.Domain.Repositories
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