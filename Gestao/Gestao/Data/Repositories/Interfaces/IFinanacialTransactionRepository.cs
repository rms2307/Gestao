using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;
using Gestao.Domain.Enums;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface IFinanacialTransactionRepository
    {
        Task AddAsync(FinancialTransaction entity);
        Task DeleteAsync(int id);
        Task<FinancialTransaction?> GetAsync(int id);
        Task<PaginatedList<FinancialTransaction>> GetAllAsync(int companyId, TypeFinancialTransaction type, int pageIndex, int pageSize);
        Task UpdateAsync(FinancialTransaction entity);
    }
}