using Gestao.Client.Libraries.Utilities;
using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedList<Company>> GetAllAsync(Guid applicationUserId, int pageIndex, int pageSize)
        {
            List<Company> items = await _db.Companies
                .Where(a => a.UserId == applicationUserId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int count = await _db.Companies.Where(a => a.UserId == applicationUserId).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<Company>(items, pageIndex, totalPages);
        }

        public async Task<Company?> Get(int id)
        {
            return await _db.Companies.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Company company)
        {
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Company company)
        {
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Company? company = await Get(id);

            if (company is not null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }
        }
    }
}
