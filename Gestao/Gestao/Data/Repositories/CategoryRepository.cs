using Gestao.Client.Libraries.Utilities;
using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PaginatedList<Category>> GetAllAsync(int companyId, int pageIndex, int pageSize)
        {
            List<Category> items = await _db.Categories
                .Where(a => a.CompanyId == companyId)
                .OrderBy(a => a.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int count = await _db.Categories.Where(a => a.CompanyId == companyId).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<Category>(items, pageIndex, totalPages);
        }

        public async Task<List<Category>> GetAllAsync(int companyId)
        {
            return await _db.Categories.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<Category?> GetAsync(int id)
        {
            return await _db.Categories.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Category entity)
        {
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is not null)
            {
                _db.Categories.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
