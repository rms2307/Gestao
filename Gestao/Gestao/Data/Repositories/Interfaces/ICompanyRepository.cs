﻿using Gestao.Client.Libraries.Utilities;
using Gestao.Domain;

namespace Gestao.Data.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task Add(Company company);
        Task Delete(int id);
        Task<Company?> Get(int id);
        Task<PaginatedList<Company>> GetAllAsync(Guid applicationUserId, int pageIndex, int pageSize);
        Task Update(Company company);
    }
}