using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Gestao.Domain.Enums;
using Gestao.Domain.Libraries.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gestao.Data.Repositories
{
    public class FinanacialTransactionRepository : IFinanacialTransactionRepository
    {
        private readonly ApplicationDbContext _db;

        public FinanacialTransactionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedList<FinancialTransaction>> GetAllAsync(int companyId, TypeFinancialTransaction type, int pageIndex, int pageSize, string search = "")
        {
            Expression<Func<FinancialTransaction, bool>> filter =
                a => a.Description.Contains(search, StringComparison.OrdinalIgnoreCase) || a.Description.Contains(search, StringComparison.OrdinalIgnoreCase);

            List<FinancialTransaction> items = await _db.FinancialTransactions
                .Where(a => a.CompanyId == companyId && a.TypeFinancialTransaction == type)
                .Where(filter)
                .OrderByDescending(a => a.ReferenceDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int count = await _db.FinancialTransactions
                .Where(a => a.CompanyId == companyId && a.TypeFinancialTransaction == type)
                .Where(filter)
                .CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<FinancialTransaction>(items, pageIndex, totalPages);
        }

        public async Task<FinancialTransaction?> GetAsync(int id)
        {
            return await _db.FinancialTransactions
                .Include(f => f.Documents)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(FinancialTransaction entity)
        {
            _db.FinancialTransactions.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialTransaction entity)
        {
            _db.FinancialTransactions.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is not null)
            {
                _db.FinancialTransactions.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
