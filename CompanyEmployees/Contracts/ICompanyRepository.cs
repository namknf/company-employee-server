﻿using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters parms, bool trackChanges);

        Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);

        void CreateCompany(Company company);

        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteCompany(Company company);
    }
}
