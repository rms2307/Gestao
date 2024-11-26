﻿using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Data.Repositories
{
    public class CompanyRepository
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

        public Company Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Company company)
        {

        }

        public void Update(Company company)
        {

        }

        public void Delete(int id)
        {

        }
    }
}
