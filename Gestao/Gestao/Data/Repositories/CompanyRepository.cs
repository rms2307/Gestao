using Gestao.Client.Libraries.Utilities;
using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gestao.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedList<Company>> GetAllAsync(Guid applicationUserId, int pageIndex, int pageSize, string search = "")
        {
            Expression<Func<Company, bool>> filter =
                a => a.TradeName.Contains(search) || a.LegalName.Contains(search);

            List<Company> items = await _db.Companies
                .Where(a => a.UserId == applicationUserId)
                .Where(filter)
                .OrderBy(a => a.TradeName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int count = await _db.Companies
                .Where(a => a.UserId == applicationUserId)
                .Where(filter)
                .CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<Company>(items, pageIndex, totalPages);
        }

        public async Task<Company?> GetAsync(int id)
        {
            return await _db.Companies.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Company company)
        {
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Company? company = await GetAsync(id);

            if (company is not null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }
        }
    }
}
