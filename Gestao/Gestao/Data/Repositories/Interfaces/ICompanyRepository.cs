using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);
        Task DeleteAsync(int id);
        Task<Company?> GetAsync(int id);
        Task<PaginatedList<Company>> GetAllAsync(Guid applicationUserId, int pageIndex, int pageSize);
        Task UpdateAsync(Company company);
    }
}