using Gestao.Client.Libraries.Utilities;
using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedList<Account>> GetAllAsync(int companyId, int pageIndex, int pageSize)
        {
            List<Account> items = await _db.Accounts
                .Where(a => a.CompanyId == companyId)
                .OrderBy(a => a.Description)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int count = await _db.Accounts.Where(a => a.CompanyId == companyId).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<Account>(items, pageIndex, totalPages);
        }

        public async Task<List<Account>> GetAllAsync(int companyId)
        {
            return await _db.Accounts.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<Account?> GetAsync(int id)
        {
            return await _db.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Account entity)
        {
            _db.Accounts.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account entity)
        {
            _db.Accounts.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is not null)
            {
                _db.Accounts.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
