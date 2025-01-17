using Gestao.Domain;
using Gestao.Domain.Libraries.Utilities;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company);
        Task DeleteAsync(int id);
        Task<Company?> GetAsync(int id);
        Task<PaginatedList<Company>> GetAllAsync(Guid applicationUserId, int pageIndex, int pageSize, string search = "");
        Task UpdateAsync(Company company);
    }
}