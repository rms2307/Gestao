using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;

namespace Gestao.Data.Repositories.Interfaces
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