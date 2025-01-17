using Gestao.Domain.Libraries.Utilities;

namespace Gestao.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task AddAsync(Account entity);
        Task DeleteAsync(int id);
        Task<Account?> GetAsync(int id);
        Task<List<Account>> GetAllAsync(int companyId);
        Task<PaginatedList<Account>> GetAllAsync(int companyId, int pageIndex, int pageSize, string search = "");
        Task UpdateAsync(Account entity);
    }
}