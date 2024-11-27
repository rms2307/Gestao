﻿using Gestao.Client.Libraries.Utilities;
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

        public async Task<PaginatedList<Account>> GetAll(int companyId, int pageIndex, int pageSize)
        {
            List<Account> items = await _db.Accounts.Where(a => a.CompanyId == companyId).
                Skip((pageIndex - 1) * pageSize).
                Take(pageSize)
                .ToListAsync();

            int count = await _db.Accounts.Where(a => a.CompanyId == companyId).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

            return new PaginatedList<Account>(items, pageIndex, totalPages);
        }

        public async Task<List<Account>> GetAll(int companyId)
        {
            return await _db.Accounts.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<Account?> Get(int id)
        {
            return await _db.Accounts.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Account entity)
        {
            _db.Accounts.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Account entity)
        {
            _db.Accounts.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);

            if (entity is not null)
            {
                _db.Accounts.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
