using Gestao.Data.Repositories.Interfaces;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _db;

        public DocumentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Document?> GetAsync(int id)
        {
            return await _db.Documents.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Document entity)
        {
            _db.Documents.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Document entity)
        {
            _db.Documents.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is not null)
            {
                _db.Documents.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
